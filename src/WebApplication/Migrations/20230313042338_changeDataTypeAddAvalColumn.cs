using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class changeDataTypeAddAvalColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "Width",
                table: "WeavingEstimatedProductions",
                nullable: false,
                oldClrType: typeof(float));

            migrationBuilder.AlterColumn<double>(
                name: "WeftBale",
                table: "WeavingEstimatedProductions",
                nullable: false,
                oldClrType: typeof(float));

            migrationBuilder.AlterColumn<double>(
                name: "Weft",
                table: "WeavingEstimatedProductions",
                nullable: false,
                oldClrType: typeof(float));

            migrationBuilder.AlterColumn<double>(
                name: "WarpLength",
                table: "WeavingEstimatedProductions",
                nullable: false,
                oldClrType: typeof(float));

            migrationBuilder.AlterColumn<double>(
                name: "WarpBale",
                table: "WeavingEstimatedProductions",
                nullable: false,
                oldClrType: typeof(float));

            migrationBuilder.AlterColumn<double>(
                name: "TotalBale",
                table: "WeavingEstimatedProductions",
                nullable: false,
                oldClrType: typeof(float));

            migrationBuilder.AlterColumn<double>(
                name: "Total",
                table: "WeavingEstimatedProductions",
                nullable: false,
                oldClrType: typeof(float));

            migrationBuilder.AlterColumn<double>(
                name: "NumberOrder",
                table: "WeavingEstimatedProductions",
                nullable: false,
                oldClrType: typeof(float));

            migrationBuilder.AlterColumn<double>(
                name: "GradeC",
                table: "WeavingEstimatedProductions",
                nullable: false,
                oldClrType: typeof(float));

            migrationBuilder.AlterColumn<double>(
                name: "GradeB",
                table: "WeavingEstimatedProductions",
                nullable: false,
                oldClrType: typeof(float));

            migrationBuilder.AlterColumn<double>(
                name: "GradeA",
                table: "WeavingEstimatedProductions",
                nullable: false,
                oldClrType: typeof(float));

            migrationBuilder.AddColumn<double>(
                name: "Aval",
                table: "WeavingEstimatedProductions",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Aval",
                table: "WeavingEstimatedProductions");

            migrationBuilder.AlterColumn<float>(
                name: "Width",
                table: "WeavingEstimatedProductions",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<float>(
                name: "WeftBale",
                table: "WeavingEstimatedProductions",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<float>(
                name: "Weft",
                table: "WeavingEstimatedProductions",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<float>(
                name: "WarpLength",
                table: "WeavingEstimatedProductions",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<float>(
                name: "WarpBale",
                table: "WeavingEstimatedProductions",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<float>(
                name: "TotalBale",
                table: "WeavingEstimatedProductions",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<float>(
                name: "Total",
                table: "WeavingEstimatedProductions",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<float>(
                name: "NumberOrder",
                table: "WeavingEstimatedProductions",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<float>(
                name: "GradeC",
                table: "WeavingEstimatedProductions",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<float>(
                name: "GradeB",
                table: "WeavingEstimatedProductions",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<float>(
                name: "GradeA",
                table: "WeavingEstimatedProductions",
                nullable: false,
                oldClrType: typeof(double));
        }
    }
}
