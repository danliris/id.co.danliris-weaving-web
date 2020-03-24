using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class AddPropertiesOnLoomBeamsUsed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ShiftDocumentId",
                table: "Weaving_DailyOperationLoomBeamsUsed",
                newName: "TyingMachineDocumentId");

            migrationBuilder.RenameColumn(
                name: "Process",
                table: "Weaving_DailyOperationLoomBeamsUsed",
                newName: "TyingMachineNumber");

            migrationBuilder.AddColumn<Guid>(
                name: "LastTyingOperatorDocumentId",
                table: "Weaving_DailyOperationLoomBeamsUsed",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "LastTyingOperatorName",
                table: "Weaving_DailyOperationLoomBeamsUsed",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LoomMachineNumber",
                table: "Weaving_DailyOperationLoomBeamsUsed",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastTyingOperatorDocumentId",
                table: "Weaving_DailyOperationLoomBeamsUsed");

            migrationBuilder.DropColumn(
                name: "LastTyingOperatorName",
                table: "Weaving_DailyOperationLoomBeamsUsed");

            migrationBuilder.DropColumn(
                name: "LoomMachineNumber",
                table: "Weaving_DailyOperationLoomBeamsUsed");

            migrationBuilder.RenameColumn(
                name: "TyingMachineNumber",
                table: "Weaving_DailyOperationLoomBeamsUsed",
                newName: "Process");

            migrationBuilder.RenameColumn(
                name: "TyingMachineDocumentId",
                table: "Weaving_DailyOperationLoomBeamsUsed",
                newName: "ShiftDocumentId");
        }
    }
}
