using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class AddWarpingBrokenCausesOnBeamProductWarping : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BrokenThreadsCause",
                table: "Weaving_DailyOperationWarpingBeamProducts");

            migrationBuilder.DropColumn(
                name: "ConeDeficient",
                table: "Weaving_DailyOperationWarpingBeamProducts");

            migrationBuilder.DropColumn(
                name: "LeftLooseCreel",
                table: "Weaving_DailyOperationWarpingBeamProducts");

            migrationBuilder.DropColumn(
                name: "LooseThreadsAmount",
                table: "Weaving_DailyOperationWarpingBeamProducts");

            migrationBuilder.RenameColumn(
                name: "WarpingBeamLengthUOMId",
                table: "Weaving_DailyOperationWarpingBeamProducts",
                newName: "WarpingTotalBeamLengthUomId");

            migrationBuilder.RenameColumn(
                name: "WarpingBeamLength",
                table: "Weaving_DailyOperationWarpingBeamProducts",
                newName: "WarpingTotalBeamLength");

            migrationBuilder.RenameColumn(
                name: "RightLooseCreel",
                table: "Weaving_DailyOperationWarpingBeamProducts",
                newName: "PressRollUomId");

            migrationBuilder.AddColumn<double>(
                name: "WarpingBeamLengthPerOperator",
                table: "Weaving_DailyOperationWarpingHistories",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AlterColumn<double>(
                name: "Tention",
                table: "Weaving_DailyOperationWarpingBeamProducts",
                nullable: true,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Weaving_DailyOperationWarpingBrokenCauses",
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
                    BrokenCauseId = table.Column<Guid>(nullable: false),
                    TotalBroken = table.Column<int>(nullable: false),
                    DailyOperationWarpingBeamProductId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Weaving_DailyOperationWarpingBrokenCauses", x => x.Identity);
                    table.ForeignKey(
                        name: "FK_Weaving_DailyOperationWarpingBrokenCauses_Weaving_DailyOperationWarpingBeamProducts_DailyOperationWarpingBeamProductId",
                        column: x => x.DailyOperationWarpingBeamProductId,
                        principalTable: "Weaving_DailyOperationWarpingBeamProducts",
                        principalColumn: "Identity",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Weaving_DailyOperationWarpingBrokenCauses_DailyOperationWarpingBeamProductId",
                table: "Weaving_DailyOperationWarpingBrokenCauses",
                column: "DailyOperationWarpingBeamProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Weaving_DailyOperationWarpingBrokenCauses");

            migrationBuilder.DropColumn(
                name: "WarpingBeamLengthPerOperator",
                table: "Weaving_DailyOperationWarpingHistories");

            migrationBuilder.RenameColumn(
                name: "WarpingTotalBeamLengthUomId",
                table: "Weaving_DailyOperationWarpingBeamProducts",
                newName: "WarpingBeamLengthUOMId");

            migrationBuilder.RenameColumn(
                name: "WarpingTotalBeamLength",
                table: "Weaving_DailyOperationWarpingBeamProducts",
                newName: "WarpingBeamLength");

            migrationBuilder.RenameColumn(
                name: "PressRollUomId",
                table: "Weaving_DailyOperationWarpingBeamProducts",
                newName: "RightLooseCreel");

            migrationBuilder.AlterColumn<int>(
                name: "Tention",
                table: "Weaving_DailyOperationWarpingBeamProducts",
                nullable: true,
                oldClrType: typeof(double),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BrokenThreadsCause",
                table: "Weaving_DailyOperationWarpingBeamProducts",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ConeDeficient",
                table: "Weaving_DailyOperationWarpingBeamProducts",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LeftLooseCreel",
                table: "Weaving_DailyOperationWarpingBeamProducts",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LooseThreadsAmount",
                table: "Weaving_DailyOperationWarpingBeamProducts",
                nullable: true);
        }
    }
}
