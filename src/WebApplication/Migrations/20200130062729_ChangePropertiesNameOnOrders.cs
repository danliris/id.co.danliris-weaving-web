using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class ChangePropertiesNameOnOrders : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "WeftOrigin",
                table: "Weaving_OrderDocuments",
                newName: "WeftOriginId");

            migrationBuilder.RenameColumn(
                name: "WarpOrigin",
                table: "Weaving_OrderDocuments",
                newName: "WarpOriginId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "WeftOriginId",
                table: "Weaving_OrderDocuments",
                newName: "WeftOrigin");

            migrationBuilder.RenameColumn(
                name: "WarpOriginId",
                table: "Weaving_OrderDocuments",
                newName: "WarpOrigin");
        }
    }
}
