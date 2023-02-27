using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class updateCOlumninWeavingDaityOperationWarpingMachine : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Day",
                table: "WeavingDailyOperationWarpingMachines");

            migrationBuilder.AddColumn<int>(
                name: "Date",
                table: "WeavingDailyOperationWarpingMachines",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "SP",
                table: "WeavingDailyOperationWarpingMachines",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WarpType",
                table: "WeavingDailyOperationWarpingMachines",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "YearSP",
                table: "WeavingDailyOperationWarpingMachines",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Date",
                table: "WeavingDailyOperationWarpingMachines");

            migrationBuilder.DropColumn(
                name: "SP",
                table: "WeavingDailyOperationWarpingMachines");

            migrationBuilder.DropColumn(
                name: "WarpType",
                table: "WeavingDailyOperationWarpingMachines");

            migrationBuilder.DropColumn(
                name: "YearSP",
                table: "WeavingDailyOperationWarpingMachines");

            migrationBuilder.AddColumn<DateTime>(
                name: "Day",
                table: "WeavingDailyOperationWarpingMachines",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
