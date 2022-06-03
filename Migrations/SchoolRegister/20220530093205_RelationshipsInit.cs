using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolRegister.Migrations.SchoolRegister
{
    public partial class RelationshipsInit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentSubjects_Subjects_SubjectId",
                table: "StudentSubjects");

            migrationBuilder.DropColumn(
                name: "Age",
                table: "Students");

            migrationBuilder.RenameColumn(
                name: "SubjectId",
                table: "StudentSubjects",
                newName: "SchoolSubjectId");

            migrationBuilder.RenameIndex(
                name: "IX_StudentSubjects_SubjectId",
                table: "StudentSubjects",
                newName: "IX_StudentSubjects_SchoolSubjectId");

            migrationBuilder.AddColumn<int>(
                name: "TeacherId",
                table: "SchoolClasses",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "StudentId",
                table: "Grades",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateTable(
                name: "SchoolSubject",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubjectId = table.Column<int>(type: "int", nullable: false),
                    SchoolClassId = table.Column<int>(type: "int", nullable: false),
                    TeacherId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SchoolSubject", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SchoolSubject_SchoolClasses_SchoolClassId",
                        column: x => x.SchoolClassId,
                        principalTable: "SchoolClasses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SchoolSubject_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SchoolSubject_Teachers_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "Teachers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SchoolClasses_TeacherId",
                table: "SchoolClasses",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_Grades_StudentId",
                table: "Grades",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolSubject_SchoolClassId",
                table: "SchoolSubject",
                column: "SchoolClassId");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolSubject_SubjectId",
                table: "SchoolSubject",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolSubject_TeacherId",
                table: "SchoolSubject",
                column: "TeacherId");

            migrationBuilder.AddForeignKey(
                name: "FK_Grades_Students_StudentId",
                table: "Grades",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SchoolClasses_Teachers_TeacherId",
                table: "SchoolClasses",
                column: "TeacherId",
                principalTable: "Teachers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentSubjects_SchoolSubject_SchoolSubjectId",
                table: "StudentSubjects",
                column: "SchoolSubjectId",
                principalTable: "SchoolSubject",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Grades_Students_StudentId",
                table: "Grades");

            migrationBuilder.DropForeignKey(
                name: "FK_SchoolClasses_Teachers_TeacherId",
                table: "SchoolClasses");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentSubjects_SchoolSubject_SchoolSubjectId",
                table: "StudentSubjects");

            migrationBuilder.DropTable(
                name: "SchoolSubject");

            migrationBuilder.DropIndex(
                name: "IX_SchoolClasses_TeacherId",
                table: "SchoolClasses");

            migrationBuilder.DropIndex(
                name: "IX_Grades_StudentId",
                table: "Grades");

            migrationBuilder.DropColumn(
                name: "TeacherId",
                table: "SchoolClasses");

            migrationBuilder.RenameColumn(
                name: "SchoolSubjectId",
                table: "StudentSubjects",
                newName: "SubjectId");

            migrationBuilder.RenameIndex(
                name: "IX_StudentSubjects_SchoolSubjectId",
                table: "StudentSubjects",
                newName: "IX_StudentSubjects_SubjectId");

            migrationBuilder.AddColumn<int>(
                name: "Age",
                table: "Students",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "StudentId",
                table: "Grades",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentSubjects_Subjects_SubjectId",
                table: "StudentSubjects",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
