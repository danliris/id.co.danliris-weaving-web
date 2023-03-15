using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class changeDataTypeTimeperMinutes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "TimePerMinutes",
                table: "WeavingTroubleMachineTreeLoses",
                nullable: false,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "TimePerMinutes",
                table: "WeavingTroubleMachineTreeLoses",
                nullable: false,
                oldClrType: typeof(double));
        }
    }
}
