using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class ChangePressRollUomDataTypeToString : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PressRollUomId",
                table: "Weaving_DailyOperationWarpingBeamProducts");

            migrationBuilder.AddColumn<string>(
                name: "PressRollUom",
                table: "Weaving_DailyOperationWarpingBeamProducts",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PressRollUom",
                table: "Weaving_DailyOperationWarpingBeamProducts");

            migrationBuilder.AddColumn<int>(
                name: "PressRollUomId",
                table: "Weaving_DailyOperationWarpingBeamProducts",
                nullable: true);
        }
    }
}
