using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClassTrack.Persistance.Context.Migrations
{
    /// <inheritdoc />
    public partial class SmallPropNameFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_StudentTaskWorks",
                table: "StudentTaskWorks");

            migrationBuilder.DropColumn(
                name: "TaskId",
                table: "StudentTaskWorks");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StudentTaskWorks",
                table: "StudentTaskWorks",
                columns: new[] { "StudentId", "TaskWorkId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_StudentTaskWorks",
                table: "StudentTaskWorks");

            migrationBuilder.AddColumn<long>(
                name: "TaskId",
                table: "StudentTaskWorks",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddPrimaryKey(
                name: "PK_StudentTaskWorks",
                table: "StudentTaskWorks",
                columns: new[] { "StudentId", "TaskId" });
        }
    }
}
