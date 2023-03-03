using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class AddUomUnitOnWarpingBeamProductRemoveOperatorIdOnWarpingHistory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "OperatorDocumentId",
                table: "Weaving_DailyOperationWarpingHistories",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AddColumn<string>(
                name: "WarpingTotalBeamLengthUomUnit",
                table: "Weaving_DailyOperationWarpingBeamProducts",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WarpingTotalBeamLengthUomUnit",
                table: "Weaving_DailyOperationWarpingBeamProducts");

            migrationBuilder.AlterColumn<Guid>(
                name: "OperatorDocumentId",
                table: "Weaving_DailyOperationWarpingHistories",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);
        }
    }
}
