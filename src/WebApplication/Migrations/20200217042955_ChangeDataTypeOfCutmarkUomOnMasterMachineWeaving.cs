using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class ChangeDataTypeOfCutmarkUomOnMasterMachineWeaving : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CutmarkUomId",
                table: "Weaving_MachineDocuments");

            migrationBuilder.AddColumn<string>(
                name: "CutmarkUom",
                table: "Weaving_MachineDocuments",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CutmarkUom",
                table: "Weaving_MachineDocuments");

            migrationBuilder.AddColumn<int>(
                name: "CutmarkUomId",
                table: "Weaving_MachineDocuments",
                nullable: true);
        }
    }
}
