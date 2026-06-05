using BitirmeProjem.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BitirmeProjem.Application.Features.Notifications;

public record NotificationDto(string Type, string Message, int Value);

public record GetNotificationsQuery(Guid UserId) : IRequest<List<NotificationDto>>;

public class GetNotificationsQueryHandler : IRequestHandler<GetNotificationsQuery, List<NotificationDto>>
{
    private readonly IApplicationDbContext _context;

    public GetNotificationsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<NotificationDto>> Handle(GetNotificationsQuery request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .Include(u => u.FinancialProfile)
            .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

        if (user?.FinancialProfile is null)
            return [];

        var goals = await _context.Goals
            .Where(g => g.UserId == request.UserId)
            .ToListAsync(cancellationToken);

        var latestResult = await _context.RiskTestResults
            .Where(r => r.UserId == request.UserId)
            .OrderByDescending(r => r.CompletedAt)
            .FirstOrDefaultAsync(cancellationToken);

        var isTurkish = user.PreferredLanguage != "en";
        var fp = user.FinancialProfile;
        var notifications = new List<NotificationDto>();

        if (fp.MonthlyIncome == 0) return notifications;

        var income           = (double)fp.MonthlyIncome;
        var savingRate       = ((double)fp.MonthlyIncome - (double)fp.MonthlyExpenses) / income;
        var expenseRatio     = (double)fp.MonthlyExpenses / income;
        var debtToIncome     = (double)fp.DebtAmount / income;

        var savingPct  = (int)Math.Round(savingRate * 100);
        var expensePct = (int)Math.Round(expenseRatio * 100);
        var debtPct    = (int)Math.Round(debtToIncome * 100);

        // Tasarruf oranı
        if (savingRate >= 0.20)
            notifications.Add(new NotificationDto("success",
                isTurkish
                    ? $"Tasarruf oranınız iyi seviyede. Gelirinizin %{savingPct}'ini biriktiriyorsunuz."
                    : $"Your saving rate is great. You are saving {savingPct}% of your income.",
                savingPct));
        else
            notifications.Add(new NotificationDto("info",
                isTurkish
                    ? $"Tasarruf oranınız %{savingPct}. Gelirinizin en az %20'sini biriktirmeyi hedefleyin."
                    : $"Your saving rate is {savingPct}%. Aim to save at least 20% of your income.",
                savingPct));

        // Gider oranı
        if (expenseRatio > 0.80)
            notifications.Add(new NotificationDto("warning",
                isTurkish
                    ? $"Aylık giderleriniz gelirinizin %{expensePct}'ini oluşturuyor. Harcamalarınızı gözden geçirmenizi öneririz."
                    : $"Your monthly expenses account for {expensePct}% of your income. Consider reviewing your spending.",
                expensePct));
        else
            notifications.Add(new NotificationDto("info",
                isTurkish
                    ? $"Gider oranınız %{expensePct} seviyesinde ve kontrol altında."
                    : $"Your expense ratio is {expensePct}% and under control.",
                expensePct));

        // Borç/gelir oranı
        if (debtToIncome > 0.50)
            notifications.Add(new NotificationDto("warning",
                isTurkish
                    ? $"Borç/gelir oranınız %{debtPct} ile yüksek görünüyor. Borçlarınızı azaltmaya öncelik verin."
                    : $"Your debt-to-income ratio is {debtPct}%, which appears high. Prioritize reducing your debt.",
                debtPct));
        else
            notifications.Add(new NotificationDto("info",
                isTurkish
                    ? $"Borç/gelir oranınız %{debtPct} ile kontrol altında."
                    : $"Your debt-to-income ratio is {debtPct}% and under control.",
                debtPct));

        // Hedef deadline kontrolü
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        foreach (var goal in goals)
        {
            if (goal.Deadline is null || goal.TargetAmount == 0) continue;

            var daysLeft = goal.Deadline.Value.DayNumber - today.DayNumber;
            var progressPct = (int)Math.Round((double)(goal.CurrentAmount / goal.TargetAmount) * 100);

            if (daysLeft is > 0 and <= 90 && progressPct < 50)
                notifications.Add(new NotificationDto("warning",
                    isTurkish
                        ? $"'{goal.Title}' hedefinize {daysLeft} gün kaldı, ancak yalnızca %{progressPct} tamamlandı."
                        : $"You have {daysLeft} days left for '{goal.Title}', but only {progressPct}% is completed.",
                    progressPct));
            else if (daysLeft > 0 && progressPct >= 80)
                notifications.Add(new NotificationDto("success",
                    isTurkish
                        ? $"'{goal.Title}' hedefinize çok yakınsınız! %{progressPct} tamamlandı."
                        : $"You are very close to your goal '{goal.Title}'! {progressPct}% completed.",
                    progressPct));
        }

        // Risk testi yapılmamış
        if (latestResult is null)
            notifications.Add(new NotificationDto("info",
                isTurkish
                    ? "Henüz risk testi yapmadınız. Risk profilinizi belirlemek için testi tamamlayın."
                    : "You have not completed a risk assessment yet. Complete the test to determine your risk profile.",
                0));

        return notifications;
    }
}
