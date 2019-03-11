using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class weavingversion1149 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Unit",
                table: "Weaving_EstimationProductDocuments");

            migrationBuilder.AddColumn<int>(
                name: "UnitId",
                table: "Weaving_EstimationProductDocuments",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UnitId",
                table: "Weaving_EstimationProductDocuments");

            migrationBuilder.AddColumn<string>(
                name: "Unit",
                table: "Weaving_EstimationProductDocuments",
                maxLength: 255,
                nullable: true);
        }
    }
}
