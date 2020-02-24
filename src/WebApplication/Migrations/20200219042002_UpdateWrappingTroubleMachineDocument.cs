using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class UpdateWrappingTroubleMachineDocument : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WeavingUnitId",
                table: "Weaving_WarpingMachineTroubleDocuments");

            migrationBuilder.RenameColumn(
                name: "OrderDocumentId",
                table: "Weaving_WarpingMachineTroubleDocuments",
                newName: "OrderDocument");

            migrationBuilder.RenameColumn(
                name: "OperatorDocumentId",
                table: "Weaving_WarpingMachineTroubleDocuments",
                newName: "OperatorDocument");

            migrationBuilder.RenameColumn(
                name: "MachineDocumentId",
                table: "Weaving_WarpingMachineTroubleDocuments",
                newName: "MachineDocument");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OrderDocument",
                table: "Weaving_WarpingMachineTroubleDocuments",
                newName: "OrderDocumentId");

            migrationBuilder.RenameColumn(
                name: "OperatorDocument",
                table: "Weaving_WarpingMachineTroubleDocuments",
                newName: "OperatorDocumentId");

            migrationBuilder.RenameColumn(
                name: "MachineDocument",
                table: "Weaving_WarpingMachineTroubleDocuments",
                newName: "MachineDocumentId");

            migrationBuilder.AddColumn<int>(
                name: "WeavingUnitId",
                table: "Weaving_WarpingMachineTroubleDocuments",
                nullable: true);
        }
    }
}
