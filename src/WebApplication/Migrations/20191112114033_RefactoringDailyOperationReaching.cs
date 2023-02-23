using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class RefactoringDailyOperationReaching : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Weaving_DailyOperationReachingTyingDetails");

            migrationBuilder.DropTable(
                name: "Weaving_DailyOperationReachingTyingDocuments");

            migrationBuilder.CreateTable(
                name: "Weaving_DailyOperationReachingDocuments",
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
                    MachineDocumentId = table.Column<Guid>(nullable: true),
                    OrderDocumentId = table.Column<Guid>(nullable: true),
                    SizingBeamId = table.Column<Guid>(nullable: true),
                    ReachingInTypeInput = table.Column<string>(nullable: true),
                    ReachingInTypeOutput = table.Column<string>(nullable: true),
                    ReachingInWidth = table.Column<double>(nullable: false),
                    CombEdgeStitching = table.Column<int>(nullable: false),
                    CombNumber = table.Column<int>(nullable: false),
                    CombWidth = table.Column<double>(nullable: false),
                    OperationStatus = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Weaving_DailyOperationReachingDocuments", x => x.Identity);
                });

            migrationBuilder.CreateTable(
                name: "Weaving_DailyOperationReachingHistories",
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
                    OperatorDocumentId = table.Column<Guid>(nullable: false),
                    YarnStrandsProcessed = table.Column<int>(nullable: false),
                    DateTimeMachine = table.Column<DateTimeOffset>(nullable: false),
                    ShiftDocumentId = table.Column<Guid>(nullable: false),
                    MachineStatus = table.Column<string>(nullable: true),
                    DailyOperationReachingDocumentId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Weaving_DailyOperationReachingHistories", x => x.Identity);
                    table.ForeignKey(
                        name: "FK_Weaving_DailyOperationReachingHistories_Weaving_DailyOperationReachingDocuments_DailyOperationReachingDocumentId",
                        column: x => x.DailyOperationReachingDocumentId,
                        principalTable: "Weaving_DailyOperationReachingDocuments",
                        principalColumn: "Identity",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Weaving_DailyOperationReachingHistories_DailyOperationReachingDocumentId",
                table: "Weaving_DailyOperationReachingHistories",
                column: "DailyOperationReachingDocumentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Weaving_DailyOperationReachingHistories");

            migrationBuilder.DropTable(
                name: "Weaving_DailyOperationReachingDocuments");

            migrationBuilder.CreateTable(
                name: "Weaving_DailyOperationReachingTyingDocuments",
                columns: table => new
                {
                    Identity = table.Column<Guid>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 32, nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(nullable: false),
                    Deleted = table.Column<bool>(nullable: true),
                    DeletedBy = table.Column<string>(maxLength: 32, nullable: true),
                    DeletedDate = table.Column<DateTimeOffset>(nullable: true),
                    MachineDocumentId = table.Column<Guid>(nullable: true),
                    ModifiedBy = table.Column<string>(maxLength: 32, nullable: true),
                    ModifiedDate = table.Column<DateTimeOffset>(nullable: true),
                    OperationStatus = table.Column<string>(nullable: true),
                    OrderDocumentId = table.Column<Guid>(nullable: true),
                    ReachingValueObjects = table.Column<string>(nullable: true),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    SizingBeamId = table.Column<Guid>(nullable: true),
                    TyingValueObjects = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Weaving_DailyOperationReachingTyingDocuments", x => x.Identity);
                });

            migrationBuilder.CreateTable(
                name: "Weaving_DailyOperationReachingTyingDetails",
                columns: table => new
                {
                    Identity = table.Column<Guid>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 32, nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(nullable: false),
                    DailyOperationReachingTyingDocumentId = table.Column<Guid>(nullable: false),
                    DateTimeMachine = table.Column<DateTimeOffset>(nullable: false),
                    Deleted = table.Column<bool>(nullable: true),
                    DeletedBy = table.Column<string>(maxLength: 32, nullable: true),
                    DeletedDate = table.Column<DateTimeOffset>(nullable: true),
                    MachineStatus = table.Column<string>(nullable: true),
                    ModifiedBy = table.Column<string>(maxLength: 32, nullable: true),
                    ModifiedDate = table.Column<DateTimeOffset>(nullable: true),
                    OperatorDocumentId = table.Column<Guid>(nullable: false),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    ShiftDocumentId = table.Column<Guid>(nullable: false),
                    YarnStrandsProcessed = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Weaving_DailyOperationReachingTyingDetails", x => x.Identity);
                    table.ForeignKey(
                        name: "FK_Weaving_DailyOperationReachingTyingDetails_Weaving_DailyOperationReachingTyingDocuments_DailyOperationReachingTyingDocumentId",
                        column: x => x.DailyOperationReachingTyingDocumentId,
                        principalTable: "Weaving_DailyOperationReachingTyingDocuments",
                        principalColumn: "Identity",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Weaving_DailyOperationReachingTyingDetails_DailyOperationReachingTyingDocumentId",
                table: "Weaving_DailyOperationReachingTyingDetails",
                column: "DailyOperationReachingTyingDocumentId");
        }
    }
}
