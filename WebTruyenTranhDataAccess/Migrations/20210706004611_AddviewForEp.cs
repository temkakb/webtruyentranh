using Microsoft.EntityFrameworkCore.Migrations;

namespace WebTruyenTranhDataAccess.Migrations
{
    public partial class AddviewForEp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "IdentityRole",
                keyColumn: "Id",
                keyValue: "b8e52e87-9ca3-4605-924e-399ab30eadc6");

            migrationBuilder.AddColumn<int>(
                name: "Views",
                table: "Episodes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "IdentityRole",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "8449f0ff-4a50-4444-aeea-6c4541b518d3", "a2563e77-737e-4fe7-ae1d-3de4e9028565", "ADMIN", null });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "IdentityRole",
                keyColumn: "Id",
                keyValue: "8449f0ff-4a50-4444-aeea-6c4541b518d3");

            migrationBuilder.DropColumn(
                name: "Views",
                table: "Episodes");

            migrationBuilder.InsertData(
                table: "IdentityRole",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "b8e52e87-9ca3-4605-924e-399ab30eadc6", "95168ac3-77e4-4c23-9a19-1bd9d73af3b6", "ADMIN", null });
        }
    }
}
