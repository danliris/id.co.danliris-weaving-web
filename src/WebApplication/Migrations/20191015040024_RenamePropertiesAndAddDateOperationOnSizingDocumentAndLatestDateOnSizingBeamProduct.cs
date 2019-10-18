using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class RenamePropertiesAndAddDateOperationOnSizingDocumentAndLatestDateOnSizingBeamProduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SizingBeamStatus",
                table: "Weaving_DailyOperationSizingBeamProducts",
                newName: "BeamStatus");

            migrationBuilder.RenameColumn(
                name: "DateTimeBeamDocument",
                table: "Weaving_DailyOperationSizingBeamProducts",
                newName: "LatestDateTimeBeamProduct");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DateTimeOperation",
                table: "Weaving_DailyOperationSizingDocuments",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AlterColumn<double>(
                name: "WeightTheoritical",
                table: "Weaving_DailyOperationSizingBeamProducts",
                nullable: true,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<double>(
                name: "WeightNetto",
                table: "Weaving_DailyOperationSizingBeamProducts",
                nullable: true,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<double>(
                name: "WeightBruto",
                table: "Weaving_DailyOperationSizingBeamProducts",
                nullable: true,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<double>(
                name: "SPU",
                table: "Weaving_DailyOperationSizingBeamProducts",
                nullable: true,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<double>(
                name: "PISMeter",
                table: "Weaving_DailyOperationSizingBeamProducts",
                nullable: true,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<double>(
                name: "CounterStart",
                table: "Weaving_DailyOperationSizingBeamProducts",
                nullable: true,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<double>(
                name: "CounterFinish",
                table: "Weaving_DailyOperationSizingBeamProducts",
                nullable: true,
                oldClrType: typeof(double));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateTimeOperation",
                table: "Weaving_DailyOperationSizingDocuments");

            migrationBuilder.RenameColumn(
                name: "LatestDateTimeBeamProduct",
                table: "Weaving_DailyOperationSizingBeamProducts",
                newName: "DateTimeBeamDocument");

            migrationBuilder.RenameColumn(
                name: "BeamStatus",
                table: "Weaving_DailyOperationSizingBeamProducts",
                newName: "SizingBeamStatus");

            migrationBuilder.AlterColumn<double>(
                name: "WeightTheoritical",
                table: "Weaving_DailyOperationSizingBeamProducts",
                nullable: false,
                oldClrType: typeof(double),
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "WeightNetto",
                table: "Weaving_DailyOperationSizingBeamProducts",
                nullable: false,
                oldClrType: typeof(double),
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "WeightBruto",
                table: "Weaving_DailyOperationSizingBeamProducts",
                nullable: false,
                oldClrType: typeof(double),
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "SPU",
                table: "Weaving_DailyOperationSizingBeamProducts",
                nullable: false,
                oldClrType: typeof(double),
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "PISMeter",
                table: "Weaving_DailyOperationSizingBeamProducts",
                nullable: false,
                oldClrType: typeof(double),
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "CounterStart",
                table: "Weaving_DailyOperationSizingBeamProducts",
                nullable: false,
                oldClrType: typeof(double),
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "CounterFinish",
                table: "Weaving_DailyOperationSizingBeamProducts",
                nullable: false,
                oldClrType: typeof(double),
                oldNullable: true);
        }
    }
}
