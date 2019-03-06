using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class weavingversion1147 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Weaving_ConstructionDetails");

            migrationBuilder.DropColumn(
                name: "MaterialType",
                table: "Weaving_Constructions");

            migrationBuilder.AddColumn<string>(
                name: "ListOfWarp",
                table: "Weaving_Constructions",
                maxLength: 20000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ListOfWeft",
                table: "Weaving_Constructions",
                maxLength: 20000,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "MaterialTypeId",
                table: "Weaving_Constructions",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ListOfWarp",
                table: "Weaving_Constructions");

            migrationBuilder.DropColumn(
                name: "ListOfWeft",
                table: "Weaving_Constructions");

            migrationBuilder.DropColumn(
                name: "MaterialTypeId",
                table: "Weaving_Constructions");

            migrationBuilder.AddColumn<string>(
                name: "MaterialType",
                table: "Weaving_Constructions",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Weaving_ConstructionDetails",
                columns: table => new
                {
                    Identity = table.Column<Guid>(nullable: false),
                    ConstructionDocumentId = table.Column<Guid>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 32, nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(nullable: false),
                    Deleted = table.Column<bool>(nullable: true),
                    DeletedBy = table.Column<string>(maxLength: 32, nullable: true),
                    DeletedDate = table.Column<DateTimeOffset>(nullable: true),
                    Detail = table.Column<string>(nullable: true),
                    Information = table.Column<string>(nullable: true),
                    ModifiedBy = table.Column<string>(maxLength: 32, nullable: true),
                    ModifiedDate = table.Column<DateTimeOffset>(nullable: true),
                    Quantity = table.Column<double>(nullable: false),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    Yarn = table.Column<string>(nullable: true)
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
    }
}
