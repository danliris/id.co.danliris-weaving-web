using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class RevampSizing : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BrokenBeam",
                table: "Weaving_DailyOperationSizingHistories");

            migrationBuilder.DropColumn(
                name: "Information",
                table: "Weaving_DailyOperationSizingHistories");

            migrationBuilder.RenameColumn(
                name: "MachineTroubled",
                table: "Weaving_DailyOperationSizingHistories",
                newName: "BrokenPerShift");

            migrationBuilder.AddColumn<int>(
                name: "TotalBroken",
                table: "Weaving_DailyOperationSizingBeamProducts",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalBroken",
                table: "Weaving_DailyOperationSizingBeamProducts");

            migrationBuilder.RenameColumn(
                name: "BrokenPerShift",
                table: "Weaving_DailyOperationSizingHistories",
                newName: "MachineTroubled");

            migrationBuilder.AddColumn<int>(
                name: "BrokenBeam",
                table: "Weaving_DailyOperationSizingHistories",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Information",
                table: "Weaving_DailyOperationSizingHistories",
                nullable: true);
        }
    }
}
