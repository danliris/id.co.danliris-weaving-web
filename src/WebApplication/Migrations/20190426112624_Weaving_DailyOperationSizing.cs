using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class Weaving_DailyOperationSizing : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Weaving_DailyOperationSizingDocuments",
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
                    ProductionDate = table.Column<DateTimeOffset>(nullable: false),
                    MachineDocumentId = table.Column<Guid>(nullable: true),
                    WeavingUnitId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Weaving_DailyOperationSizingDocuments", x => x.Identity);
                });

            migrationBuilder.CreateTable(
                name: "Weaving_DailyOperationSizingDetails",
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
                    BeamDocumentId = table.Column<Guid>(nullable: true),
                    ConstructionDocumentId = table.Column<Guid>(nullable: true),
                    PIS = table.Column<int>(nullable: false),
                    Visco = table.Column<string>(maxLength: 255, nullable: true),
                    ProductionTime = table.Column<string>(nullable: true),
                    BeamTime = table.Column<string>(nullable: true),
                    BrokenBeam = table.Column<int>(nullable: false),
                    TroubledMachine = table.Column<int>(nullable: false),
                    Counter = table.Column<double>(nullable: false),
                    ShiftDocumentId = table.Column<Guid>(nullable: true),
                    Information = table.Column<string>(maxLength: 2000, nullable: true),
                    DailyOperationSizingDocumentId = table.Column<Guid>(nullable: false)
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
                name: "IX_Weaving_DailyOperationSizingDetails_DailyOperationSizingDocumentId",
                table: "Weaving_DailyOperationSizingDetails",
                column: "DailyOperationSizingDocumentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Weaving_DailyOperationSizingDetails");

            migrationBuilder.DropTable(
                name: "Weaving_DailyOperationSizingDocuments");
        }
    }
}
