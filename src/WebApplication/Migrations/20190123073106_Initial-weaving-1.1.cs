using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class Initialweaving11 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Weaving_Constructions",
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
                    ConstructionNumber = table.Column<string>(maxLength: 255, nullable: true),
                    AmountOfWarp = table.Column<int>(nullable: false),
                    AmountOfWeft = table.Column<int>(nullable: false),
                    Width = table.Column<int>(nullable: false),
                    WovenType = table.Column<string>(maxLength: 255, nullable: true),
                    WarpType = table.Column<string>(maxLength: 255, nullable: true),
                    WeftType = table.Column<string>(maxLength: 255, nullable: true),
                    TotalYarn = table.Column<double>(nullable: false),
                    MaterialType = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Weaving_Constructions", x => x.Identity);
                });

            migrationBuilder.CreateTable(
                name: "Weaving_MaterialTypes",
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
                    Code = table.Column<string>(maxLength: 255, nullable: true),
                    Name = table.Column<string>(maxLength: 255, nullable: true),
                    Description = table.Column<string>(maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Weaving_MaterialTypes", x => x.Identity);
                });

            migrationBuilder.CreateTable(
                name: "Weaving_OrderDocuments",
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
                    OrderNumber = table.Column<string>(nullable: true),
                    DateOrdered = table.Column<DateTimeOffset>(nullable: false),
                    WarpOrigin = table.Column<string>(nullable: true),
                    WeftOrigin = table.Column<string>(nullable: true),
                    WholeGrade = table.Column<int>(nullable: false),
                    YarnType = table.Column<string>(nullable: true),
                    Period = table.Column<string>(maxLength: 255, nullable: true),
                    Composition = table.Column<string>(maxLength: 255, nullable: true),
                    FabricConstructionDocument = table.Column<string>(maxLength: 255, nullable: true),
                    WeavingUnit = table.Column<string>(maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Weaving_OrderDocuments", x => x.Identity);
                });

            migrationBuilder.CreateTable(
                name: "Weaving_ConstructionDetails",
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
                    Quantity = table.Column<double>(nullable: false),
                    Information = table.Column<string>(nullable: true),
                    Yarn = table.Column<string>(maxLength: 255, nullable: true),
                    Detail = table.Column<string>(nullable: true),
                    ConstructionDocumentId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Weaving_ConstructionDetails", x => x.Identity);
                    table.ForeignKey(
                        name: "FK_Weaving_ConstructionDetails_Weaving_Constructions_ConstructionDocumentId",
                        column: x => x.ConstructionDocumentId,
                        principalTable: "Weaving_Constructions",
                        principalColumn: "Identity",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Weaving_ConstructionDetails_ConstructionDocumentId",
                table: "Weaving_ConstructionDetails",
                column: "ConstructionDocumentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Weaving_ConstructionDetails");

            migrationBuilder.DropTable(
                name: "Weaving_MaterialTypes");

            migrationBuilder.DropTable(
                name: "Weaving_OrderDocuments");

            migrationBuilder.DropTable(
                name: "Weaving_Constructions");
        }
    }
}
