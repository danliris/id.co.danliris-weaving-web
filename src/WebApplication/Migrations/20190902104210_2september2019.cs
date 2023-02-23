using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class _2september2019 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateTimeOperation",
                table: "Weaving_StockCardDocuments");

            migrationBuilder.DropColumn(
                name: "IsAvailable",
                table: "Weaving_StockCardDocuments");

            migrationBuilder.DropColumn(
                name: "IsReaching",
                table: "Weaving_StockCardDocuments");

            migrationBuilder.DropColumn(
                name: "IsTying",
                table: "Weaving_StockCardDocuments");

            migrationBuilder.DropColumn(
                name: "StockType",
                table: "Weaving_StockCardDocuments");

            migrationBuilder.DropColumn(
                name: "WeavingUnitId",
                table: "Weaving_DailyOperationSizingDocuments");

            migrationBuilder.DropColumn(
                name: "PISPieces",
                table: "Weaving_DailyOperationReachingTyingDocuments");

            migrationBuilder.AddColumn<Guid>(
                name: "BeamId",
                table: "Weaving_StockCardDocuments",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<double>(
                name: "EmptyWeight",
                table: "Weaving_DailyOperationSizingDocuments",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BeamId",
                table: "Weaving_StockCardDocuments");

            migrationBuilder.DropColumn(
                name: "EmptyWeight",
                table: "Weaving_DailyOperationSizingDocuments");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DateTimeOperation",
                table: "Weaving_StockCardDocuments",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<bool>(
                name: "IsAvailable",
                table: "Weaving_StockCardDocuments",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsReaching",
                table: "Weaving_StockCardDocuments",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsTying",
                table: "Weaving_StockCardDocuments",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "StockType",
                table: "Weaving_StockCardDocuments",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WeavingUnitId",
                table: "Weaving_DailyOperationSizingDocuments",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "PISPieces",
                table: "Weaving_DailyOperationReachingTyingDocuments",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
