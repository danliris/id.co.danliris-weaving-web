using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class ChangeDataTypeOnMasterYarnNumber : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Number",
                table: "Weaving_YarnNumberDocuments",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(int),
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "AdditionalNumber",
                table: "Weaving_YarnNumberDocuments",
                nullable: true,
                oldClrType: typeof(int),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Number",
                table: "Weaving_YarnNumberDocuments",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AdditionalNumber",
                table: "Weaving_YarnNumberDocuments",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
