using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class weavingversionbeta1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Weaving_RingDocuments",
                table: "Weaving_RingDocuments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Weaving_MaterialTypes",
                table: "Weaving_MaterialTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Weaving_Constructions",
                table: "Weaving_Constructions");

            migrationBuilder.RenameTable(
                name: "Weaving_RingDocuments",
                newName: "Weaving_YarnNumberDocuments");

            migrationBuilder.RenameTable(
                name: "Weaving_MaterialTypes",
                newName: "Weaving_MaterialTypeDocument");

            migrationBuilder.RenameTable(
                name: "Weaving_Constructions",
                newName: "Weaving_ConstructionDocuments");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Weaving_YarnNumberDocuments",
                table: "Weaving_YarnNumberDocuments",
                column: "Identity");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Weaving_MaterialTypeDocument",
                table: "Weaving_MaterialTypeDocument",
                column: "Identity");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Weaving_ConstructionDocuments",
                table: "Weaving_ConstructionDocuments",
                column: "Identity");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Weaving_YarnNumberDocuments",
                table: "Weaving_YarnNumberDocuments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Weaving_MaterialTypeDocument",
                table: "Weaving_MaterialTypeDocument");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Weaving_ConstructionDocuments",
                table: "Weaving_ConstructionDocuments");

            migrationBuilder.RenameTable(
                name: "Weaving_YarnNumberDocuments",
                newName: "Weaving_RingDocuments");

            migrationBuilder.RenameTable(
                name: "Weaving_MaterialTypeDocument",
                newName: "Weaving_MaterialTypes");

            migrationBuilder.RenameTable(
                name: "Weaving_ConstructionDocuments",
                newName: "Weaving_Constructions");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Weaving_RingDocuments",
                table: "Weaving_RingDocuments",
                column: "Identity");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Weaving_MaterialTypes",
                table: "Weaving_MaterialTypes",
                column: "Identity");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Weaving_Constructions",
                table: "Weaving_Constructions",
                column: "Identity");
        }
    }
}
