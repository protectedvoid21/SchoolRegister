using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolRegister.Migrations.SchoolRegister
{
    public partial class SchoolSubjectManyToMany : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SchoolClasses_Teachers_TeacherId",
                table: "SchoolClasses");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentSubjects_SchoolSubjects_SchoolSubjectId",
                table: "StudentSubjects");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentSubjects_Students_StudentId",
                table: "StudentSubjects");

            migrationBuilder.DropForeignKey(
                name: "FK_Subjects_SchoolClasses_SchoolClassId",
                table: "Subjects");

            migrationBuilder.DropForeignKey(
                name: "FK_Subjects_Teachers_TeacherId",
                table: "Subjects");

            migrationBuilder.DropIndex(
                name: "IX_Subjects_SchoolClassId",
                table: "Subjects");

            migrationBuilder.DropIndex(
                name: "IX_Subjects_TeacherId",
                table: "Subjects");

            migrationBuilder.DropColumn(
                name: "SchoolClassId",
                table: "Subjects");

            migrationBuilder.DropColumn(
                name: "TeacherId",
                table: "Subjects");

            migrationBuilder.RenameColumn(
                name: "SchoolSubjectId",
                table: "StudentSubjects",
                newName: "SubjectId");

            migrationBuilder.RenameIndex(
                name: "IX_StudentSubjects_SchoolSubjectId",
                table: "StudentSubjects",
                newName: "IX_StudentSubjects_SubjectId");

            migrationBuilder.RenameColumn(
                name: "TeacherId",
                table: "SchoolClasses",
                newName: "SchoolClassId");

            migrationBuilder.RenameIndex(
                name: "IX_SchoolClasses_TeacherId",
                table: "SchoolClasses",
                newName: "IX_SchoolClasses_SchoolClassId");

            migrationBuilder.AlterColumn<int>(
                name: "StudentId",
                table: "StudentSubjects",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_SchoolClasses_SchoolClasses_SchoolClassId",
                table: "SchoolClasses",
                column: "SchoolClassId",
                principalTable: "SchoolClasses",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentSubjects_Students_StudentId",
                table: "StudentSubjects",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentSubjects_Subjects_SubjectId",
                table: "StudentSubjects",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SchoolClasses_SchoolClasses_SchoolClassId",
                table: "SchoolClasses");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentSubjects_Students_StudentId",
                table: "StudentSubjects");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentSubjects_Subjects_SubjectId",
                table: "StudentSubjects");

            migrationBuilder.RenameColumn(
                name: "SubjectId",
                table: "StudentSubjects",
                newName: "SchoolSubjectId");

            migrationBuilder.RenameIndex(
                name: "IX_StudentSubjects_SubjectId",
                table: "StudentSubjects",
                newName: "IX_StudentSubjects_SchoolSubjectId");

            migrationBuilder.RenameColumn(
                name: "SchoolClassId",
                table: "SchoolClasses",
                newName: "TeacherId");

            migrationBuilder.RenameIndex(
                name: "IX_SchoolClasses_SchoolClassId",
                table: "SchoolClasses",
                newName: "IX_SchoolClasses_TeacherId");

            migrationBuilder.AddColumn<int>(
                name: "SchoolClassId",
                table: "Subjects",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TeacherId",
                table: "Subjects",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "StudentId",
                table: "StudentSubjects",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Subjects_SchoolClassId",
                table: "Subjects",
                column: "SchoolClassId");

            migrationBuilder.CreateIndex(
                name: "IX_Subjects_TeacherId",
                table: "Subjects",
                column: "TeacherId");

            migrationBuilder.AddForeignKey(
                name: "FK_SchoolClasses_Teachers_TeacherId",
                table: "SchoolClasses",
                column: "TeacherId",
                principalTable: "Teachers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentSubjects_SchoolSubjects_SchoolSubjectId",
                table: "StudentSubjects",
                column: "SchoolSubjectId",
                principalTable: "SchoolSubjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentSubjects_Students_StudentId",
                table: "StudentSubjects",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Subjects_SchoolClasses_SchoolClassId",
                table: "Subjects",
                column: "SchoolClassId",
                principalTable: "SchoolClasses",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Subjects_Teachers_TeacherId",
                table: "Subjects",
                column: "TeacherId",
                principalTable: "Teachers",
                principalColumn: "Id");
        }
    }
}
