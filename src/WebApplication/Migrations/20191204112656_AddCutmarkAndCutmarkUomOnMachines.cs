using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class AddCutmarkAndCutmarkUomOnMachines : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Cutmark",
                table: "Weaving_MachineDocuments",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CutmarkUomId",
                table: "Weaving_MachineDocuments",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cutmark",
                table: "Weaving_MachineDocuments");

            migrationBuilder.DropColumn(
                name: "CutmarkUomId",
                table: "Weaving_MachineDocuments");
        }
    }
}
