using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class AddWeftOriginTwoOnOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.RenameColumn(
                name: "WeftOriginId",
                table: "Weaving_OrderDocuments",
                newName: "WeftOriginIdOne");

            migrationBuilder.RenameColumn(
                name: "WarpOriginId",
                table: "Weaving_OrderDocuments",
                newName: "WarpOriginIdOne");

            migrationBuilder.AddColumn<Guid>(
                name: "WeftOriginIdTwo",
                table: "Weaving_OrderDocuments",
                nullable: true);

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Weaving_DailyOperationSizingBeamsWarping");

            migrationBuilder.DropColumn(
                name: "WeftOriginIdTwo",
                table: "Weaving_OrderDocuments");

            migrationBuilder.DropColumn(
                name: "Area",
                table: "Weaving_MachineDocuments");

            migrationBuilder.DropColumn(
                name: "Block",
                table: "Weaving_MachineDocuments");

            migrationBuilder.DropColumn(
                name: "CutmarkUom",
                table: "Weaving_MachineDocuments");

            migrationBuilder.DropColumn(
                name: "Process",
                table: "Weaving_MachineDocuments");

            migrationBuilder.DropColumn(
                name: "WarpingBeamLengthPerOperatorUomId",
                table: "Weaving_DailyOperationWarpingHistories");

            migrationBuilder.DropColumn(
                name: "BeamProductResult",
                table: "Weaving_DailyOperationSizingDocuments");

            migrationBuilder.DropColumn(
                name: "TotalBroken",
                table: "Weaving_DailyOperationSizingBeamProducts");

            migrationBuilder.RenameColumn(
                name: "WeftOriginIdOne",
                table: "Weaving_OrderDocuments",
                newName: "WeftOriginId");

            migrationBuilder.RenameColumn(
                name: "WarpOriginIdOne",
                table: "Weaving_OrderDocuments",
                newName: "WarpOriginId");

            migrationBuilder.RenameColumn(
                name: "BrokenPerShift",
                table: "Weaving_DailyOperationSizingHistories",
                newName: "MachineTroubled");
           
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
    }
}
