using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class weavingversion1144 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Composition",
                table: "Weaving_OrderDocuments",
                newName: "WeftComposition");

            migrationBuilder.AddColumn<string>(
                name: "WarpComposition",
                table: "Weaving_OrderDocuments",
                maxLength: 255,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WarpComposition",
                table: "Weaving_OrderDocuments");

            migrationBuilder.RenameColumn(
                name: "WeftComposition",
                table: "Weaving_OrderDocuments",
                newName: "Composition");
        }
    }
}
