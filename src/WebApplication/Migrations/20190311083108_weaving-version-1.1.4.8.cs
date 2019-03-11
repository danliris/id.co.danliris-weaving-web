using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class weavingversion1148 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FabricConstructionDocument",
                table: "Weaving_OrderDocuments");

            migrationBuilder.DropColumn(
                name: "WeavingUnit",
                table: "Weaving_OrderDocuments");

            migrationBuilder.AddColumn<Guid>(
                name: "ConstructionId",
                table: "Weaving_OrderDocuments",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UnitId",
                table: "Weaving_OrderDocuments",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConstructionId",
                table: "Weaving_OrderDocuments");

            migrationBuilder.DropColumn(
                name: "UnitId",
                table: "Weaving_OrderDocuments");

            migrationBuilder.AddColumn<string>(
                name: "FabricConstructionDocument",
                table: "Weaving_OrderDocuments",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WeavingUnit",
                table: "Weaving_OrderDocuments",
                maxLength: 255,
                nullable: true);
        }
    }
}
