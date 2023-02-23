using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class AddBeamStockMonitoring : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Weaving_BeamStockMonitoringDocuments",
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
                    SizingEntryDate = table.Column<DateTimeOffset>(nullable: false),
                    ReachingEntryDate = table.Column<DateTimeOffset>(nullable: false),
                    LoomEntryDate = table.Column<DateTimeOffset>(nullable: false),
                    EmptyEntryDate = table.Column<DateTimeOffset>(nullable: false),
                    OrderDocumentId = table.Column<Guid>(nullable: false),
                    SizingLengthStock = table.Column<double>(nullable: false),
                    ReachingLengthStock = table.Column<double>(nullable: false),
                    LoomLengthStock = table.Column<double>(nullable: false),
                    LengthUOMId = table.Column<int>(nullable: false),
                    Position = table.Column<int>(nullable: false),
                    LoomFinish = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Weaving_BeamStockMonitoringDocuments", x => x.Identity);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Weaving_BeamStockMonitoringDocuments");
        }
    }
}
