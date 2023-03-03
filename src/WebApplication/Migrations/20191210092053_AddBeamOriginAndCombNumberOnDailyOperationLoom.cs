using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class AddBeamOriginAndCombNumberOnDailyOperationLoom : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BeamOrigin",
                table: "Weaving_DailyOperationLoomBeamProducts",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "CombNumber",
                table: "Weaving_DailyOperationLoomBeamProducts",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BeamOrigin",
                table: "Weaving_DailyOperationLoomBeamProducts");

            migrationBuilder.DropColumn(
                name: "CombNumber",
                table: "Weaving_DailyOperationLoomBeamProducts");
        }
    }
}
