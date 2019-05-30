using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class dev_module_loom : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BeamDocumentId",
                table: "Weaving_DailyOperationSizingDetails");

            migrationBuilder.DropColumn(
                name: "BrokenBeam",
                table: "Weaving_DailyOperationSizingDetails");

            migrationBuilder.DropColumn(
                name: "ConstructionDocumentId",
                table: "Weaving_DailyOperationSizingDetails");

            migrationBuilder.DropColumn(
                name: "Counter",
                table: "Weaving_DailyOperationSizingDetails");

            migrationBuilder.DropColumn(
                name: "Information",
                table: "Weaving_DailyOperationSizingDetails");

            migrationBuilder.DropColumn(
                name: "PIS",
                table: "Weaving_DailyOperationSizingDetails");

            migrationBuilder.DropColumn(
                name: "TroubledMachine",
                table: "Weaving_DailyOperationSizingDetails");

            migrationBuilder.DropColumn(
                name: "Visco",
                table: "Weaving_DailyOperationSizingDetails");

            migrationBuilder.DropColumn(
                name: "DateOperated",
                table: "Weaving_DailyOperationLoomDocuments");

            migrationBuilder.DropColumn(
                name: "BeamId",
                table: "Weaving_DailyOperationLoomDetails");

            migrationBuilder.DropColumn(
                name: "DailyOperationLoomHistory",
                table: "Weaving_DailyOperationLoomDetails");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "Weaving_DailyOperationLoomDetails");

            migrationBuilder.DropColumn(
                name: "SizingOperatorId",
                table: "Weaving_DailyOperationLoomDetails");

            migrationBuilder.RenameColumn(
                name: "ProductionTime",
                table: "Weaving_DailyOperationSizingDetails",
                newName: "History");

            migrationBuilder.RenameColumn(
                name: "BeamTime",
                table: "Weaving_DailyOperationSizingDetails",
                newName: "Causes");

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "StartTime",
                table: "Weaving_ShiftDocuments",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "EndTime",
                table: "Weaving_ShiftDocuments",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ConstructionDocumentId",
                table: "Weaving_DailyOperationSizingDocuments",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Counter",
                table: "Weaving_DailyOperationSizingDocuments",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MachineSpeed",
                table: "Weaving_DailyOperationSizingDocuments",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PIS",
                table: "Weaving_DailyOperationSizingDocuments",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RecipeCode",
                table: "Weaving_DailyOperationSizingDocuments",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "SPU",
                table: "Weaving_DailyOperationSizingDocuments",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SizingBeamDocumentId",
                table: "Weaving_DailyOperationSizingDocuments",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "TexSQ",
                table: "Weaving_DailyOperationSizingDocuments",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Visco",
                table: "Weaving_DailyOperationSizingDocuments",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WarpingBeamCollectionDocumentId",
                table: "Weaving_DailyOperationSizingDocuments",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Weight",
                table: "Weaving_DailyOperationSizingDocuments",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "ShiftDocumentId",
                table: "Weaving_DailyOperationSizingDetails",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "OperatorDocumentId",
                table: "Weaving_DailyOperationSizingDetails",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<int>(
                name: "UnitId",
                table: "Weaving_DailyOperationLoomDocuments",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "MachineId",
                table: "Weaving_DailyOperationLoomDocuments",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "BeamId",
                table: "Weaving_DailyOperationLoomDocuments",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "DailyOperationMonitoringId",
                table: "Weaving_DailyOperationLoomDocuments",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "OrderId",
                table: "Weaving_DailyOperationLoomDocuments",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DateTimeOperation",
                table: "Weaving_DailyOperationLoomDetails",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<bool>(
                name: "IsDown",
                table: "Weaving_DailyOperationLoomDetails",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsUp",
                table: "Weaving_DailyOperationLoomDetails",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "OperationStatus",
                table: "Weaving_DailyOperationLoomDetails",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConstructionDocumentId",
                table: "Weaving_DailyOperationSizingDocuments");

            migrationBuilder.DropColumn(
                name: "Counter",
                table: "Weaving_DailyOperationSizingDocuments");

            migrationBuilder.DropColumn(
                name: "MachineSpeed",
                table: "Weaving_DailyOperationSizingDocuments");

            migrationBuilder.DropColumn(
                name: "PIS",
                table: "Weaving_DailyOperationSizingDocuments");

            migrationBuilder.DropColumn(
                name: "RecipeCode",
                table: "Weaving_DailyOperationSizingDocuments");

            migrationBuilder.DropColumn(
                name: "SPU",
                table: "Weaving_DailyOperationSizingDocuments");

            migrationBuilder.DropColumn(
                name: "SizingBeamDocumentId",
                table: "Weaving_DailyOperationSizingDocuments");

            migrationBuilder.DropColumn(
                name: "TexSQ",
                table: "Weaving_DailyOperationSizingDocuments");

            migrationBuilder.DropColumn(
                name: "Visco",
                table: "Weaving_DailyOperationSizingDocuments");

            migrationBuilder.DropColumn(
                name: "WarpingBeamCollectionDocumentId",
                table: "Weaving_DailyOperationSizingDocuments");

            migrationBuilder.DropColumn(
                name: "Weight",
                table: "Weaving_DailyOperationSizingDocuments");

            migrationBuilder.DropColumn(
                name: "OperatorDocumentId",
                table: "Weaving_DailyOperationSizingDetails");

            migrationBuilder.DropColumn(
                name: "BeamId",
                table: "Weaving_DailyOperationLoomDocuments");

            migrationBuilder.DropColumn(
                name: "DailyOperationMonitoringId",
                table: "Weaving_DailyOperationLoomDocuments");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "Weaving_DailyOperationLoomDocuments");

            migrationBuilder.DropColumn(
                name: "DateTimeOperation",
                table: "Weaving_DailyOperationLoomDetails");

            migrationBuilder.DropColumn(
                name: "IsDown",
                table: "Weaving_DailyOperationLoomDetails");

            migrationBuilder.DropColumn(
                name: "IsUp",
                table: "Weaving_DailyOperationLoomDetails");

            migrationBuilder.DropColumn(
                name: "OperationStatus",
                table: "Weaving_DailyOperationLoomDetails");

            migrationBuilder.RenameColumn(
                name: "History",
                table: "Weaving_DailyOperationSizingDetails",
                newName: "ProductionTime");

            migrationBuilder.RenameColumn(
                name: "Causes",
                table: "Weaving_DailyOperationSizingDetails",
                newName: "BeamTime");

            migrationBuilder.AlterColumn<string>(
                name: "StartTime",
                table: "Weaving_ShiftDocuments",
                nullable: true,
                oldClrType: typeof(TimeSpan));

            migrationBuilder.AlterColumn<string>(
                name: "EndTime",
                table: "Weaving_ShiftDocuments",
                nullable: true,
                oldClrType: typeof(TimeSpan));

            migrationBuilder.AlterColumn<Guid>(
                name: "ShiftDocumentId",
                table: "Weaving_DailyOperationSizingDetails",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AddColumn<Guid>(
                name: "BeamDocumentId",
                table: "Weaving_DailyOperationSizingDetails",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BrokenBeam",
                table: "Weaving_DailyOperationSizingDetails",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "ConstructionDocumentId",
                table: "Weaving_DailyOperationSizingDetails",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Counter",
                table: "Weaving_DailyOperationSizingDetails",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "Information",
                table: "Weaving_DailyOperationSizingDetails",
                maxLength: 2000,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PIS",
                table: "Weaving_DailyOperationSizingDetails",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TroubledMachine",
                table: "Weaving_DailyOperationSizingDetails",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Visco",
                table: "Weaving_DailyOperationSizingDetails",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "UnitId",
                table: "Weaving_DailyOperationLoomDocuments",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<Guid>(
                name: "MachineId",
                table: "Weaving_DailyOperationLoomDocuments",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DateOperated",
                table: "Weaving_DailyOperationLoomDocuments",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<Guid>(
                name: "BeamId",
                table: "Weaving_DailyOperationLoomDetails",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "DailyOperationLoomHistory",
                table: "Weaving_DailyOperationLoomDetails",
                maxLength: 2000,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "OrderId",
                table: "Weaving_DailyOperationLoomDetails",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "SizingOperatorId",
                table: "Weaving_DailyOperationLoomDetails",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }
    }
}
