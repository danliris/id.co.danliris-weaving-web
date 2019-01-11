using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProductGoods",
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
                    ProductId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductGoods", x => x.Identity);
                });

            migrationBuilder.CreateTable(
                name: "Weaving_Orders",
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
                    UnitDepartmentId = table.Column<int>(nullable: false),
                    MachineId = table.Column<int>(nullable: false),
                    YarnCodesJson = table.Column<string>(maxLength: 255, nullable: true),
                    State = table.Column<int>(nullable: false),
                    OrderDate = table.Column<DateTimeOffset>(nullable: false),
                    BlendedJson = table.Column<string>(maxLength: 255, nullable: true),
                    UserId = table.Column<string>(nullable: true),
                    CompositionId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Weaving_Orders", x => x.Identity);
                });

            migrationBuilder.CreateTable(
                name: "WeavingOrderDocuments",
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
                    OrderNumber = table.Column<string>(nullable: true),
                    DateOrdered = table.Column<DateTimeOffset>(nullable: false),
                    WarpOrigin = table.Column<string>(nullable: true),
                    WeftOrigin = table.Column<string>(nullable: true),
                    WholeGrade = table.Column<int>(nullable: false),
                    YarnType = table.Column<string>(nullable: true),
                    Period = table.Column<string>(maxLength: 255, nullable: true),
                    Composition = table.Column<string>(maxLength: 255, nullable: true),
                    FabricConstructionDocument = table.Column<string>(maxLength: 255, nullable: true),
                    WeavingUnit = table.Column<string>(maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeavingOrderDocuments", x => x.Identity);
                });

            migrationBuilder.CreateTable(
                name: "GoodsConstruction",
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
                    MaterialIdsJson = table.Column<string>(maxLength: 500, nullable: true),
                    GoodsId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoodsConstruction", x => x.Identity);
                    table.ForeignKey(
                        name: "FK_GoodsConstruction_ProductGoods_GoodsId",
                        column: x => x.GoodsId,
                        principalTable: "ProductGoods",
                        principalColumn: "Identity",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GoodsConstruction_GoodsId",
                table: "GoodsConstruction",
                column: "GoodsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GoodsConstruction");

            migrationBuilder.DropTable(
                name: "Weaving_Orders");

            migrationBuilder.DropTable(
                name: "WeavingOrderDocuments");

            migrationBuilder.DropTable(
                name: "ProductGoods");
        }
    }
}
