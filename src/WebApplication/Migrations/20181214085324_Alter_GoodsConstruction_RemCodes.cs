using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class Alter_GoodsConstruction_RemCodes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CodesJson",
                table: "GoodsConstruction");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CodesJson",
                table: "GoodsConstruction",
                maxLength: 500,
                nullable: true);
        }
    }
}