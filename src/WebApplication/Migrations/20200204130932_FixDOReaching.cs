using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class FixDOReaching : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Weaving_DailyOperationReachingHistories_Weaving_DailyOperationReachingDocuments_DailyOperationReachingDocumentId",
                table: "Weaving_DailyOperationReachingHistories");

            migrationBuilder.DropIndex(
                name: "IX_Weaving_DailyOperationReachingHistories_DailyOperationReachingDocumentId",
                table: "Weaving_DailyOperationReachingHistories");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Weaving_DailyOperationReachingHistories_DailyOperationReachingDocumentId",
                table: "Weaving_DailyOperationReachingHistories",
                column: "DailyOperationReachingDocumentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Weaving_DailyOperationReachingHistories_Weaving_DailyOperationReachingDocuments_DailyOperationReachingDocumentId",
                table: "Weaving_DailyOperationReachingHistories",
                column: "DailyOperationReachingDocumentId",
                principalTable: "Weaving_DailyOperationReachingDocuments",
                principalColumn: "Identity",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
