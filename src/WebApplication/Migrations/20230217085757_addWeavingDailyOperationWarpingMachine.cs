using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class addWeavingDailyOperationWarpingMachine : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsCompletedProduction",
                table: "Weaving_DailyOperationLoomBeamsUsed",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "WeavingDailyOperationWarpingMachines",
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
                    Day = table.Column<DateTime>(nullable: false),
                    Month = table.Column<int>(nullable: false),
                    Year = table.Column<int>(nullable: false),
                    Shift = table.Column<string>(nullable: true),
                    MCNo = table.Column<string>(nullable: true),
                    Name = table.Column<string>(maxLength: 100, nullable: true),
                    Lot = table.Column<string>(nullable: true),
                    Code = table.Column<string>(maxLength: 100, nullable: true),
                    BeamNo = table.Column<string>(maxLength: 100, nullable: true),
                    TotalCone = table.Column<int>(nullable: false),
                    ThreadNo = table.Column<string>(nullable: true),
                    Length = table.Column<double>(nullable: false),
                    Uom = table.Column<string>(nullable: true),
                    Start = table.Column<DateTime>(nullable: false),
                    Doff = table.Column<DateTime>(nullable: false),
                    HNLeft = table.Column<double>(nullable: false),
                    HNMiddle = table.Column<double>(nullable: false),
                    HNRight = table.Column<double>(nullable: false),
                    SpeedMeterPerMinute = table.Column<double>(nullable: false),
                    ThreadCut = table.Column<double>(nullable: false),
                    Capacity = table.Column<double>(nullable: false),
                    Eff = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeavingDailyOperationWarpingMachines", x => x.Identity);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WeavingDailyOperationWarpingMachines");

            migrationBuilder.DropColumn(
                name: "IsCompletedProduction",
                table: "Weaving_DailyOperationLoomBeamsUsed");
        }
    }
}
