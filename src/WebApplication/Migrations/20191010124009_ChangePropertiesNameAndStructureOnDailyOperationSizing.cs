using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class ChangePropertiesNameAndStructureOnDailyOperationSizing : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Weaving_DailyOperationSizingBeamDocuments");

            migrationBuilder.DropTable(
                name: "Weaving_DailyOperationSizingDetails");

            migrationBuilder.CreateTable(
                name: "Weaving_DailyOperationSizingBeamProducts",
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
                    SizingBeamId = table.Column<Guid>(nullable: false),
                    DateTimeBeamDocument = table.Column<DateTimeOffset>(nullable: false),
                    CounterStart = table.Column<double>(nullable: false),
                    CounterFinish = table.Column<double>(nullable: false),
                    WeightNetto = table.Column<double>(nullable: false),
                    WeightBruto = table.Column<double>(nullable: false),
                    WeightTheoritical = table.Column<double>(nullable: false),
                    PISMeter = table.Column<double>(nullable: false),
                    SPU = table.Column<double>(nullable: false),
                    SizingBeamStatus = table.Column<string>(nullable: true),
                    DailyOperationSizingDocumentId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Weaving_DailyOperationSizingBeamProducts", x => x.Identity);
                    table.ForeignKey(
                        name: "FK_Weaving_DailyOperationSizingBeamProducts_Weaving_DailyOperationSizingDocuments_DailyOperationSizingDocumentId",
                        column: x => x.DailyOperationSizingDocumentId,
                        principalTable: "Weaving_DailyOperationSizingDocuments",
                        principalColumn: "Identity",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Weaving_DailyOperationSizingHistories",
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
                    ShiftDocumentId = table.Column<Guid>(nullable: false),
                    OperatorDocumentId = table.Column<Guid>(nullable: false),
                    DateTimeMachine = table.Column<DateTimeOffset>(nullable: false),
                    MachineStatus = table.Column<string>(nullable: true),
                    Information = table.Column<string>(nullable: true),
                    BrokenBeam = table.Column<int>(nullable: false),
                    MachineTroubled = table.Column<int>(nullable: false),
                    SizingBeamNumber = table.Column<string>(nullable: true),
                    DailyOperationSizingDocumentId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Weaving_DailyOperationSizingHistories", x => x.Identity);
                    table.ForeignKey(
                        name: "FK_Weaving_DailyOperationSizingHistories_Weaving_DailyOperationSizingDocuments_DailyOperationSizingDocumentId",
                        column: x => x.DailyOperationSizingDocumentId,
                        principalTable: "Weaving_DailyOperationSizingDocuments",
                        principalColumn: "Identity",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Weaving_DailyOperationSizingBeamProducts_DailyOperationSizingDocumentId",
                table: "Weaving_DailyOperationSizingBeamProducts",
                column: "DailyOperationSizingDocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_Weaving_DailyOperationSizingHistories_DailyOperationSizingDocumentId",
                table: "Weaving_DailyOperationSizingHistories",
                column: "DailyOperationSizingDocumentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Weaving_DailyOperationSizingBeamProducts");

            migrationBuilder.DropTable(
                name: "Weaving_DailyOperationSizingHistories");

            migrationBuilder.CreateTable(
                name: "Weaving_DailyOperationSizingBeamDocuments",
                columns: table => new
                {
                    Identity = table.Column<Guid>(nullable: false),
                    Counter = table.Column<string>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 32, nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(nullable: false),
                    DailyOperationSizingDocumentId = table.Column<Guid>(nullable: false),
                    DateTimeBeamDocument = table.Column<DateTimeOffset>(nullable: false),
                    Deleted = table.Column<bool>(nullable: true),
                    DeletedBy = table.Column<string>(maxLength: 32, nullable: true),
                    DeletedDate = table.Column<DateTimeOffset>(nullable: true),
                    ModifiedBy = table.Column<string>(maxLength: 32, nullable: true),
                    ModifiedDate = table.Column<DateTimeOffset>(nullable: true),
                    PISMeter = table.Column<double>(nullable: false),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    SPU = table.Column<double>(nullable: false),
                    SizingBeamId = table.Column<Guid>(nullable: false),
                    SizingBeamStatus = table.Column<string>(nullable: true),
                    Weight = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Weaving_DailyOperationSizingBeamDocuments", x => x.Identity);
                    table.ForeignKey(
                        name: "FK_Weaving_DailyOperationSizingBeamDocuments_Weaving_DailyOperationSizingDocuments_DailyOperationSizingDocumentId",
                        column: x => x.DailyOperationSizingDocumentId,
                        principalTable: "Weaving_DailyOperationSizingDocuments",
                        principalColumn: "Identity",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Weaving_DailyOperationSizingDetails",
                columns: table => new
                {
                    Identity = table.Column<Guid>(nullable: false),
                    Causes = table.Column<string>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 32, nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(nullable: false),
                    DailyOperationSizingDocumentId = table.Column<Guid>(nullable: false),
                    DateTimeMachine = table.Column<DateTimeOffset>(nullable: false),
                    Deleted = table.Column<bool>(nullable: true),
                    DeletedBy = table.Column<string>(maxLength: 32, nullable: true),
                    DeletedDate = table.Column<DateTimeOffset>(nullable: true),
                    Information = table.Column<string>(nullable: true),
                    MachineStatus = table.Column<string>(nullable: true),
                    ModifiedBy = table.Column<string>(maxLength: 32, nullable: true),
                    ModifiedDate = table.Column<DateTimeOffset>(nullable: true),
                    OperatorDocumentId = table.Column<Guid>(nullable: false),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    ShiftDocumentId = table.Column<Guid>(nullable: false),
                    SizingBeamNumber = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Weaving_DailyOperationSizingDetails", x => x.Identity);
                    table.ForeignKey(
                        name: "FK_Weaving_DailyOperationSizingDetails_Weaving_DailyOperationSizingDocuments_DailyOperationSizingDocumentId",
                        column: x => x.DailyOperationSizingDocumentId,
                        principalTable: "Weaving_DailyOperationSizingDocuments",
                        principalColumn: "Identity",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Weaving_DailyOperationSizingBeamDocuments_DailyOperationSizingDocumentId",
                table: "Weaving_DailyOperationSizingBeamDocuments",
                column: "DailyOperationSizingDocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_Weaving_DailyOperationSizingDetails_DailyOperationSizingDocumentId",
                table: "Weaving_DailyOperationSizingDetails",
                column: "DailyOperationSizingDocumentId");
        }
    }
}
