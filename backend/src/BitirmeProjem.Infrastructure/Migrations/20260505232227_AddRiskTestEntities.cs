using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BitirmeProjem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRiskTestEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RiskQuestions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrderIndex = table.Column<int>(type: "int", nullable: false),
                    QuestionTextTr = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    QuestionTextEn = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RiskQuestions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RiskTestResults",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TotalScore = table.Column<int>(type: "int", nullable: false),
                    RiskLevel = table.Column<int>(type: "int", nullable: false),
                    MLRiskLevel = table.Column<int>(type: "int", nullable: true),
                    CompletedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RiskTestResults", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RiskTestResults_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RiskOptions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RiskQuestionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OptionTextTr = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    OptionTextEn = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    Score = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RiskOptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RiskOptions_RiskQuestions_RiskQuestionId",
                        column: x => x.RiskQuestionId,
                        principalTable: "RiskQuestions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RiskTestAnswers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RiskTestResultId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RiskQuestionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RiskOptionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RiskTestAnswers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RiskTestAnswers_RiskOptions_RiskOptionId",
                        column: x => x.RiskOptionId,
                        principalTable: "RiskOptions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RiskTestAnswers_RiskQuestions_RiskQuestionId",
                        column: x => x.RiskQuestionId,
                        principalTable: "RiskQuestions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RiskTestAnswers_RiskTestResults_RiskTestResultId",
                        column: x => x.RiskTestResultId,
                        principalTable: "RiskTestResults",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RiskOptions_RiskQuestionId",
                table: "RiskOptions",
                column: "RiskQuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_RiskQuestions_OrderIndex",
                table: "RiskQuestions",
                column: "OrderIndex",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RiskTestAnswers_RiskOptionId",
                table: "RiskTestAnswers",
                column: "RiskOptionId");

            migrationBuilder.CreateIndex(
                name: "IX_RiskTestAnswers_RiskQuestionId",
                table: "RiskTestAnswers",
                column: "RiskQuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_RiskTestAnswers_RiskTestResultId",
                table: "RiskTestAnswers",
                column: "RiskTestResultId");

            migrationBuilder.CreateIndex(
                name: "IX_RiskTestResults_UserId",
                table: "RiskTestResults",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RiskTestAnswers");

            migrationBuilder.DropTable(
                name: "RiskOptions");

            migrationBuilder.DropTable(
                name: "RiskTestResults");

            migrationBuilder.DropTable(
                name: "RiskQuestions");
        }
    }
}
