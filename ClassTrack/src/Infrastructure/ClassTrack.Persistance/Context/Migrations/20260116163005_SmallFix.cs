using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClassTrack.Persistance.Context.Migrations
{
    /// <inheritdoc />
    public partial class SmallFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuizAnswers_Quizes_QuizId",
                table: "QuizAnswers");

            migrationBuilder.DropIndex(
                name: "IX_QuizAnswers_QuizId",
                table: "QuizAnswers");

            migrationBuilder.DropColumn(
                name: "QuizId",
                table: "QuizAnswers");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "QuizId",
                table: "QuizAnswers",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_QuizAnswers_QuizId",
                table: "QuizAnswers",
                column: "QuizId");

            migrationBuilder.AddForeignKey(
                name: "FK_QuizAnswers_Quizes_QuizId",
                table: "QuizAnswers",
                column: "QuizId",
                principalTable: "Quizes",
                principalColumn: "Id");
        }
    }
}
