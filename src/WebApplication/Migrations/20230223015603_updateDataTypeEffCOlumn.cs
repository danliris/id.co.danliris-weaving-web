using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class updateDataTypeEffCOlumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Eff",
                table: "WeavingDailyOperationWarpingMachines",
                nullable: true,
                oldClrType: typeof(double));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "Eff",
                table: "WeavingDailyOperationWarpingMachines",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
