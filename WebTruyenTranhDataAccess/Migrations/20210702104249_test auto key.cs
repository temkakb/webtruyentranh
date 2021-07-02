using Microsoft.EntityFrameworkCore.Migrations;

namespace WebTruyenTranhDataAccess.Migrations
{
    public partial class testautokey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "IdentityRole",
                keyColumn: "Id",
                keyValue: "ae973978-cc30-41ec-aa05-624efc60785f");

            migrationBuilder.InsertData(
                table: "IdentityRole",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "b3c95e22-44d7-40d2-9b8a-1cda3bea5e05", "163952c7-c65d-434d-8fbc-27dc06db11a2", "ADMIN", null });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "IdentityRole",
                keyColumn: "Id",
                keyValue: "b3c95e22-44d7-40d2-9b8a-1cda3bea5e05");

            migrationBuilder.InsertData(
                table: "IdentityRole",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "ae973978-cc30-41ec-aa05-624efc60785f", "4ba0158e-c9ed-47ce-b250-02c152ef4486", "ADMIN", null });
        }
    }
}
