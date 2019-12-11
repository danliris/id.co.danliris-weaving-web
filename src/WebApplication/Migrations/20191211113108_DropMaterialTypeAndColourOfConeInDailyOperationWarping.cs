using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class DropMaterialTypeAndColourOfConeInDailyOperationWarping : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ColourOfCone",
                table: "Weaving_DailyOperationWarpingDocuments");

            migrationBuilder.DropColumn(
                name: "MaterialTypeId",
                table: "Weaving_DailyOperationWarpingDocuments");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ColourOfCone",
                table: "Weaving_DailyOperationWarpingDocuments",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "MaterialTypeId",
                table: "Weaving_DailyOperationWarpingDocuments",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }
    }
}
