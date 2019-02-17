using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class weavingversion1141 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalEstimationOrder",
                table: "Weaving_EstimationProductDocuments");

            migrationBuilder.AddColumn<double>(
                name: "TotalGramEstimation",
                table: "Weaving_EstimationDetails",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalGramEstimation",
                table: "Weaving_EstimationDetails");

            migrationBuilder.AddColumn<double>(
                name: "TotalEstimationOrder",
                table: "Weaving_EstimationProductDocuments",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
