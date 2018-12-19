using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class Add_ProductGoods : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Weaving_Orders_GoodsConstruction_CompositionId",
                table: "Weaving_Orders");

            migrationBuilder.DropIndex(
                name: "IX_Weaving_Orders_CompositionId",
                table: "Weaving_Orders");

            migrationBuilder.AddColumn<Guid>(
                name: "GoodsId",
                table: "GoodsConstruction",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

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

            migrationBuilder.CreateIndex(
                name: "IX_GoodsConstruction_GoodsId",
                table: "GoodsConstruction",
                column: "GoodsId");

            migrationBuilder.AddForeignKey(
                name: "FK_GoodsConstruction_ProductGoods_GoodsId",
                table: "GoodsConstruction",
                column: "GoodsId",
                principalTable: "ProductGoods",
                principalColumn: "Identity",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GoodsConstruction_ProductGoods_GoodsId",
                table: "GoodsConstruction");

            migrationBuilder.DropTable(
                name: "ProductGoods");

            migrationBuilder.DropIndex(
                name: "IX_GoodsConstruction_GoodsId",
                table: "GoodsConstruction");

            migrationBuilder.DropColumn(
                name: "GoodsId",
                table: "GoodsConstruction");

            migrationBuilder.CreateIndex(
                name: "IX_Weaving_Orders_CompositionId",
                table: "Weaving_Orders",
                column: "CompositionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Weaving_Orders_GoodsConstruction_CompositionId",
                table: "Weaving_Orders",
                column: "CompositionId",
                principalTable: "GoodsConstruction",
                principalColumn: "Identity",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
