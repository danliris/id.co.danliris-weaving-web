using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class weavingversion1145 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaterialTypeDocument",
                table: "Weaving_YarnDocuments");

            migrationBuilder.DropColumn(
                name: "RingDocument",
                table: "Weaving_YarnDocuments");

            migrationBuilder.AddColumn<Guid>(
                name: "MaterialTypeId",
                table: "Weaving_YarnDocuments",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "YarnNumberId",
                table: "Weaving_YarnDocuments",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaterialTypeId",
                table: "Weaving_YarnDocuments");

            migrationBuilder.DropColumn(
                name: "YarnNumberId",
                table: "Weaving_YarnDocuments");

            migrationBuilder.AddColumn<string>(
                name: "MaterialTypeDocument",
                table: "Weaving_YarnDocuments",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RingDocument",
                table: "Weaving_YarnDocuments",
                maxLength: 255,
                nullable: true);
        }
    }
}
