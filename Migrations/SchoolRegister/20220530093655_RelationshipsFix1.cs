using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolRegister.Migrations.SchoolRegister
{
    public partial class RelationshipsFix1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SchoolSubject_SchoolClasses_SchoolClassId",
                table: "SchoolSubject");

            migrationBuilder.DropForeignKey(
                name: "FK_SchoolSubject_Subjects_SubjectId",
                table: "SchoolSubject");

            migrationBuilder.DropForeignKey(
                name: "FK_SchoolSubject_Teachers_TeacherId",
                table: "SchoolSubject");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentSubjects_SchoolSubject_SchoolSubjectId",
                table: "StudentSubjects");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SchoolSubject",
                table: "SchoolSubject");

            migrationBuilder.RenameTable(
                name: "SchoolSubject",
                newName: "SchoolSubjects");

            migrationBuilder.RenameIndex(
                name: "IX_SchoolSubject_TeacherId",
                table: "SchoolSubjects",
                newName: "IX_SchoolSubjects_TeacherId");

            migrationBuilder.RenameIndex(
                name: "IX_SchoolSubject_SubjectId",
                table: "SchoolSubjects",
                newName: "IX_SchoolSubjects_SubjectId");

            migrationBuilder.RenameIndex(
                name: "IX_SchoolSubject_SchoolClassId",
                table: "SchoolSubjects",
                newName: "IX_SchoolSubjects_SchoolClassId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SchoolSubjects",
                table: "SchoolSubjects",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SchoolSubjects_SchoolClasses_SchoolClassId",
                table: "SchoolSubjects",
                column: "SchoolClassId",
                principalTable: "SchoolClasses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SchoolSubjects_Subjects_SubjectId",
                table: "SchoolSubjects",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SchoolSubjects_Teachers_TeacherId",
                table: "SchoolSubjects",
                column: "TeacherId",
                principalTable: "Teachers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentSubjects_SchoolSubjects_SchoolSubjectId",
                table: "StudentSubjects",
                column: "SchoolSubjectId",
                principalTable: "SchoolSubjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SchoolSubjects_SchoolClasses_SchoolClassId",
                table: "SchoolSubjects");

            migrationBuilder.DropForeignKey(
                name: "FK_SchoolSubjects_Subjects_SubjectId",
                table: "SchoolSubjects");

            migrationBuilder.DropForeignKey(
                name: "FK_SchoolSubjects_Teachers_TeacherId",
                table: "SchoolSubjects");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentSubjects_SchoolSubjects_SchoolSubjectId",
                table: "StudentSubjects");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SchoolSubjects",
                table: "SchoolSubjects");

            migrationBuilder.RenameTable(
                name: "SchoolSubjects",
                newName: "SchoolSubject");

            migrationBuilder.RenameIndex(
                name: "IX_SchoolSubjects_TeacherId",
                table: "SchoolSubject",
                newName: "IX_SchoolSubject_TeacherId");

            migrationBuilder.RenameIndex(
                name: "IX_SchoolSubjects_SubjectId",
                table: "SchoolSubject",
                newName: "IX_SchoolSubject_SubjectId");

            migrationBuilder.RenameIndex(
                name: "IX_SchoolSubjects_SchoolClassId",
                table: "SchoolSubject",
                newName: "IX_SchoolSubject_SchoolClassId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SchoolSubject",
                table: "SchoolSubject",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SchoolSubject_SchoolClasses_SchoolClassId",
                table: "SchoolSubject",
                column: "SchoolClassId",
                principalTable: "SchoolClasses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SchoolSubject_Subjects_SubjectId",
                table: "SchoolSubject",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SchoolSubject_Teachers_TeacherId",
                table: "SchoolSubject",
                column: "TeacherId",
                principalTable: "Teachers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentSubjects_SchoolSubject_SchoolSubjectId",
                table: "StudentSubjects",
                column: "SchoolSubjectId",
                principalTable: "SchoolSubject",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
