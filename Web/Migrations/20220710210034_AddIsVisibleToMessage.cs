using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolRegister.Migrations
{
    public partial class AddIsVisibleToMessage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsVisible",
                table: "Messages",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsVisible",
                table: "Messages");
        }
    }
}
