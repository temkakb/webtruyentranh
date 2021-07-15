using Microsoft.EntityFrameworkCore.Migrations;

namespace WebTruyenTranhDataAccess.Migrations
{
    public partial class AddEpTitle : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Episodes",
                type: "nvarchar(255)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Title",
                table: "Episodes");
        }
    }
}
