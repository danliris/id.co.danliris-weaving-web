using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class RevampAndAddBeamProductResultsOnSizing : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Weaving_DailyOperationSizingBeamProducts_Weaving_DailyOperationSizingDocuments_DailyOperationSizingDocumentId",
                table: "Weaving_DailyOperationSizingBeamProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_Weaving_DailyOperationSizingHistories_Weaving_DailyOperationSizingDocuments_DailyOperationSizingDocumentId",
                table: "Weaving_DailyOperationSizingHistories");

            migrationBuilder.DropIndex(
                name: "IX_Weaving_DailyOperationSizingHistories_DailyOperationSizingDocumentId",
                table: "Weaving_DailyOperationSizingHistories");

            migrationBuilder.DropIndex(
                name: "IX_Weaving_DailyOperationSizingBeamProducts_DailyOperationSizingDocumentId",
                table: "Weaving_DailyOperationSizingBeamProducts");

            migrationBuilder.DropColumn(
                name: "BeamsWarping",
                table: "Weaving_DailyOperationSizingDocuments");

            migrationBuilder.AlterColumn<double>(
                name: "GradeD",
                table: "Weaving_EstimationProductDetails",
                nullable: true,
                oldClrType: typeof(double));

            migrationBuilder.AddColumn<int>(
                name: "WarpingBeamLengthPerOperatorUomId",
                table: "Weaving_DailyOperationWarpingHistories",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "Visco",
                table: "Weaving_DailyOperationSizingDocuments",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "TexSQ",
                table: "Weaving_DailyOperationSizingDocuments",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<Guid>(
                name: "OrderDocumentId",
                table: "Weaving_DailyOperationSizingDocuments",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "MachineDocumentId",
                table: "Weaving_DailyOperationSizingDocuments",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BeamProductResult",
                table: "Weaving_DailyOperationSizingDocuments",
                nullable: false,
                defaultValue: 0);

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

            migrationBuilder.CreateTable(
                name: "Weaving_DailyOperationSizingBeamsWarping",
                columns: table => new
                {
                    Identity = table.Column<Guid>(nullable: false),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 32, nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 32, nullable: true),
                    ModifiedDate = table.Column<DateTimeOffset>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    DeletedDate = table.Column<DateTimeOffset>(nullable: true),
                    DeletedBy = table.Column<string>(maxLength: 32, nullable: true),
                    BeamDocumentId = table.Column<Guid>(nullable: false),
                    YarnStrands = table.Column<double>(nullable: false),
                    EmptyWeight = table.Column<double>(nullable: false),
                    DailyOperationSizingDocumentId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Weaving_DailyOperationSizingBeamsWarping", x => x.Identity);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Weaving_DailyOperationSizingBeamsWarping");

            migrationBuilder.DropColumn(
                name: "WarpingBeamLengthPerOperatorUomId",
                table: "Weaving_DailyOperationWarpingHistories");

            migrationBuilder.DropColumn(
                name: "BeamProductResult",
                table: "Weaving_DailyOperationSizingDocuments");

            migrationBuilder.AlterColumn<double>(
                name: "GradeD",
                table: "Weaving_EstimationProductDetails",
                nullable: false,
                oldClrType: typeof(double),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Visco",
                table: "Weaving_DailyOperationSizingDocuments",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "TexSQ",
                table: "Weaving_DailyOperationSizingDocuments",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "OrderDocumentId",
                table: "Weaving_DailyOperationSizingDocuments",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AlterColumn<Guid>(
                name: "MachineDocumentId",
                table: "Weaving_DailyOperationSizingDocuments",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AddColumn<string>(
                name: "BeamsWarping",
                table: "Weaving_DailyOperationSizingDocuments",
                nullable: true);

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

            migrationBuilder.CreateIndex(
                name: "IX_Weaving_DailyOperationSizingHistories_DailyOperationSizingDocumentId",
                table: "Weaving_DailyOperationSizingHistories",
                column: "DailyOperationSizingDocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_Weaving_DailyOperationSizingBeamProducts_DailyOperationSizingDocumentId",
                table: "Weaving_DailyOperationSizingBeamProducts",
                column: "DailyOperationSizingDocumentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Weaving_DailyOperationSizingBeamProducts_Weaving_DailyOperationSizingDocuments_DailyOperationSizingDocumentId",
                table: "Weaving_DailyOperationSizingBeamProducts",
                column: "DailyOperationSizingDocumentId",
                principalTable: "Weaving_DailyOperationSizingDocuments",
                principalColumn: "Identity",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Weaving_DailyOperationSizingHistories_Weaving_DailyOperationSizingDocuments_DailyOperationSizingDocumentId",
                table: "Weaving_DailyOperationSizingHistories",
                column: "DailyOperationSizingDocumentId",
                principalTable: "Weaving_DailyOperationSizingDocuments",
                principalColumn: "Identity",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
