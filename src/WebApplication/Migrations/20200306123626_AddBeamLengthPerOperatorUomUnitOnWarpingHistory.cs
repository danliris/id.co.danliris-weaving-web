using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class AddBeamLengthPerOperatorUomUnitOnWarpingHistory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "WarpingBeamLengthPerOperatorUomUnit",
                table: "Weaving_DailyOperationWarpingHistories",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WarpingBeamLengthPerOperatorUomUnit",
                table: "Weaving_DailyOperationWarpingHistories");
        }
    }
}
