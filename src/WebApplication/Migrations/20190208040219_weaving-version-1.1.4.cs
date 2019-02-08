using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class weavingversion114 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Weaving_EstimationProductDocuments",
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
                    EstimatedNumber = table.Column<string>(nullable: true),
                    Period = table.Column<string>(maxLength: 255, nullable: true),
                    Unit = table.Column<string>(maxLength: 255, nullable: true),
                    TotalEstimationOrder = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Weaving_EstimationProductDocuments", x => x.Identity);
                });

            migrationBuilder.CreateTable(
                name: "Weaving_EstimationDetails",
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
                    OrderDocument = table.Column<string>(maxLength: 2000, nullable: true),
                    ProductGrade = table.Column<string>(maxLength: 2000, nullable: true),
                    EstimatedProductionDocumentId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Weaving_EstimationDetails", x => x.Identity);
                    table.ForeignKey(
                        name: "FK_Weaving_EstimationDetails_Weaving_EstimationProductDocuments_EstimatedProductionDocumentId",
                        column: x => x.EstimatedProductionDocumentId,
                        principalTable: "Weaving_EstimationProductDocuments",
                        principalColumn: "Identity",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Weaving_EstimationDetails_EstimatedProductionDocumentId",
                table: "Weaving_EstimationDetails",
                column: "EstimatedProductionDocumentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Weaving_EstimationDetails");

            migrationBuilder.DropTable(
                name: "Weaving_EstimationProductDocuments");
        }
    }
}
