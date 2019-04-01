using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class weaving12 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserOperatorId",
                table: "Weaving_MachinesPlanningDocuments",
                nullable: true,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserMaintenanceId",
                table: "Weaving_MachinesPlanningDocuments",
                nullable: true,
                oldClrType: typeof(int),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "UserOperatorId",
                table: "Weaving_MachinesPlanningDocuments",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "UserMaintenanceId",
                table: "Weaving_MachinesPlanningDocuments",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
