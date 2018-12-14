using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class Alter_WeavingOrders_AddCompositionId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Expenses");

            migrationBuilder.DropTable(
                name: "Incomes");

            migrationBuilder.AddColumn<Guid>(
                name: "CompositionId",
                table: "Weaving_Orders",
                nullable: true);

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Weaving_Orders_GoodsConstruction_CompositionId",
                table: "Weaving_Orders");

            migrationBuilder.DropIndex(
                name: "IX_Weaving_Orders_CompositionId",
                table: "Weaving_Orders");

            migrationBuilder.DropColumn(
                name: "CompositionId",
                table: "Weaving_Orders");

            migrationBuilder.CreateTable(
                name: "Expenses",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Amount = table.Column<decimal>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Expenses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Incomes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Amount = table.Column<decimal>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Incomes", x => x.Id);
                });
        }
    }
}
