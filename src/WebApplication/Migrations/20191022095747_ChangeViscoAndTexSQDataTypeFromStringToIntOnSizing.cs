using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class ChangeViscoAndTexSQDataTypeFromStringToIntOnSizing : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Visco",
                table: "Weaving_DailyOperationSizingDocuments",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "TexSQ",
                table: "Weaving_DailyOperationSizingDocuments",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Visco",
                table: "Weaving_DailyOperationSizingDocuments",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<string>(
                name: "TexSQ",
                table: "Weaving_DailyOperationSizingDocuments",
                nullable: true,
                oldClrType: typeof(int));
        }
    }
}
