using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class TestLoom : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Weaving_DailyOperationLoomBeamProducts_Weaving_DailyOperationLoomDocuments_DailyOperationLoomDocumentId",
                table: "Weaving_DailyOperationLoomBeamProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_Weaving_DailyOperationLoomHistories_Weaving_DailyOperationLoomDocuments_DailyOperationLoomDocumentId",
                table: "Weaving_DailyOperationLoomHistories");

            migrationBuilder.DropIndex(
                name: "IX_Weaving_DailyOperationLoomHistories_DailyOperationLoomDocumentId",
                table: "Weaving_DailyOperationLoomHistories");

            migrationBuilder.DropIndex(
                name: "IX_Weaving_DailyOperationLoomBeamProducts_DailyOperationLoomDocumentId",
                table: "Weaving_DailyOperationLoomBeamProducts");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Weaving_DailyOperationLoomHistories_DailyOperationLoomDocumentId",
                table: "Weaving_DailyOperationLoomHistories",
                column: "DailyOperationLoomDocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_Weaving_DailyOperationLoomBeamProducts_DailyOperationLoomDocumentId",
                table: "Weaving_DailyOperationLoomBeamProducts",
                column: "DailyOperationLoomDocumentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Weaving_DailyOperationLoomBeamProducts_Weaving_DailyOperationLoomDocuments_DailyOperationLoomDocumentId",
                table: "Weaving_DailyOperationLoomBeamProducts",
                column: "DailyOperationLoomDocumentId",
                principalTable: "Weaving_DailyOperationLoomDocuments",
                principalColumn: "Identity",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Weaving_DailyOperationLoomHistories_Weaving_DailyOperationLoomDocuments_DailyOperationLoomDocumentId",
                table: "Weaving_DailyOperationLoomHistories",
                column: "DailyOperationLoomDocumentId",
                principalTable: "Weaving_DailyOperationLoomDocuments",
                principalColumn: "Identity",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
