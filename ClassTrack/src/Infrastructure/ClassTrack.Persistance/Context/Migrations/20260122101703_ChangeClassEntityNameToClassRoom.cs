using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClassTrack.Persistance.Context.Migrations
{
    /// <inheritdoc />
    public partial class ChangeClassEntityNameToClassRoom : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Quizes_Classes_ClassId",
                table: "Quizes");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentClasses_Classes_ClassId",
                table: "StudentClasses");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskWorks_Classes_ClassId",
                table: "TaskWorks");

            migrationBuilder.DropForeignKey(
                name: "FK_TeacherClasses_Classes_ClassId",
                table: "TeacherClasses");

            migrationBuilder.DropTable(
                name: "Classes");

            migrationBuilder.RenameColumn(
                name: "ClassId",
                table: "TeacherClasses",
                newName: "ClassRoomId");

            migrationBuilder.RenameIndex(
                name: "IX_TeacherClasses_ClassId",
                table: "TeacherClasses",
                newName: "IX_TeacherClasses_ClassRoomId");

            migrationBuilder.RenameColumn(
                name: "ClassId",
                table: "TaskWorks",
                newName: "ClassRoomId");

            migrationBuilder.RenameIndex(
                name: "IX_TaskWorks_ClassId",
                table: "TaskWorks",
                newName: "IX_TaskWorks_ClassRoomId");

            migrationBuilder.RenameColumn(
                name: "ClassId",
                table: "StudentClasses",
                newName: "ClassRoomId");

            migrationBuilder.RenameIndex(
                name: "IX_StudentClasses_ClassId",
                table: "StudentClasses",
                newName: "IX_StudentClasses_ClassRoomId");

            migrationBuilder.RenameColumn(
                name: "ClassId",
                table: "Quizes",
                newName: "ClassRoomId");

            migrationBuilder.RenameIndex(
                name: "IX_Quizes_ClassId",
                table: "Quizes",
                newName: "IX_Quizes_ClassRoomId");

            migrationBuilder.CreateTable(
                name: "ClassRooms",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AvgPoint = table.Column<decimal>(type: "DECIMAL(5,2)", nullable: false),
                    PrivateKey = table.Column<string>(type: "CHAR(8)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassRooms", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Quizes_ClassRooms_ClassRoomId",
                table: "Quizes",
                column: "ClassRoomId",
                principalTable: "ClassRooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentClasses_ClassRooms_ClassRoomId",
                table: "StudentClasses",
                column: "ClassRoomId",
                principalTable: "ClassRooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskWorks_ClassRooms_ClassRoomId",
                table: "TaskWorks",
                column: "ClassRoomId",
                principalTable: "ClassRooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TeacherClasses_ClassRooms_ClassRoomId",
                table: "TeacherClasses",
                column: "ClassRoomId",
                principalTable: "ClassRooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Quizes_ClassRooms_ClassRoomId",
                table: "Quizes");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentClasses_ClassRooms_ClassRoomId",
                table: "StudentClasses");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskWorks_ClassRooms_ClassRoomId",
                table: "TaskWorks");

            migrationBuilder.DropForeignKey(
                name: "FK_TeacherClasses_ClassRooms_ClassRoomId",
                table: "TeacherClasses");

            migrationBuilder.DropTable(
                name: "ClassRooms");

            migrationBuilder.RenameColumn(
                name: "ClassRoomId",
                table: "TeacherClasses",
                newName: "ClassId");

            migrationBuilder.RenameIndex(
                name: "IX_TeacherClasses_ClassRoomId",
                table: "TeacherClasses",
                newName: "IX_TeacherClasses_ClassId");

            migrationBuilder.RenameColumn(
                name: "ClassRoomId",
                table: "TaskWorks",
                newName: "ClassId");

            migrationBuilder.RenameIndex(
                name: "IX_TaskWorks_ClassRoomId",
                table: "TaskWorks",
                newName: "IX_TaskWorks_ClassId");

            migrationBuilder.RenameColumn(
                name: "ClassRoomId",
                table: "StudentClasses",
                newName: "ClassId");

            migrationBuilder.RenameIndex(
                name: "IX_StudentClasses_ClassRoomId",
                table: "StudentClasses",
                newName: "IX_StudentClasses_ClassId");

            migrationBuilder.RenameColumn(
                name: "ClassRoomId",
                table: "Quizes",
                newName: "ClassId");

            migrationBuilder.RenameIndex(
                name: "IX_Quizes_ClassRoomId",
                table: "Quizes",
                newName: "IX_Quizes_ClassId");

            migrationBuilder.CreateTable(
                name: "Classes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AvgPoint = table.Column<decimal>(type: "DECIMAL(5,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PrivateKey = table.Column<string>(type: "CHAR(8)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Classes", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Quizes_Classes_ClassId",
                table: "Quizes",
                column: "ClassId",
                principalTable: "Classes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentClasses_Classes_ClassId",
                table: "StudentClasses",
                column: "ClassId",
                principalTable: "Classes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskWorks_Classes_ClassId",
                table: "TaskWorks",
                column: "ClassId",
                principalTable: "Classes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TeacherClasses_Classes_ClassId",
                table: "TeacherClasses",
                column: "ClassId",
                principalTable: "Classes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
