using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class weavingversion1133 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CoreCurrency",
                table: "Weaving_YarnDocuments");

            migrationBuilder.DropColumn(
                name: "CoreUom",
                table: "Weaving_YarnDocuments");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Weaving_YarnDocuments");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Weaving_YarnDocuments");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CoreCurrency",
                table: "Weaving_YarnDocuments",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CoreUom",
                table: "Weaving_YarnDocuments",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Weaving_YarnDocuments",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Price",
                table: "Weaving_YarnDocuments",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
