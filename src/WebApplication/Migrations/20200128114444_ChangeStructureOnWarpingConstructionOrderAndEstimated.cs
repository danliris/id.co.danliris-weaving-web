using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class ChangeStructureOnWarpingConstructionOrderAndEstimated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Weaving_DailyOperationWarpingBeamProducts_Weaving_DailyOperationWarpingDocuments_DailyOperationWarpingDocumentId",
                table: "Weaving_DailyOperationWarpingBeamProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_Weaving_DailyOperationWarpingBrokenCauses_Weaving_DailyOperationWarpingBeamProducts_DailyOperationWarpingBeamProductId",
                table: "Weaving_DailyOperationWarpingBrokenCauses");

            migrationBuilder.DropForeignKey(
                name: "FK_Weaving_DailyOperationWarpingHistories_Weaving_DailyOperationWarpingDocuments_DailyOperationWarpingDocumentId",
                table: "Weaving_DailyOperationWarpingHistories");

            migrationBuilder.DropTable(
                name: "Weaving_EstimationDetails");

            migrationBuilder.DropIndex(
                name: "IX_Weaving_DailyOperationWarpingHistories_DailyOperationWarpingDocumentId",
                table: "Weaving_DailyOperationWarpingHistories");

            migrationBuilder.DropIndex(
                name: "IX_Weaving_DailyOperationWarpingBrokenCauses_DailyOperationWarpingBeamProductId",
                table: "Weaving_DailyOperationWarpingBrokenCauses");

            migrationBuilder.DropIndex(
                name: "IX_Weaving_DailyOperationWarpingBeamProducts_DailyOperationWarpingDocumentId",
                table: "Weaving_DailyOperationWarpingBeamProducts");

            migrationBuilder.DropColumn(
                name: "ConstructionId",
                table: "Weaving_OrderDocuments");

            migrationBuilder.DropColumn(
                name: "DateOrdered",
                table: "Weaving_OrderDocuments");

            migrationBuilder.DropColumn(
                name: "WarpComposition",
                table: "Weaving_OrderDocuments");

            migrationBuilder.DropColumn(
                name: "WeftComposition",
                table: "Weaving_OrderDocuments");

            migrationBuilder.DropColumn(
                name: "WholeGrade",
                table: "Weaving_OrderDocuments");

            migrationBuilder.DropColumn(
                name: "ListOfWarp",
                table: "Weaving_ConstructionDocuments");

            migrationBuilder.DropColumn(
                name: "ListOfWeft",
                table: "Weaving_ConstructionDocuments");

            migrationBuilder.DropColumn(
                name: "TotalEnds",
                table: "Weaving_ConstructionDocuments");

            migrationBuilder.RenameColumn(
                name: "MaterialTypeName",
                table: "Weaving_ConstructionDocuments",
                newName: "MaterialType");

            migrationBuilder.AlterColumn<Guid>(
                name: "WeftOrigin",
                table: "Weaving_OrderDocuments",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "WarpOrigin",
                table: "Weaving_OrderDocuments",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "UnitId",
                table: "Weaving_OrderDocuments",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "Period",
                table: "Weaving_OrderDocuments",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AddColumn<double>(
                name: "AllGrade",
                table: "Weaving_OrderDocuments",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<Guid>(
                name: "ConstructionDocumentId",
                table: "Weaving_OrderDocuments",
                maxLength: 255,
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<double>(
                name: "WarpCompositionCotton",
                table: "Weaving_OrderDocuments",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "WarpCompositionOthers",
                table: "Weaving_OrderDocuments",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "WarpCompositionPoly",
                table: "Weaving_OrderDocuments",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "WeftCompositionCotton",
                table: "Weaving_OrderDocuments",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "WeftCompositionOthers",
                table: "Weaving_OrderDocuments",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "WeftCompositionPoly",
                table: "Weaving_OrderDocuments",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AlterColumn<int>(
                name: "UnitId",
                table: "Weaving_EstimationProductDocuments",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "Period",
                table: "Weaving_EstimationProductDocuments",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "WarpingTotalBeamLengthUomId",
                table: "Weaving_DailyOperationWarpingBeamProducts",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "WarpingTotalBeamLength",
                table: "Weaving_DailyOperationWarpingBeamProducts",
                nullable: false,
                oldClrType: typeof(double),
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "Tention",
                table: "Weaving_DailyOperationWarpingBeamProducts",
                nullable: false,
                oldClrType: typeof(double),
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "PressRoll",
                table: "Weaving_DailyOperationWarpingBeamProducts",
                nullable: false,
                oldClrType: typeof(double),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "MachineSpeed",
                table: "Weaving_DailyOperationWarpingBeamProducts",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "ReedSpace",
                table: "Weaving_ConstructionDocuments",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddColumn<double>(
                name: "YarnStrandsAmount",
                table: "Weaving_ConstructionDocuments",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.CreateTable(
                name: "Weaving_ConstructionYarnDetails",
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
                    YarnId = table.Column<Guid>(nullable: false),
                    Quantity = table.Column<double>(nullable: false),
                    Information = table.Column<string>(maxLength: 255, nullable: true),
                    Type = table.Column<string>(nullable: true),
                    FabricConstructionDocumentId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Weaving_ConstructionYarnDetails", x => x.Identity);
                });

            migrationBuilder.CreateTable(
                name: "Weaving_EstimationProductDetails",
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
                    OrderId = table.Column<Guid>(nullable: false),
                    ConstructionId = table.Column<Guid>(nullable: false),
                    GradeA = table.Column<double>(nullable: false),
                    GradeB = table.Column<double>(nullable: false),
                    GradeC = table.Column<double>(nullable: false),
                    GradeD = table.Column<double>(nullable: false),
                    EstimatedProductionDocumentId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Weaving_EstimationProductDetails", x => x.Identity);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Weaving_ConstructionYarnDetails");

            migrationBuilder.DropTable(
                name: "Weaving_EstimationProductDetails");

            migrationBuilder.DropColumn(
                name: "AllGrade",
                table: "Weaving_OrderDocuments");

            migrationBuilder.DropColumn(
                name: "ConstructionDocumentId",
                table: "Weaving_OrderDocuments");

            migrationBuilder.DropColumn(
                name: "WarpCompositionCotton",
                table: "Weaving_OrderDocuments");

            migrationBuilder.DropColumn(
                name: "WarpCompositionOthers",
                table: "Weaving_OrderDocuments");

            migrationBuilder.DropColumn(
                name: "WarpCompositionPoly",
                table: "Weaving_OrderDocuments");

            migrationBuilder.DropColumn(
                name: "WeftCompositionCotton",
                table: "Weaving_OrderDocuments");

            migrationBuilder.DropColumn(
                name: "WeftCompositionOthers",
                table: "Weaving_OrderDocuments");

            migrationBuilder.DropColumn(
                name: "WeftCompositionPoly",
                table: "Weaving_OrderDocuments");

            migrationBuilder.DropColumn(
                name: "YarnStrandsAmount",
                table: "Weaving_ConstructionDocuments");

            migrationBuilder.RenameColumn(
                name: "MaterialType",
                table: "Weaving_ConstructionDocuments",
                newName: "MaterialTypeName");

            migrationBuilder.AlterColumn<string>(
                name: "WeftOrigin",
                table: "Weaving_OrderDocuments",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AlterColumn<string>(
                name: "WarpOrigin",
                table: "Weaving_OrderDocuments",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AlterColumn<int>(
                name: "UnitId",
                table: "Weaving_OrderDocuments",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<string>(
                name: "Period",
                table: "Weaving_OrderDocuments",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(DateTime),
                oldMaxLength: 255);

            migrationBuilder.AddColumn<Guid>(
                name: "ConstructionId",
                table: "Weaving_OrderDocuments",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DateOrdered",
                table: "Weaving_OrderDocuments",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<string>(
                name: "WarpComposition",
                table: "Weaving_OrderDocuments",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WeftComposition",
                table: "Weaving_OrderDocuments",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WholeGrade",
                table: "Weaving_OrderDocuments",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "UnitId",
                table: "Weaving_EstimationProductDocuments",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<string>(
                name: "Period",
                table: "Weaving_EstimationProductDocuments",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<int>(
                name: "WarpingTotalBeamLengthUomId",
                table: "Weaving_DailyOperationWarpingBeamProducts",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<double>(
                name: "WarpingTotalBeamLength",
                table: "Weaving_DailyOperationWarpingBeamProducts",
                nullable: true,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<double>(
                name: "Tention",
                table: "Weaving_DailyOperationWarpingBeamProducts",
                nullable: true,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<double>(
                name: "PressRoll",
                table: "Weaving_DailyOperationWarpingBeamProducts",
                nullable: true,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<int>(
                name: "MachineSpeed",
                table: "Weaving_DailyOperationWarpingBeamProducts",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "ReedSpace",
                table: "Weaving_ConstructionDocuments",
                nullable: true,
                oldClrType: typeof(double));

            migrationBuilder.AddColumn<string>(
                name: "ListOfWarp",
                table: "Weaving_ConstructionDocuments",
                maxLength: 20000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ListOfWeft",
                table: "Weaving_ConstructionDocuments",
                maxLength: 20000,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TotalEnds",
                table: "Weaving_ConstructionDocuments",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Weaving_EstimationDetails",
                columns: table => new
                {
                    Identity = table.Column<Guid>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 32, nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(nullable: false),
                    Deleted = table.Column<bool>(nullable: true),
                    DeletedBy = table.Column<string>(maxLength: 32, nullable: true),
                    DeletedDate = table.Column<DateTimeOffset>(nullable: true),
                    EstimatedProductionDocumentId = table.Column<Guid>(nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 32, nullable: true),
                    ModifiedDate = table.Column<DateTimeOffset>(nullable: true),
                    OrderDocument = table.Column<string>(maxLength: 2000, nullable: true),
                    ProductGrade = table.Column<string>(maxLength: 2000, nullable: true),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    TotalGramEstimation = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Weaving_EstimationDetails", x => x.Identity);
                    table.ForeignKey(
                        name: "FK_Weaving_EstimationDetails_Weaving_EstimationProductDocuments_EstimatedProductionDocumentId",
                        column: x => x.EstimatedProductionDocumentId,
                        principalTable: "Weaving_EstimationProductDocuments",
                        principalColumn: "Identity",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Weaving_DailyOperationWarpingHistories_DailyOperationWarpingDocumentId",
                table: "Weaving_DailyOperationWarpingHistories",
                column: "DailyOperationWarpingDocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_Weaving_DailyOperationWarpingBrokenCauses_DailyOperationWarpingBeamProductId",
                table: "Weaving_DailyOperationWarpingBrokenCauses",
                column: "DailyOperationWarpingBeamProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Weaving_DailyOperationWarpingBeamProducts_DailyOperationWarpingDocumentId",
                table: "Weaving_DailyOperationWarpingBeamProducts",
                column: "DailyOperationWarpingDocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_Weaving_EstimationDetails_EstimatedProductionDocumentId",
                table: "Weaving_EstimationDetails",
                column: "EstimatedProductionDocumentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Weaving_DailyOperationWarpingBeamProducts_Weaving_DailyOperationWarpingDocuments_DailyOperationWarpingDocumentId",
                table: "Weaving_DailyOperationWarpingBeamProducts",
                column: "DailyOperationWarpingDocumentId",
                principalTable: "Weaving_DailyOperationWarpingDocuments",
                principalColumn: "Identity",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Weaving_DailyOperationWarpingBrokenCauses_Weaving_DailyOperationWarpingBeamProducts_DailyOperationWarpingBeamProductId",
                table: "Weaving_DailyOperationWarpingBrokenCauses",
                column: "DailyOperationWarpingBeamProductId",
                principalTable: "Weaving_DailyOperationWarpingBeamProducts",
                principalColumn: "Identity",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Weaving_DailyOperationWarpingHistories_Weaving_DailyOperationWarpingDocuments_DailyOperationWarpingDocumentId",
                table: "Weaving_DailyOperationWarpingHistories",
                column: "DailyOperationWarpingDocumentId",
                principalTable: "Weaving_DailyOperationWarpingDocuments",
                principalColumn: "Identity",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
