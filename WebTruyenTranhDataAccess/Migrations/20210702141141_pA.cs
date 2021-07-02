using Microsoft.EntityFrameworkCore.Migrations;

namespace WebTruyenTranhDataAccess.Migrations
{
    public partial class pA : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "IdentityRole",
                keyColumn: "Id",
                keyValue: "b3c95e22-44d7-40d2-9b8a-1cda3bea5e05");

            migrationBuilder.InsertData(
                table: "IdentityRole",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "1fa044d0-4b16-45b7-a497-e485e5075290", "ecfea90b-8987-42db-942e-ed6f2b2a95b8", "ADMIN", null });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "IdentityRole",
                keyColumn: "Id",
                keyValue: "1fa044d0-4b16-45b7-a497-e485e5075290");

            migrationBuilder.InsertData(
                table: "IdentityRole",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "b3c95e22-44d7-40d2-9b8a-1cda3bea5e05", "163952c7-c65d-434d-8fbc-27dc06db11a2", "ADMIN", null });
        }
    }
}
