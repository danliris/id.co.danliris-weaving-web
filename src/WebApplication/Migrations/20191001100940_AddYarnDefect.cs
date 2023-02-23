using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class AddYarnDefect : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Weaving_DailyOperationWarpingBeamProduct_Weaving_DailyOperationWarpingDocuments_DailyOperationWarpingDocumentId",
                table: "Weaving_DailyOperationWarpingBeamProduct");

            migrationBuilder.DropForeignKey(
                name: "FK_Weaving_DailyOperationWarpingHistory_Weaving_DailyOperationWarpingDocuments_DailyOperationWarpingDocumentId",
                table: "Weaving_DailyOperationWarpingHistory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Weaving_DailyOperationWarpingHistory",
                table: "Weaving_DailyOperationWarpingHistory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Weaving_DailyOperationWarpingBeamProduct",
                table: "Weaving_DailyOperationWarpingBeamProduct");

            migrationBuilder.DropColumn(
                name: "ConstructionId",
                table: "Weaving_DailyOperationWarpingDocuments");

            migrationBuilder.DropColumn(
                name: "OperatorId",
                table: "Weaving_DailyOperationWarpingDocuments");

            migrationBuilder.DropColumn(
                name: "WeavingUnitId",
                table: "Weaving_DailyOperationReachingTyingDocuments");

            migrationBuilder.RenameTable(
                name: "Weaving_DailyOperationWarpingHistory",
                newName: "Weaving_DailyOperationWarpingHistories");

            migrationBuilder.RenameTable(
                name: "Weaving_DailyOperationWarpingBeamProduct",
                newName: "Weaving_DailyOperationWarpingBeamProducts");

            migrationBuilder.RenameColumn(
                name: "OrderId",
                table: "Weaving_DailyOperationWarpingDocuments",
                newName: "OrderDocumentId");

            migrationBuilder.RenameColumn(
                name: "DailyOperationStatus",
                table: "Weaving_DailyOperationWarpingDocuments",
                newName: "OperationStatus");

            migrationBuilder.RenameColumn(
                name: "ShiftId",
                table: "Weaving_DailyOperationWarpingHistories",
                newName: "ShiftDocumentId");

            migrationBuilder.RenameColumn(
                name: "OperationStatus",
                table: "Weaving_DailyOperationWarpingHistories",
                newName: "WarpingBeamNumber");

            migrationBuilder.RenameColumn(
                name: "DateTimeOperation",
                table: "Weaving_DailyOperationWarpingHistories",
                newName: "DateTimeMachine");

            migrationBuilder.RenameColumn(
                name: "BeamOperatorId",
                table: "Weaving_DailyOperationWarpingHistories",
                newName: "OperatorDocumentId");

            migrationBuilder.RenameColumn(
                name: "BeamNumber",
                table: "Weaving_DailyOperationWarpingHistories",
                newName: "MachineStatus");

            migrationBuilder.RenameIndex(
                name: "IX_Weaving_DailyOperationWarpingHistory_DailyOperationWarpingDocumentId",
                table: "Weaving_DailyOperationWarpingHistories",
                newName: "IX_Weaving_DailyOperationWarpingHistories_DailyOperationWarpingDocumentId");

            migrationBuilder.RenameColumn(
                name: "Speed",
                table: "Weaving_DailyOperationWarpingBeamProducts",
                newName: "RightLooseCreel");

            migrationBuilder.RenameColumn(
                name: "Length",
                table: "Weaving_DailyOperationWarpingBeamProducts",
                newName: "WarpingBeamLength");

            migrationBuilder.RenameColumn(
                name: "BeamId",
                table: "Weaving_DailyOperationWarpingBeamProducts",
                newName: "WarpingBeamId");

            migrationBuilder.RenameIndex(
                name: "IX_Weaving_DailyOperationWarpingBeamProduct_DailyOperationWarpingDocumentId",
                table: "Weaving_DailyOperationWarpingBeamProducts",
                newName: "IX_Weaving_DailyOperationWarpingBeamProducts_DailyOperationWarpingDocumentId");

            migrationBuilder.AddColumn<int>(
                name: "BrokenThreadsCause",
                table: "Weaving_DailyOperationWarpingBeamProducts",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ConeDeficient",
                table: "Weaving_DailyOperationWarpingBeamProducts",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "LatestDateTimeBeamProduct",
                table: "Weaving_DailyOperationWarpingBeamProducts",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<int>(
                name: "LeftLooseCreel",
                table: "Weaving_DailyOperationWarpingBeamProducts",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LooseThreadsAmount",
                table: "Weaving_DailyOperationWarpingBeamProducts",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MachineSpeed",
                table: "Weaving_DailyOperationWarpingBeamProducts",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Weaving_DailyOperationWarpingHistories",
                table: "Weaving_DailyOperationWarpingHistories",
                column: "Identity");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Weaving_DailyOperationWarpingBeamProducts",
                table: "Weaving_DailyOperationWarpingBeamProducts",
                column: "Identity");

            migrationBuilder.CreateTable(
                name: "Weaving_YarnDefectDocuments",
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
                    DefectCode = table.Column<string>(maxLength: 255, nullable: true),
                    DefectType = table.Column<string>(maxLength: 255, nullable: true),
                    DefectCategory = table.Column<string>(maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Weaving_YarnDefectDocuments", x => x.Identity);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Weaving_DailyOperationWarpingBeamProducts_Weaving_DailyOperationWarpingDocuments_DailyOperationWarpingDocumentId",
                table: "Weaving_DailyOperationWarpingBeamProducts",
                column: "DailyOperationWarpingDocumentId",
                principalTable: "Weaving_DailyOperationWarpingDocuments",
                principalColumn: "Identity",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Weaving_DailyOperationWarpingHistories_Weaving_DailyOperationWarpingDocuments_DailyOperationWarpingDocumentId",
                table: "Weaving_DailyOperationWarpingHistories",
                column: "DailyOperationWarpingDocumentId",
                principalTable: "Weaving_DailyOperationWarpingDocuments",
                principalColumn: "Identity",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Weaving_DailyOperationWarpingBeamProducts_Weaving_DailyOperationWarpingDocuments_DailyOperationWarpingDocumentId",
                table: "Weaving_DailyOperationWarpingBeamProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_Weaving_DailyOperationWarpingHistories_Weaving_DailyOperationWarpingDocuments_DailyOperationWarpingDocumentId",
                table: "Weaving_DailyOperationWarpingHistories");

            migrationBuilder.DropTable(
                name: "Weaving_YarnDefectDocuments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Weaving_DailyOperationWarpingHistories",
                table: "Weaving_DailyOperationWarpingHistories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Weaving_DailyOperationWarpingBeamProducts",
                table: "Weaving_DailyOperationWarpingBeamProducts");

            migrationBuilder.DropColumn(
                name: "BrokenThreadsCause",
                table: "Weaving_DailyOperationWarpingBeamProducts");

            migrationBuilder.DropColumn(
                name: "ConeDeficient",
                table: "Weaving_DailyOperationWarpingBeamProducts");

            migrationBuilder.DropColumn(
                name: "LatestDateTimeBeamProduct",
                table: "Weaving_DailyOperationWarpingBeamProducts");

            migrationBuilder.DropColumn(
                name: "LeftLooseCreel",
                table: "Weaving_DailyOperationWarpingBeamProducts");

            migrationBuilder.DropColumn(
                name: "LooseThreadsAmount",
                table: "Weaving_DailyOperationWarpingBeamProducts");

            migrationBuilder.DropColumn(
                name: "MachineSpeed",
                table: "Weaving_DailyOperationWarpingBeamProducts");

            migrationBuilder.RenameTable(
                name: "Weaving_DailyOperationWarpingHistories",
                newName: "Weaving_DailyOperationWarpingHistory");

            migrationBuilder.RenameTable(
                name: "Weaving_DailyOperationWarpingBeamProducts",
                newName: "Weaving_DailyOperationWarpingBeamProduct");

            migrationBuilder.RenameColumn(
                name: "OrderDocumentId",
                table: "Weaving_DailyOperationWarpingDocuments",
                newName: "OrderId");

            migrationBuilder.RenameColumn(
                name: "OperationStatus",
                table: "Weaving_DailyOperationWarpingDocuments",
                newName: "DailyOperationStatus");

            migrationBuilder.RenameColumn(
                name: "WarpingBeamNumber",
                table: "Weaving_DailyOperationWarpingHistory",
                newName: "OperationStatus");

            migrationBuilder.RenameColumn(
                name: "ShiftDocumentId",
                table: "Weaving_DailyOperationWarpingHistory",
                newName: "ShiftId");

            migrationBuilder.RenameColumn(
                name: "OperatorDocumentId",
                table: "Weaving_DailyOperationWarpingHistory",
                newName: "BeamOperatorId");

            migrationBuilder.RenameColumn(
                name: "MachineStatus",
                table: "Weaving_DailyOperationWarpingHistory",
                newName: "BeamNumber");

            migrationBuilder.RenameColumn(
                name: "DateTimeMachine",
                table: "Weaving_DailyOperationWarpingHistory",
                newName: "DateTimeOperation");

            migrationBuilder.RenameIndex(
                name: "IX_Weaving_DailyOperationWarpingHistories_DailyOperationWarpingDocumentId",
                table: "Weaving_DailyOperationWarpingHistory",
                newName: "IX_Weaving_DailyOperationWarpingHistory_DailyOperationWarpingDocumentId");

            migrationBuilder.RenameColumn(
                name: "WarpingBeamLength",
                table: "Weaving_DailyOperationWarpingBeamProduct",
                newName: "Length");

            migrationBuilder.RenameColumn(
                name: "WarpingBeamId",
                table: "Weaving_DailyOperationWarpingBeamProduct",
                newName: "BeamId");

            migrationBuilder.RenameColumn(
                name: "RightLooseCreel",
                table: "Weaving_DailyOperationWarpingBeamProduct",
                newName: "Speed");

            migrationBuilder.RenameIndex(
                name: "IX_Weaving_DailyOperationWarpingBeamProducts_DailyOperationWarpingDocumentId",
                table: "Weaving_DailyOperationWarpingBeamProduct",
                newName: "IX_Weaving_DailyOperationWarpingBeamProduct_DailyOperationWarpingDocumentId");

            migrationBuilder.AddColumn<Guid>(
                name: "ConstructionId",
                table: "Weaving_DailyOperationWarpingDocuments",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "OperatorId",
                table: "Weaving_DailyOperationWarpingDocuments",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "WeavingUnitId",
                table: "Weaving_DailyOperationReachingTyingDocuments",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Weaving_DailyOperationWarpingHistory",
                table: "Weaving_DailyOperationWarpingHistory",
                column: "Identity");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Weaving_DailyOperationWarpingBeamProduct",
                table: "Weaving_DailyOperationWarpingBeamProduct",
                column: "Identity");

            migrationBuilder.AddForeignKey(
                name: "FK_Weaving_DailyOperationWarpingBeamProduct_Weaving_DailyOperationWarpingDocuments_DailyOperationWarpingDocumentId",
                table: "Weaving_DailyOperationWarpingBeamProduct",
                column: "DailyOperationWarpingDocumentId",
                principalTable: "Weaving_DailyOperationWarpingDocuments",
                principalColumn: "Identity",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Weaving_DailyOperationWarpingHistory_Weaving_DailyOperationWarpingDocuments_DailyOperationWarpingDocumentId",
                table: "Weaving_DailyOperationWarpingHistory",
                column: "DailyOperationWarpingDocumentId",
                principalTable: "Weaving_DailyOperationWarpingDocuments",
                principalColumn: "Identity",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
