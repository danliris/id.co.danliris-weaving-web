using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class updateCOlumnWeavingdailyOperationwarpingMachine : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Year",
                table: "WeavingDailyOperationWarpingMachines",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<string>(
                name: "ThreadCut",
                table: "WeavingDailyOperationWarpingMachines",
                nullable: true,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<string>(
                name: "Month",
                table: "WeavingDailyOperationWarpingMachines",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<string>(
                name: "AL",
                table: "WeavingDailyOperationWarpingMachines",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Construction",
                table: "WeavingDailyOperationWarpingMachines",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Group",
                table: "WeavingDailyOperationWarpingMachines",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "YearPeriode",
                table: "WeavingDailyOperationWarpingMachines",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AL",
                table: "WeavingDailyOperationWarpingMachines");

            migrationBuilder.DropColumn(
                name: "Construction",
                table: "WeavingDailyOperationWarpingMachines");

            migrationBuilder.DropColumn(
                name: "Group",
                table: "WeavingDailyOperationWarpingMachines");

            migrationBuilder.DropColumn(
                name: "YearPeriode",
                table: "WeavingDailyOperationWarpingMachines");

            migrationBuilder.AlterColumn<int>(
                name: "Year",
                table: "WeavingDailyOperationWarpingMachines",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "ThreadCut",
                table: "WeavingDailyOperationWarpingMachines",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Month",
                table: "WeavingDailyOperationWarpingMachines",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
