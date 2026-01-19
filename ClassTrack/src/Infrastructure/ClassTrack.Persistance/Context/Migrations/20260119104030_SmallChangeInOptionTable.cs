using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClassTrack.Persistance.Context.Migrations
{
    /// <inheritdoc />
    public partial class SmallChangeInOptionTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Options_Questions_ChoiceQuestionId",
                table: "Options");

            migrationBuilder.RenameColumn(
                name: "ChoiceQuestionId",
                table: "Options",
                newName: "QuestionId");

            migrationBuilder.RenameIndex(
                name: "IX_Options_ChoiceQuestionId",
                table: "Options",
                newName: "IX_Options_QuestionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Options_Questions_QuestionId",
                table: "Options",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Options_Questions_QuestionId",
                table: "Options");

            migrationBuilder.RenameColumn(
                name: "QuestionId",
                table: "Options",
                newName: "ChoiceQuestionId");

            migrationBuilder.RenameIndex(
                name: "IX_Options_QuestionId",
                table: "Options",
                newName: "IX_Options_ChoiceQuestionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Options_Questions_ChoiceQuestionId",
                table: "Options",
                column: "ChoiceQuestionId",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
