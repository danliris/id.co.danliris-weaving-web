using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class AddWarpingBeamLengthUOMIdOnWarping : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "WarpingBeamLengthUOMId",
                table: "Weaving_DailyOperationWarpingBeamProducts",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WarpingBeamLengthUOMId",
                table: "Weaving_DailyOperationWarpingBeamProducts");
        }
    }
}
