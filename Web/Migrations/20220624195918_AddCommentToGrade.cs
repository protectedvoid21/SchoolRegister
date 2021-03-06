using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolRegister.Migrations
{
    public partial class AddCommentToGrade : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Comment",
                table: "Grades",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Comment",
                table: "Grades");
        }
    }
}
