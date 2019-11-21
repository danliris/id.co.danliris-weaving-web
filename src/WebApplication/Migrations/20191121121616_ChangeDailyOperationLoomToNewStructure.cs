using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class ChangeDailyOperationLoomToNewStructure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Weaving_DailyOperationLoomDetails");

            migrationBuilder.DropColumn(
                name: "BeamId",
                table: "Weaving_DailyOperationLoomDocuments");

            migrationBuilder.DropColumn(
                name: "MachineId",
                table: "Weaving_DailyOperationLoomDocuments");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "Weaving_DailyOperationLoomDocuments");

            migrationBuilder.DropColumn(
                name: "UnitId",
                table: "Weaving_DailyOperationLoomDocuments");

            migrationBuilder.DropColumn(
                name: "UsedYarn",
                table: "Weaving_DailyOperationLoomDocuments");

            migrationBuilder.RenameColumn(
                name: "DailyOperationStatus",
                table: "Weaving_DailyOperationLoomDocuments",
                newName: "OperationStatus");

            migrationBuilder.RenameColumn(
                name: "DailyOperationMonitoringId",
                table: "Weaving_DailyOperationLoomDocuments",
                newName: "OrderDocumentId");

            migrationBuilder.CreateTable(
                name: "Weaving_DailyOperationLoomBeamProducts",
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
                    GreigeLength = table.Column<double>(nullable: true),
                    LatestDateTimeBeamProduct = table.Column<DateTimeOffset>(nullable: false),
                    BeamProductStatus = table.Column<string>(nullable: true),
                    DailyOperationLoomDocumentId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Weaving_DailyOperationLoomBeamProducts", x => x.Identity);
                    table.ForeignKey(
                        name: "FK_Weaving_DailyOperationLoomBeamProducts_Weaving_DailyOperationLoomDocuments_DailyOperationLoomDocumentId",
                        column: x => x.DailyOperationLoomDocumentId,
                        principalTable: "Weaving_DailyOperationLoomDocuments",
                        principalColumn: "Identity",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Weaving_DailyOperationLoomHistories",
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
                    BeamDocumentId = table.Column<Guid>(nullable: false),
                    MachineDocumentId = table.Column<Guid>(nullable: false),
                    OperatorDocumentId = table.Column<Guid>(nullable: false),
                    DateTimeMachine = table.Column<DateTimeOffset>(nullable: false),
                    ShiftDocumentId = table.Column<Guid>(nullable: false),
                    Process = table.Column<string>(nullable: true),
                    Information = table.Column<string>(nullable: true),
                    MachineStatus = table.Column<string>(nullable: true),
                    DailyOperationLoomDocumentId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Weaving_DailyOperationLoomHistories", x => x.Identity);
                    table.ForeignKey(
                        name: "FK_Weaving_DailyOperationLoomHistories_Weaving_DailyOperationLoomDocuments_DailyOperationLoomDocumentId",
                        column: x => x.DailyOperationLoomDocumentId,
                        principalTable: "Weaving_DailyOperationLoomDocuments",
                        principalColumn: "Identity",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Weaving_DailyOperationLoomBeamProducts_DailyOperationLoomDocumentId",
                table: "Weaving_DailyOperationLoomBeamProducts",
                column: "DailyOperationLoomDocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_Weaving_DailyOperationLoomHistories_DailyOperationLoomDocumentId",
                table: "Weaving_DailyOperationLoomHistories",
                column: "DailyOperationLoomDocumentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Weaving_DailyOperationLoomBeamProducts");

            migrationBuilder.DropTable(
                name: "Weaving_DailyOperationLoomHistories");

            migrationBuilder.RenameColumn(
                name: "OrderDocumentId",
                table: "Weaving_DailyOperationLoomDocuments",
                newName: "DailyOperationMonitoringId");

            migrationBuilder.RenameColumn(
                name: "OperationStatus",
                table: "Weaving_DailyOperationLoomDocuments",
                newName: "DailyOperationStatus");

            migrationBuilder.AddColumn<Guid>(
                name: "BeamId",
                table: "Weaving_DailyOperationLoomDocuments",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "MachineId",
                table: "Weaving_DailyOperationLoomDocuments",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "OrderId",
                table: "Weaving_DailyOperationLoomDocuments",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "UnitId",
                table: "Weaving_DailyOperationLoomDocuments",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "UsedYarn",
                table: "Weaving_DailyOperationLoomDocuments",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Weaving_DailyOperationLoomDetails",
                columns: table => new
                {
                    Identity = table.Column<Guid>(nullable: false),
                    BeamOperatorId = table.Column<Guid>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 32, nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(nullable: false),
                    DailyOperationLoomDocumentId = table.Column<Guid>(nullable: false),
                    DateTimeOperation = table.Column<DateTimeOffset>(nullable: false),
                    Deleted = table.Column<bool>(nullable: true),
                    DeletedBy = table.Column<string>(maxLength: 32, nullable: true),
                    DeletedDate = table.Column<DateTimeOffset>(nullable: true),
                    IsDown = table.Column<bool>(nullable: false),
                    IsUp = table.Column<bool>(nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 32, nullable: true),
                    ModifiedDate = table.Column<DateTimeOffset>(nullable: true),
                    OperationStatus = table.Column<string>(nullable: true),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    ShiftId = table.Column<Guid>(nullable: false),
                    WarpOrigin = table.Column<string>(nullable: true),
                    WeftOrigin = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Weaving_DailyOperationLoomDetails", x => x.Identity);
                    table.ForeignKey(
                        name: "FK_Weaving_DailyOperationLoomDetails_Weaving_DailyOperationLoomDocuments_DailyOperationLoomDocumentId",
                        column: x => x.DailyOperationLoomDocumentId,
                        principalTable: "Weaving_DailyOperationLoomDocuments",
                        principalColumn: "Identity",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Weaving_DailyOperationLoomDetails_DailyOperationLoomDocumentId",
                table: "Weaving_DailyOperationLoomDetails",
                column: "DailyOperationLoomDocumentId");
        }
    }
}
