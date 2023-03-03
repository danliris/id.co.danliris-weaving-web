using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class updateDataTypeThreadCut : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "ThreadCut",
                table: "WeavingDailyOperationWarpingMachines",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ThreadCut",
                table: "WeavingDailyOperationWarpingMachines",
                nullable: true,
                oldClrType: typeof(double));
        }
    }
}
