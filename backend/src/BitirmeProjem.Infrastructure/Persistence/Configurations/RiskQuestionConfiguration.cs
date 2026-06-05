using BitirmeProjem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BitirmeProjem.Infrastructure.Persistence.Configurations;

public class RiskQuestionConfiguration : IEntityTypeConfiguration<RiskQuestion>
{
    public void Configure(EntityTypeBuilder<RiskQuestion> builder)
    {
        builder.HasKey(q => q.Id);
        builder.Property(q => q.QuestionTextTr).IsRequired().HasMaxLength(500);
        builder.Property(q => q.QuestionTextEn).IsRequired().HasMaxLength(500);
        builder.Property(q => q.OrderIndex).IsRequired();
        builder.HasIndex(q => q.OrderIndex).IsUnique();

        builder.HasData(
            new RiskQuestion { Id = Guid.Parse("11111111-1111-1111-1111-111111111101"), OrderIndex = 1, QuestionTextTr = "Bir yatırımınız kısa sürede %20 değer kaybederse ne yaparsınız?", QuestionTextEn = "If one of your investments loses 20% of its value in a short time, what would you do?" },
            new RiskQuestion { Id = Guid.Parse("11111111-1111-1111-1111-111111111102"), OrderIndex = 2, QuestionTextTr = "Yatırım yaparken en önemli önceliğiniz nedir?", QuestionTextEn = "What is your most important priority when investing?" },
            new RiskQuestion { Id = Guid.Parse("11111111-1111-1111-1111-111111111103"), OrderIndex = 3, QuestionTextTr = "Aylık gelirinizin ne kadarını yatırım için ayırabilirsiniz?", QuestionTextEn = "What portion of your monthly income can you set aside for investment?" },
            new RiskQuestion { Id = Guid.Parse("11111111-1111-1111-1111-111111111104"), OrderIndex = 4, QuestionTextTr = "Yatırım sürenizi nasıl düşünüyorsunuz?", QuestionTextEn = "How do you think about your investment horizon?" },
            new RiskQuestion { Id = Guid.Parse("11111111-1111-1111-1111-111111111105"), OrderIndex = 5, QuestionTextTr = "Finansal piyasalardaki bilginizi nasıl değerlendirirsiniz?", QuestionTextEn = "How would you assess your knowledge of financial markets?" },
            new RiskQuestion { Id = Guid.Parse("11111111-1111-1111-1111-111111111106"), OrderIndex = 6, QuestionTextTr = "Aşağıdaki yatırım türlerinden hangisini tercih edersiniz?", QuestionTextEn = "Which of the following investment types do you prefer?" },
            new RiskQuestion { Id = Guid.Parse("11111111-1111-1111-1111-111111111107"), OrderIndex = 7, QuestionTextTr = "Beklenmedik bir ekonomik kriz olduğunda nasıl davranırsınız?", QuestionTextEn = "How would you react in an unexpected economic crisis?" },
            new RiskQuestion { Id = Guid.Parse("11111111-1111-1111-1111-111111111108"), OrderIndex = 8, QuestionTextTr = "Finansal hedefinize ulaşmak için ne kadar risk almaya hazırsınız?", QuestionTextEn = "How much risk are you willing to take to reach your financial goal?" },
            new RiskQuestion { Id = Guid.Parse("11111111-1111-1111-1111-111111111109"), OrderIndex = 9, QuestionTextTr = "Yatırımınızın yıllık performansı nasıl olursa sizi memnun eder?", QuestionTextEn = "What annual performance of your investment would satisfy you?" },
            new RiskQuestion { Id = Guid.Parse("11111111-1111-1111-1111-111111111110"), OrderIndex = 10, QuestionTextTr = "Aşağıdakilerden hangisi sizi daha iyi tanımlar?", QuestionTextEn = "Which of the following describes you best?" }
        );
    }
}
