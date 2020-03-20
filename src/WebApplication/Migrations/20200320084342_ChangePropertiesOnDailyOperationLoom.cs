using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class ChangePropertiesOnDailyOperationLoom : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Weaving_DailyOperationLoomBeamProducts");

            migrationBuilder.DropColumn(
                name: "LenoBrokenThreads",
                table: "Weaving_DailyOperationLoomHistories");

            migrationBuilder.DropColumn(
                name: "MachineNumber",
                table: "Weaving_DailyOperationLoomHistories");

            migrationBuilder.DropColumn(
                name: "ReprocessTo",
                table: "Weaving_DailyOperationLoomHistories");

            migrationBuilder.DropColumn(
                name: "WarpBrokenThreads",
                table: "Weaving_DailyOperationLoomHistories");

            migrationBuilder.DropColumn(
                name: "WeftBrokenThreads",
                table: "Weaving_DailyOperationLoomHistories");

            migrationBuilder.RenameColumn(
                name: "OperatorDocumentId",
                table: "Weaving_DailyOperationLoomHistories",
                newName: "TyingOperatorId");

            migrationBuilder.AddColumn<Guid>(
                name: "BeamDocumentId",
                table: "Weaving_DailyOperationLoomHistories",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<double>(
                name: "CounterPerOperator",
                table: "Weaving_DailyOperationLoomHistories",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<Guid>(
                name: "LoomMachineId",
                table: "Weaving_DailyOperationLoomHistories",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "LoomOperatorId",
                table: "Weaving_DailyOperationLoomHistories",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "TyingMachineId",
                table: "Weaving_DailyOperationLoomHistories",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "BeamProcessed",
                table: "Weaving_DailyOperationLoomDocuments",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "TotalCounter",
                table: "Weaving_DailyOperationLoomDocuments",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.CreateTable(
                name: "Weaving_DailyOperationLoomBeamsUsed",
                columns: table => new
                {
                    Identity = table.Column<Guid>(nullable: false),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 32, nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 32, nullable: true),
                    ModifiedDate = table.Column<DateTimeOffset>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    DeletedDate = table.Column<DateTimeOffset>(nullable: true),
                    DeletedBy = table.Column<string>(maxLength: 32, nullable: true),
                    BeamOrigin = table.Column<string>(nullable: true),
                    BeamDocumentId = table.Column<Guid>(nullable: false),
                    BeamNumber = table.Column<string>(nullable: true),
                    StartCounter = table.Column<double>(nullable: false),
                    FinishCounter = table.Column<double>(nullable: false),
                    MachineSpeed = table.Column<double>(nullable: false),
                    SCMPX = table.Column<double>(nullable: false),
                    Efficiency = table.Column<double>(nullable: false),
                    F = table.Column<double>(nullable: false),
                    W = table.Column<double>(nullable: false),
                    L = table.Column<double>(nullable: false),
                    T = table.Column<double>(nullable: false),
                    UomDocumentId = table.Column<int>(nullable: false),
                    UomUnit = table.Column<string>(nullable: true),
                    LastDateTimeProcessed = table.Column<DateTimeOffset>(nullable: false),
                    ShiftDocumentId = table.Column<Guid>(nullable: false),
                    Process = table.Column<string>(nullable: true),
                    BeamUsedStatus = table.Column<string>(nullable: true),
                    DailyOperationLoomDocumentId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Weaving_DailyOperationLoomBeamsUsed", x => x.Identity);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Weaving_DailyOperationLoomBeamsUsed");

            migrationBuilder.DropColumn(
                name: "BeamDocumentId",
                table: "Weaving_DailyOperationLoomHistories");

            migrationBuilder.DropColumn(
                name: "CounterPerOperator",
                table: "Weaving_DailyOperationLoomHistories");

            migrationBuilder.DropColumn(
                name: "LoomMachineId",
                table: "Weaving_DailyOperationLoomHistories");

            migrationBuilder.DropColumn(
                name: "LoomOperatorId",
                table: "Weaving_DailyOperationLoomHistories");

            migrationBuilder.DropColumn(
                name: "TyingMachineId",
                table: "Weaving_DailyOperationLoomHistories");

            migrationBuilder.DropColumn(
                name: "BeamProcessed",
                table: "Weaving_DailyOperationLoomDocuments");

            migrationBuilder.DropColumn(
                name: "TotalCounter",
                table: "Weaving_DailyOperationLoomDocuments");

            migrationBuilder.RenameColumn(
                name: "TyingOperatorId",
                table: "Weaving_DailyOperationLoomHistories",
                newName: "OperatorDocumentId");

            migrationBuilder.AddColumn<int>(
                name: "LenoBrokenThreads",
                table: "Weaving_DailyOperationLoomHistories",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MachineNumber",
                table: "Weaving_DailyOperationLoomHistories",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReprocessTo",
                table: "Weaving_DailyOperationLoomHistories",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WarpBrokenThreads",
                table: "Weaving_DailyOperationLoomHistories",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WeftBrokenThreads",
                table: "Weaving_DailyOperationLoomHistories",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Weaving_DailyOperationLoomBeamProducts",
                columns: table => new
                {
                    Identity = table.Column<Guid>(nullable: false),
                    BeamDocumentId = table.Column<Guid>(nullable: false),
                    BeamOrigin = table.Column<string>(nullable: true),
                    BeamProductStatus = table.Column<string>(nullable: true),
                    CombNumber = table.Column<double>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 32, nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(nullable: false),
                    DailyOperationLoomDocumentId = table.Column<Guid>(nullable: false),
                    Deleted = table.Column<bool>(nullable: true),
                    DeletedBy = table.Column<string>(maxLength: 32, nullable: true),
                    DeletedDate = table.Column<DateTimeOffset>(nullable: true),
                    LatestDateTimeBeamProduct = table.Column<DateTimeOffset>(nullable: false),
                    LoomProcess = table.Column<string>(nullable: true),
                    MachineDocumentId = table.Column<Guid>(nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 32, nullable: true),
                    ModifiedDate = table.Column<DateTimeOffset>(nullable: true),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Weaving_DailyOperationLoomBeamProducts", x => x.Identity);
                });
        }
    }
}
