using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class update_weavingorderdocument_fabricspecificationdetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FabricSpecificationId",
                table: "WeavingOrderDocuments");

            migrationBuilder.AddColumn<string>(
                name: "FabricSpecification",
                table: "WeavingOrderDocuments",
                maxLength: 255,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FabricSpecification",
                table: "WeavingOrderDocuments");

            migrationBuilder.AddColumn<Guid>(
                name: "FabricSpecificationId",
                table: "WeavingOrderDocuments",
                nullable: true);
        }
    }
}
