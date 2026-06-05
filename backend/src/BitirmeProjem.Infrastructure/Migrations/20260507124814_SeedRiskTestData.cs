using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BitirmeProjem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedRiskTestData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "RiskQuestions",
                columns: new[] { "Id", "OrderIndex", "QuestionTextEn", "QuestionTextTr" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111101"), 1, "If one of your investments loses 20% of its value in a short time, what would you do?", "Bir yatırımınız kısa sürede %20 değer kaybederse ne yaparsınız?" },
                    { new Guid("11111111-1111-1111-1111-111111111102"), 2, "What is your most important priority when investing?", "Yatırım yaparken en önemli önceliğiniz nedir?" },
                    { new Guid("11111111-1111-1111-1111-111111111103"), 3, "What portion of your monthly income can you set aside for investment?", "Aylık gelirinizin ne kadarını yatırım için ayırabilirsiniz?" },
                    { new Guid("11111111-1111-1111-1111-111111111104"), 4, "How do you think about your investment horizon?", "Yatırım sürenizi nasıl düşünüyorsunuz?" },
                    { new Guid("11111111-1111-1111-1111-111111111105"), 5, "How would you assess your knowledge of financial markets?", "Finansal piyasalardaki bilginizi nasıl değerlendirirsiniz?" },
                    { new Guid("11111111-1111-1111-1111-111111111106"), 6, "Which of the following investment types do you prefer?", "Aşağıdaki yatırım türlerinden hangisini tercih edersiniz?" },
                    { new Guid("11111111-1111-1111-1111-111111111107"), 7, "How would you react in an unexpected economic crisis?", "Beklenmedik bir ekonomik kriz olduğunda nasıl davranırsınız?" },
                    { new Guid("11111111-1111-1111-1111-111111111108"), 8, "How much risk are you willing to take to reach your financial goal?", "Finansal hedefinize ulaşmak için ne kadar risk almaya hazırsınız?" },
                    { new Guid("11111111-1111-1111-1111-111111111109"), 9, "What annual performance of your investment would satisfy you?", "Yatırımınızın yıllık performansı nasıl olursa sizi memnun eder?" },
                    { new Guid("11111111-1111-1111-1111-111111111110"), 10, "Which of the following describes you best?", "Aşağıdakilerden hangisi sizi daha iyi tanımlar?" }
                });

            migrationBuilder.InsertData(
                table: "RiskOptions",
                columns: new[] { "Id", "OptionTextEn", "OptionTextTr", "RiskQuestionId", "Score" },
                values: new object[,]
                {
                    { new Guid("22222222-2222-2222-2222-222222220101"), "I sell immediately", "Hemen satarım", new Guid("11111111-1111-1111-1111-111111111101"), 1 },
                    { new Guid("22222222-2222-2222-2222-222222220102"), "I wait for a while", "Bir süre beklerim", new Guid("11111111-1111-1111-1111-111111111101"), 2 },
                    { new Guid("22222222-2222-2222-2222-222222220103"), "I consider making additional investments", "Ek yatırım yapmayı düşünürüm", new Guid("11111111-1111-1111-1111-111111111101"), 3 },
                    { new Guid("22222222-2222-2222-2222-222222220104"), "I think long-term and keep holding", "Uzun vadeli düşünür, tutmaya devam ederim", new Guid("11111111-1111-1111-1111-111111111101"), 4 },
                    { new Guid("22222222-2222-2222-2222-222222220201"), "Keeping my money safe", "Paramın güvende kalması", new Guid("11111111-1111-1111-1111-111111111102"), 1 },
                    { new Guid("22222222-2222-2222-2222-222222220202"), "Balanced growth", "Dengeli büyüme", new Guid("11111111-1111-1111-1111-111111111102"), 2 },
                    { new Guid("22222222-2222-2222-2222-222222220203"), "Achieving high returns", "Yüksek getiri elde etmek", new Guid("11111111-1111-1111-1111-111111111102"), 3 },
                    { new Guid("22222222-2222-2222-2222-222222220204"), "Taking risks for maximum gain", "Maksimum kazanç için risk almak", new Guid("11111111-1111-1111-1111-111111111102"), 4 },
                    { new Guid("22222222-2222-2222-2222-222222220301"), "Less than 5%", "%5'ten az", new Guid("11111111-1111-1111-1111-111111111103"), 1 },
                    { new Guid("22222222-2222-2222-2222-222222220302"), "Between 5% - 15%", "%5 - %15 arası", new Guid("11111111-1111-1111-1111-111111111103"), 2 },
                    { new Guid("22222222-2222-2222-2222-222222220303"), "Between 15% - 30%", "%15 - %30 arası", new Guid("11111111-1111-1111-1111-111111111103"), 3 },
                    { new Guid("22222222-2222-2222-2222-222222220304"), "More than 30%", "%30'dan fazla", new Guid("11111111-1111-1111-1111-111111111103"), 4 },
                    { new Guid("22222222-2222-2222-2222-222222220401"), "Less than 1 year", "1 yıldan kısa", new Guid("11111111-1111-1111-1111-111111111104"), 1 },
                    { new Guid("22222222-2222-2222-2222-222222220402"), "Between 1-3 years", "1-3 yıl arası", new Guid("11111111-1111-1111-1111-111111111104"), 2 },
                    { new Guid("22222222-2222-2222-2222-222222220403"), "Between 3-5 years", "3-5 yıl arası", new Guid("11111111-1111-1111-1111-111111111104"), 3 },
                    { new Guid("22222222-2222-2222-2222-222222220404"), "More than 5 years", "5 yıldan uzun", new Guid("11111111-1111-1111-1111-111111111104"), 4 },
                    { new Guid("22222222-2222-2222-2222-222222220501"), "I have very little knowledge", "Çok az bilgim var", new Guid("11111111-1111-1111-1111-111111111105"), 1 },
                    { new Guid("22222222-2222-2222-2222-222222220502"), "I have basic knowledge", "Temel seviyede bilgim var", new Guid("11111111-1111-1111-1111-111111111105"), 2 },
                    { new Guid("22222222-2222-2222-2222-222222220503"), "I have intermediate knowledge", "Orta seviyede bilgim var", new Guid("11111111-1111-1111-1111-111111111105"), 3 },
                    { new Guid("22222222-2222-2222-2222-222222220504"), "I have advanced knowledge", "İleri seviyede bilgim var", new Guid("11111111-1111-1111-1111-111111111105"), 4 },
                    { new Guid("22222222-2222-2222-2222-222222220601"), "Time deposit", "Vadeli mevduat", new Guid("11111111-1111-1111-1111-111111111106"), 1 },
                    { new Guid("22222222-2222-2222-2222-222222220602"), "Gold / foreign currency", "Altın / döviz", new Guid("11111111-1111-1111-1111-111111111106"), 2 },
                    { new Guid("22222222-2222-2222-2222-222222220603"), "Stock funds", "Hisse senedi fonları", new Guid("11111111-1111-1111-1111-111111111106"), 3 },
                    { new Guid("22222222-2222-2222-2222-222222220604"), "Crypto / high-risk investments", "Kripto / yüksek riskli yatırımlar", new Guid("11111111-1111-1111-1111-111111111106"), 4 },
                    { new Guid("22222222-2222-2222-2222-222222220701"), "I sell all my investments", "Tüm yatırımlarımı satarım", new Guid("11111111-1111-1111-1111-111111111107"), 1 },
                    { new Guid("22222222-2222-2222-2222-222222220702"), "I reduce my risk", "Riskimi azaltırım", new Guid("11111111-1111-1111-1111-111111111107"), 2 },
                    { new Guid("22222222-2222-2222-2222-222222220703"), "I monitor the market and wait", "Piyasayı takip ederim ve beklerim", new Guid("11111111-1111-1111-1111-111111111107"), 3 },
                    { new Guid("22222222-2222-2222-2222-222222220704"), "I see it as an opportunity and increase investment", "Fırsat görüp yatırım artırırım", new Guid("11111111-1111-1111-1111-111111111107"), 4 },
                    { new Guid("22222222-2222-2222-2222-222222220801"), "Very low risk", "Çok düşük risk", new Guid("11111111-1111-1111-1111-111111111108"), 1 },
                    { new Guid("22222222-2222-2222-2222-222222220802"), "Controlled risk", "Kontrollü risk", new Guid("11111111-1111-1111-1111-111111111108"), 2 },
                    { new Guid("22222222-2222-2222-2222-222222220803"), "Medium-high risk", "Orta-yüksek risk", new Guid("11111111-1111-1111-1111-111111111108"), 3 },
                    { new Guid("22222222-2222-2222-2222-222222220804"), "High risk", "Yüksek risk", new Guid("11111111-1111-1111-1111-111111111108"), 4 },
                    { new Guid("22222222-2222-2222-2222-222222220901"), "Low but guaranteed return", "Düşük ama garanti getiri", new Guid("11111111-1111-1111-1111-111111111109"), 1 },
                    { new Guid("22222222-2222-2222-2222-222222220902"), "Return above inflation", "Enflasyon üstü getiri", new Guid("11111111-1111-1111-1111-111111111109"), 2 },
                    { new Guid("22222222-2222-2222-2222-222222220903"), "High growth potential", "Yüksek büyüme potansiyeli", new Guid("11111111-1111-1111-1111-111111111109"), 3 },
                    { new Guid("22222222-2222-2222-2222-222222220904"), "Very high earnings potential", "Çok yüksek kazanç ihtimali", new Guid("11111111-1111-1111-1111-111111111109"), 4 },
                    { new Guid("22222222-2222-2222-2222-222222221001"), "Safety is most important to me", "Güvenlik benim için en önemlisi", new Guid("11111111-1111-1111-1111-111111111110"), 1 },
                    { new Guid("22222222-2222-2222-2222-222222221002"), "I balance risk and security", "Risk ve güven arasında denge kurarım", new Guid("11111111-1111-1111-1111-111111111110"), 2 },
                    { new Guid("22222222-2222-2222-2222-222222221003"), "I can take risks for profit", "Kazanç için risk alabilirim", new Guid("11111111-1111-1111-1111-111111111110"), 3 },
                    { new Guid("22222222-2222-2222-2222-222222221004"), "High risk doesn't bother me", "Yüksek risk beni rahatsız etmez", new Guid("11111111-1111-1111-1111-111111111110"), 4 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "RiskOptions",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222220101"));

            migrationBuilder.DeleteData(
                table: "RiskOptions",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222220102"));

            migrationBuilder.DeleteData(
                table: "RiskOptions",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222220103"));

            migrationBuilder.DeleteData(
                table: "RiskOptions",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222220104"));

            migrationBuilder.DeleteData(
                table: "RiskOptions",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222220201"));

            migrationBuilder.DeleteData(
                table: "RiskOptions",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222220202"));

            migrationBuilder.DeleteData(
                table: "RiskOptions",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222220203"));

            migrationBuilder.DeleteData(
                table: "RiskOptions",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222220204"));

            migrationBuilder.DeleteData(
                table: "RiskOptions",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222220301"));

            migrationBuilder.DeleteData(
                table: "RiskOptions",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222220302"));

            migrationBuilder.DeleteData(
                table: "RiskOptions",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222220303"));

            migrationBuilder.DeleteData(
                table: "RiskOptions",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222220304"));

            migrationBuilder.DeleteData(
                table: "RiskOptions",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222220401"));

            migrationBuilder.DeleteData(
                table: "RiskOptions",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222220402"));

            migrationBuilder.DeleteData(
                table: "RiskOptions",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222220403"));

            migrationBuilder.DeleteData(
                table: "RiskOptions",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222220404"));

            migrationBuilder.DeleteData(
                table: "RiskOptions",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222220501"));

            migrationBuilder.DeleteData(
                table: "RiskOptions",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222220502"));

            migrationBuilder.DeleteData(
                table: "RiskOptions",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222220503"));

            migrationBuilder.DeleteData(
                table: "RiskOptions",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222220504"));

            migrationBuilder.DeleteData(
                table: "RiskOptions",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222220601"));

            migrationBuilder.DeleteData(
                table: "RiskOptions",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222220602"));

            migrationBuilder.DeleteData(
                table: "RiskOptions",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222220603"));

            migrationBuilder.DeleteData(
                table: "RiskOptions",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222220604"));

            migrationBuilder.DeleteData(
                table: "RiskOptions",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222220701"));

            migrationBuilder.DeleteData(
                table: "RiskOptions",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222220702"));

            migrationBuilder.DeleteData(
                table: "RiskOptions",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222220703"));

            migrationBuilder.DeleteData(
                table: "RiskOptions",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222220704"));

            migrationBuilder.DeleteData(
                table: "RiskOptions",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222220801"));

            migrationBuilder.DeleteData(
                table: "RiskOptions",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222220802"));

            migrationBuilder.DeleteData(
                table: "RiskOptions",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222220803"));

            migrationBuilder.DeleteData(
                table: "RiskOptions",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222220804"));

            migrationBuilder.DeleteData(
                table: "RiskOptions",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222220901"));

            migrationBuilder.DeleteData(
                table: "RiskOptions",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222220902"));

            migrationBuilder.DeleteData(
                table: "RiskOptions",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222220903"));

            migrationBuilder.DeleteData(
                table: "RiskOptions",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222220904"));

            migrationBuilder.DeleteData(
                table: "RiskOptions",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222221001"));

            migrationBuilder.DeleteData(
                table: "RiskOptions",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222221002"));

            migrationBuilder.DeleteData(
                table: "RiskOptions",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222221003"));

            migrationBuilder.DeleteData(
                table: "RiskOptions",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222221004"));

            migrationBuilder.DeleteData(
                table: "RiskQuestions",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111101"));

            migrationBuilder.DeleteData(
                table: "RiskQuestions",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111102"));

            migrationBuilder.DeleteData(
                table: "RiskQuestions",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111103"));

            migrationBuilder.DeleteData(
                table: "RiskQuestions",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111104"));

            migrationBuilder.DeleteData(
                table: "RiskQuestions",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111105"));

            migrationBuilder.DeleteData(
                table: "RiskQuestions",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111106"));

            migrationBuilder.DeleteData(
                table: "RiskQuestions",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111107"));

            migrationBuilder.DeleteData(
                table: "RiskQuestions",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111108"));

            migrationBuilder.DeleteData(
                table: "RiskQuestions",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111109"));

            migrationBuilder.DeleteData(
                table: "RiskQuestions",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111110"));
        }
    }
}
