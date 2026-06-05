using BitirmeProjem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BitirmeProjem.Infrastructure.Persistence.Configurations;

public class RiskOptionConfiguration : IEntityTypeConfiguration<RiskOption>
{
    public void Configure(EntityTypeBuilder<RiskOption> builder)
    {
        builder.HasKey(o => o.Id);
        builder.Property(o => o.OptionTextTr).IsRequired().HasMaxLength(300);
        builder.Property(o => o.OptionTextEn).IsRequired().HasMaxLength(300);
        builder.Property(o => o.Score).IsRequired();

        builder.HasOne(o => o.RiskQuestion)
               .WithMany(q => q.Options)
               .HasForeignKey(o => o.RiskQuestionId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasData(
            // Soru 1
            new RiskOption { Id = Guid.Parse("22222222-2222-2222-2222-222222220101"), RiskQuestionId = Guid.Parse("11111111-1111-1111-1111-111111111101"), OptionTextTr = "Hemen satarım", OptionTextEn = "I sell immediately", Score = 1 },
            new RiskOption { Id = Guid.Parse("22222222-2222-2222-2222-222222220102"), RiskQuestionId = Guid.Parse("11111111-1111-1111-1111-111111111101"), OptionTextTr = "Bir süre beklerim", OptionTextEn = "I wait for a while", Score = 2 },
            new RiskOption { Id = Guid.Parse("22222222-2222-2222-2222-222222220103"), RiskQuestionId = Guid.Parse("11111111-1111-1111-1111-111111111101"), OptionTextTr = "Ek yatırım yapmayı düşünürüm", OptionTextEn = "I consider making additional investments", Score = 3 },
            new RiskOption { Id = Guid.Parse("22222222-2222-2222-2222-222222220104"), RiskQuestionId = Guid.Parse("11111111-1111-1111-1111-111111111101"), OptionTextTr = "Uzun vadeli düşünür, tutmaya devam ederim", OptionTextEn = "I think long-term and keep holding", Score = 4 },

            // Soru 2
            new RiskOption { Id = Guid.Parse("22222222-2222-2222-2222-222222220201"), RiskQuestionId = Guid.Parse("11111111-1111-1111-1111-111111111102"), OptionTextTr = "Paramın güvende kalması", OptionTextEn = "Keeping my money safe", Score = 1 },
            new RiskOption { Id = Guid.Parse("22222222-2222-2222-2222-222222220202"), RiskQuestionId = Guid.Parse("11111111-1111-1111-1111-111111111102"), OptionTextTr = "Dengeli büyüme", OptionTextEn = "Balanced growth", Score = 2 },
            new RiskOption { Id = Guid.Parse("22222222-2222-2222-2222-222222220203"), RiskQuestionId = Guid.Parse("11111111-1111-1111-1111-111111111102"), OptionTextTr = "Yüksek getiri elde etmek", OptionTextEn = "Achieving high returns", Score = 3 },
            new RiskOption { Id = Guid.Parse("22222222-2222-2222-2222-222222220204"), RiskQuestionId = Guid.Parse("11111111-1111-1111-1111-111111111102"), OptionTextTr = "Maksimum kazanç için risk almak", OptionTextEn = "Taking risks for maximum gain", Score = 4 },

            // Soru 3
            new RiskOption { Id = Guid.Parse("22222222-2222-2222-2222-222222220301"), RiskQuestionId = Guid.Parse("11111111-1111-1111-1111-111111111103"), OptionTextTr = "%5'ten az", OptionTextEn = "Less than 5%", Score = 1 },
            new RiskOption { Id = Guid.Parse("22222222-2222-2222-2222-222222220302"), RiskQuestionId = Guid.Parse("11111111-1111-1111-1111-111111111103"), OptionTextTr = "%5 - %15 arası", OptionTextEn = "Between 5% - 15%", Score = 2 },
            new RiskOption { Id = Guid.Parse("22222222-2222-2222-2222-222222220303"), RiskQuestionId = Guid.Parse("11111111-1111-1111-1111-111111111103"), OptionTextTr = "%15 - %30 arası", OptionTextEn = "Between 15% - 30%", Score = 3 },
            new RiskOption { Id = Guid.Parse("22222222-2222-2222-2222-222222220304"), RiskQuestionId = Guid.Parse("11111111-1111-1111-1111-111111111103"), OptionTextTr = "%30'dan fazla", OptionTextEn = "More than 30%", Score = 4 },

            // Soru 4
            new RiskOption { Id = Guid.Parse("22222222-2222-2222-2222-222222220401"), RiskQuestionId = Guid.Parse("11111111-1111-1111-1111-111111111104"), OptionTextTr = "1 yıldan kısa", OptionTextEn = "Less than 1 year", Score = 1 },
            new RiskOption { Id = Guid.Parse("22222222-2222-2222-2222-222222220402"), RiskQuestionId = Guid.Parse("11111111-1111-1111-1111-111111111104"), OptionTextTr = "1-3 yıl arası", OptionTextEn = "Between 1-3 years", Score = 2 },
            new RiskOption { Id = Guid.Parse("22222222-2222-2222-2222-222222220403"), RiskQuestionId = Guid.Parse("11111111-1111-1111-1111-111111111104"), OptionTextTr = "3-5 yıl arası", OptionTextEn = "Between 3-5 years", Score = 3 },
            new RiskOption { Id = Guid.Parse("22222222-2222-2222-2222-222222220404"), RiskQuestionId = Guid.Parse("11111111-1111-1111-1111-111111111104"), OptionTextTr = "5 yıldan uzun", OptionTextEn = "More than 5 years", Score = 4 },

            // Soru 5
            new RiskOption { Id = Guid.Parse("22222222-2222-2222-2222-222222220501"), RiskQuestionId = Guid.Parse("11111111-1111-1111-1111-111111111105"), OptionTextTr = "Çok az bilgim var", OptionTextEn = "I have very little knowledge", Score = 1 },
            new RiskOption { Id = Guid.Parse("22222222-2222-2222-2222-222222220502"), RiskQuestionId = Guid.Parse("11111111-1111-1111-1111-111111111105"), OptionTextTr = "Temel seviyede bilgim var", OptionTextEn = "I have basic knowledge", Score = 2 },
            new RiskOption { Id = Guid.Parse("22222222-2222-2222-2222-222222220503"), RiskQuestionId = Guid.Parse("11111111-1111-1111-1111-111111111105"), OptionTextTr = "Orta seviyede bilgim var", OptionTextEn = "I have intermediate knowledge", Score = 3 },
            new RiskOption { Id = Guid.Parse("22222222-2222-2222-2222-222222220504"), RiskQuestionId = Guid.Parse("11111111-1111-1111-1111-111111111105"), OptionTextTr = "İleri seviyede bilgim var", OptionTextEn = "I have advanced knowledge", Score = 4 },

            // Soru 6
            new RiskOption { Id = Guid.Parse("22222222-2222-2222-2222-222222220601"), RiskQuestionId = Guid.Parse("11111111-1111-1111-1111-111111111106"), OptionTextTr = "Vadeli mevduat", OptionTextEn = "Time deposit", Score = 1 },
            new RiskOption { Id = Guid.Parse("22222222-2222-2222-2222-222222220602"), RiskQuestionId = Guid.Parse("11111111-1111-1111-1111-111111111106"), OptionTextTr = "Altın / döviz", OptionTextEn = "Gold / foreign currency", Score = 2 },
            new RiskOption { Id = Guid.Parse("22222222-2222-2222-2222-222222220603"), RiskQuestionId = Guid.Parse("11111111-1111-1111-1111-111111111106"), OptionTextTr = "Hisse senedi fonları", OptionTextEn = "Stock funds", Score = 3 },
            new RiskOption { Id = Guid.Parse("22222222-2222-2222-2222-222222220604"), RiskQuestionId = Guid.Parse("11111111-1111-1111-1111-111111111106"), OptionTextTr = "Kripto / yüksek riskli yatırımlar", OptionTextEn = "Crypto / high-risk investments", Score = 4 },

            // Soru 7
            new RiskOption { Id = Guid.Parse("22222222-2222-2222-2222-222222220701"), RiskQuestionId = Guid.Parse("11111111-1111-1111-1111-111111111107"), OptionTextTr = "Tüm yatırımlarımı satarım", OptionTextEn = "I sell all my investments", Score = 1 },
            new RiskOption { Id = Guid.Parse("22222222-2222-2222-2222-222222220702"), RiskQuestionId = Guid.Parse("11111111-1111-1111-1111-111111111107"), OptionTextTr = "Riskimi azaltırım", OptionTextEn = "I reduce my risk", Score = 2 },
            new RiskOption { Id = Guid.Parse("22222222-2222-2222-2222-222222220703"), RiskQuestionId = Guid.Parse("11111111-1111-1111-1111-111111111107"), OptionTextTr = "Piyasayı takip ederim ve beklerim", OptionTextEn = "I monitor the market and wait", Score = 3 },
            new RiskOption { Id = Guid.Parse("22222222-2222-2222-2222-222222220704"), RiskQuestionId = Guid.Parse("11111111-1111-1111-1111-111111111107"), OptionTextTr = "Fırsat görüp yatırım artırırım", OptionTextEn = "I see it as an opportunity and increase investment", Score = 4 },

            // Soru 8
            new RiskOption { Id = Guid.Parse("22222222-2222-2222-2222-222222220801"), RiskQuestionId = Guid.Parse("11111111-1111-1111-1111-111111111108"), OptionTextTr = "Çok düşük risk", OptionTextEn = "Very low risk", Score = 1 },
            new RiskOption { Id = Guid.Parse("22222222-2222-2222-2222-222222220802"), RiskQuestionId = Guid.Parse("11111111-1111-1111-1111-111111111108"), OptionTextTr = "Kontrollü risk", OptionTextEn = "Controlled risk", Score = 2 },
            new RiskOption { Id = Guid.Parse("22222222-2222-2222-2222-222222220803"), RiskQuestionId = Guid.Parse("11111111-1111-1111-1111-111111111108"), OptionTextTr = "Orta-yüksek risk", OptionTextEn = "Medium-high risk", Score = 3 },
            new RiskOption { Id = Guid.Parse("22222222-2222-2222-2222-222222220804"), RiskQuestionId = Guid.Parse("11111111-1111-1111-1111-111111111108"), OptionTextTr = "Yüksek risk", OptionTextEn = "High risk", Score = 4 },

            // Soru 9
            new RiskOption { Id = Guid.Parse("22222222-2222-2222-2222-222222220901"), RiskQuestionId = Guid.Parse("11111111-1111-1111-1111-111111111109"), OptionTextTr = "Düşük ama garanti getiri", OptionTextEn = "Low but guaranteed return", Score = 1 },
            new RiskOption { Id = Guid.Parse("22222222-2222-2222-2222-222222220902"), RiskQuestionId = Guid.Parse("11111111-1111-1111-1111-111111111109"), OptionTextTr = "Enflasyon üstü getiri", OptionTextEn = "Return above inflation", Score = 2 },
            new RiskOption { Id = Guid.Parse("22222222-2222-2222-2222-222222220903"), RiskQuestionId = Guid.Parse("11111111-1111-1111-1111-111111111109"), OptionTextTr = "Yüksek büyüme potansiyeli", OptionTextEn = "High growth potential", Score = 3 },
            new RiskOption { Id = Guid.Parse("22222222-2222-2222-2222-222222220904"), RiskQuestionId = Guid.Parse("11111111-1111-1111-1111-111111111109"), OptionTextTr = "Çok yüksek kazanç ihtimali", OptionTextEn = "Very high earnings potential", Score = 4 },

            // Soru 10
            new RiskOption { Id = Guid.Parse("22222222-2222-2222-2222-222222221001"), RiskQuestionId = Guid.Parse("11111111-1111-1111-1111-111111111110"), OptionTextTr = "Güvenlik benim için en önemlisi", OptionTextEn = "Safety is most important to me", Score = 1 },
            new RiskOption { Id = Guid.Parse("22222222-2222-2222-2222-222222221002"), RiskQuestionId = Guid.Parse("11111111-1111-1111-1111-111111111110"), OptionTextTr = "Risk ve güven arasında denge kurarım", OptionTextEn = "I balance risk and security", Score = 2 },
            new RiskOption { Id = Guid.Parse("22222222-2222-2222-2222-222222221003"), RiskQuestionId = Guid.Parse("11111111-1111-1111-1111-111111111110"), OptionTextTr = "Kazanç için risk alabilirim", OptionTextEn = "I can take risks for profit", Score = 3 },
            new RiskOption { Id = Guid.Parse("22222222-2222-2222-2222-222222221004"), RiskQuestionId = Guid.Parse("11111111-1111-1111-1111-111111111110"), OptionTextTr = "Yüksek risk beni rahatsız etmez", OptionTextEn = "High risk doesn't bother me", Score = 4 }
        );
    }
}
