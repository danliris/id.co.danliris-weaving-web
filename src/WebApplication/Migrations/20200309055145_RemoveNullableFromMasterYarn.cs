using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class RemoveNullableFromMasterYarn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "YarnNumberId",
                table: "Weaving_YarnDocuments",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "MaterialTypeId",
                table: "Weaving_YarnDocuments",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "YarnNumberId",
                table: "Weaving_YarnDocuments",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AlterColumn<Guid>(
                name: "MaterialTypeId",
                table: "Weaving_YarnDocuments",
                nullable: true,
                oldClrType: typeof(Guid));
        }
    }
}
