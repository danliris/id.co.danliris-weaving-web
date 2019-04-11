using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class weaving14 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Information",
                table: "Weaving_DailyOperationMachineDocuments");

            migrationBuilder.RenameColumn(
                name: "Time",
                table: "Weaving_DailyOperationMachineDocuments",
                newName: "DateOperated");

            migrationBuilder.RenameColumn(
                name: "Shift",
                table: "Weaving_DailyOperationMachineDetails",
                newName: "WeftsOrigin");

            migrationBuilder.RenameColumn(
                name: "OrderDocument",
                table: "Weaving_DailyOperationMachineDetails",
                newName: "Information");

            migrationBuilder.RenameColumn(
                name: "Beam",
                table: "Weaving_DailyOperationMachineDetails",
                newName: "WarpsOrigin");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Weaving_DailyOperationMachineDocuments",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DOMTime",
                table: "Weaving_DailyOperationMachineDetails",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 2000,
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "BeamDocumentId",
                table: "Weaving_DailyOperationMachineDetails",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "BeamOperatorDocumentId",
                table: "Weaving_DailyOperationMachineDetails",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DetailStatus",
                table: "Weaving_DailyOperationMachineDetails",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LoomGroup",
                table: "Weaving_DailyOperationMachineDetails",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "OrderDocumentId",
                table: "Weaving_DailyOperationMachineDetails",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ShiftDocumentId",
                table: "Weaving_DailyOperationMachineDetails",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SizingGroup",
                table: "Weaving_DailyOperationMachineDetails",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SizingOperatorDocumentId",
                table: "Weaving_DailyOperationMachineDetails",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Weaving_DailyOperationMachineDocuments");

            migrationBuilder.DropColumn(
                name: "BeamDocumentId",
                table: "Weaving_DailyOperationMachineDetails");

            migrationBuilder.DropColumn(
                name: "BeamOperatorDocumentId",
                table: "Weaving_DailyOperationMachineDetails");

            migrationBuilder.DropColumn(
                name: "DetailStatus",
                table: "Weaving_DailyOperationMachineDetails");

            migrationBuilder.DropColumn(
                name: "LoomGroup",
                table: "Weaving_DailyOperationMachineDetails");

            migrationBuilder.DropColumn(
                name: "OrderDocumentId",
                table: "Weaving_DailyOperationMachineDetails");

            migrationBuilder.DropColumn(
                name: "ShiftDocumentId",
                table: "Weaving_DailyOperationMachineDetails");

            migrationBuilder.DropColumn(
                name: "SizingGroup",
                table: "Weaving_DailyOperationMachineDetails");

            migrationBuilder.DropColumn(
                name: "SizingOperatorDocumentId",
                table: "Weaving_DailyOperationMachineDetails");

            migrationBuilder.RenameColumn(
                name: "DateOperated",
                table: "Weaving_DailyOperationMachineDocuments",
                newName: "Time");

            migrationBuilder.RenameColumn(
                name: "WeftsOrigin",
                table: "Weaving_DailyOperationMachineDetails",
                newName: "Shift");

            migrationBuilder.RenameColumn(
                name: "WarpsOrigin",
                table: "Weaving_DailyOperationMachineDetails",
                newName: "Beam");

            migrationBuilder.RenameColumn(
                name: "Information",
                table: "Weaving_DailyOperationMachineDetails",
                newName: "OrderDocument");

            migrationBuilder.AddColumn<string>(
                name: "Information",
                table: "Weaving_DailyOperationMachineDocuments",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DOMTime",
                table: "Weaving_DailyOperationMachineDetails",
                maxLength: 2000,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
