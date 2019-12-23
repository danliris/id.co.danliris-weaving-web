using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class ChangeBeamNumberToBeamIdOnDailyOperationWarpingHistory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WarpingBeamNumber",
                table: "Weaving_DailyOperationWarpingHistories");

            migrationBuilder.AddColumn<Guid>(
                name: "WarpingBeamId",
                table: "Weaving_DailyOperationWarpingHistories",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WarpingBeamId",
                table: "Weaving_DailyOperationWarpingHistories");

            migrationBuilder.AddColumn<string>(
                name: "WarpingBeamNumber",
                table: "Weaving_DailyOperationWarpingHistories",
                nullable: true);
        }
    }
}
