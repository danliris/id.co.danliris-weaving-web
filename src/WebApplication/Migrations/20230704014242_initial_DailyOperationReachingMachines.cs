using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class initial_DailyOperationReachingMachines : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WeavingDailyOperationReachingMachines",
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
                    Date = table.Column<int>(nullable: false),
                    Month = table.Column<string>(nullable: true),
                    MonthId = table.Column<int>(nullable: false),
                    YearPeriode = table.Column<string>(nullable: true),
                    Year = table.Column<string>(nullable: true),
                    Shift = table.Column<string>(nullable: true),
                    Group = table.Column<string>(maxLength: 100, nullable: true),
                    Operator = table.Column<string>(nullable: true),
                    MCNo = table.Column<string>(nullable: true),
                    Code = table.Column<string>(maxLength: 100, nullable: true),
                    BeamNo = table.Column<string>(maxLength: 100, nullable: true),
                    ReachingInstall = table.Column<string>(nullable: true),
                    InstallEfficiency = table.Column<string>(nullable: true),
                    CM = table.Column<string>(nullable: true),
                    BeamWidth = table.Column<string>(nullable: true),
                    TotalWarp = table.Column<string>(nullable: true),
                    ReachingStrands = table.Column<string>(nullable: true),
                    ReachingEfficiency = table.Column<string>(nullable: true),
                    CombWidth = table.Column<string>(nullable: true),
                    CombStrands = table.Column<string>(nullable: true),
                    CombEfficiency = table.Column<string>(nullable: true),
                    Doffing = table.Column<string>(nullable: true),
                    DoffingEfficiency = table.Column<string>(nullable: true),
                    Webbing = table.Column<string>(nullable: true),
                    Margin = table.Column<string>(nullable: true),
                    CombNo = table.Column<string>(nullable: true),
                    ReedSpace = table.Column<string>(nullable: true),
                    Eff2 = table.Column<string>(nullable: true),
                    Checker = table.Column<string>(nullable: true),
                    Information = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeavingDailyOperationReachingMachines", x => x.Identity);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WeavingDailyOperationReachingMachines");
        }
    }
}
