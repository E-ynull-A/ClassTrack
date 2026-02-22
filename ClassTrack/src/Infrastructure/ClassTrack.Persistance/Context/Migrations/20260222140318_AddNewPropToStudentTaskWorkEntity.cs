using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClassTrack.Persistance.Context.Migrations
{
    /// <inheritdoc />
    public partial class AddNewPropToStudentTaskWorkEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StudentAnswer",
                table: "StudentTaskWorks",
                newName: "StudentAnswerText");

            migrationBuilder.AlterColumn<decimal>(
                name: "Point",
                table: "StudentTaskWorks",
                type: "DECIMAL(5,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "DECIMAL(5,2)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StudentAnswerLink",
                table: "StudentTaskWorks",
                type: "NVARCHAR(MAX)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StudentAnswerLink",
                table: "StudentTaskWorks");

            migrationBuilder.RenameColumn(
                name: "StudentAnswerText",
                table: "StudentTaskWorks",
                newName: "StudentAnswer");

            migrationBuilder.AlterColumn<decimal>(
                name: "Point",
                table: "StudentTaskWorks",
                type: "DECIMAL(5,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "DECIMAL(5,2)");
        }
    }
}
