using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class ChangeOnMachineTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Area",
                table: "Weaving_MachineDocuments",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Block",
                table: "Weaving_MachineDocuments",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Process",
                table: "Weaving_MachineDocuments",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Area",
                table: "Weaving_MachineDocuments");

            migrationBuilder.DropColumn(
                name: "Block",
                table: "Weaving_MachineDocuments");

            migrationBuilder.DropColumn(
                name: "Process",
                table: "Weaving_MachineDocuments");
        }
    }
}
