using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class iniitial_LoomMachines : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WeavingDailyOperationLoomMachines",
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
                    YearPeriode = table.Column<string>(nullable: true),
                    MonthPeriode = table.Column<string>(nullable: true),
                    MonthPeriodeId = table.Column<int>(nullable: false),
                    Shift = table.Column<string>(nullable: true),
                    MCNo = table.Column<string>(nullable: true),
                    SPNo = table.Column<string>(nullable: true),
                    Year = table.Column<string>(nullable: true),
                    TA = table.Column<string>(nullable: true),
                    Warp = table.Column<string>(nullable: true),
                    Weft = table.Column<string>(nullable: true),
                    Length = table.Column<string>(nullable: true),
                    WarpType = table.Column<string>(nullable: true),
                    WeftType = table.Column<string>(nullable: true),
                    WeftType2 = table.Column<string>(nullable: true),
                    WeftType3 = table.Column<string>(nullable: true),
                    AL = table.Column<string>(nullable: true),
                    AP = table.Column<string>(nullable: true),
                    AP2 = table.Column<string>(nullable: true),
                    AP3 = table.Column<string>(nullable: true),
                    Thread = table.Column<string>(nullable: true),
                    Construction = table.Column<string>(nullable: true),
                    ThreadType = table.Column<string>(nullable: true),
                    MonthId = table.Column<string>(nullable: true),
                    ProductionCMPX = table.Column<string>(nullable: true),
                    EFFMC = table.Column<string>(nullable: true),
                    RPM = table.Column<string>(nullable: true),
                    T = table.Column<string>(nullable: true),
                    F = table.Column<string>(nullable: true),
                    W = table.Column<string>(nullable: true),
                    L = table.Column<string>(nullable: true),
                    Column1 = table.Column<string>(nullable: true),
                    Production = table.Column<string>(nullable: true),
                    Production100 = table.Column<string>(nullable: true),
                    PercentEff = table.Column<string>(nullable: true),
                    MC2Eff = table.Column<string>(nullable: true),
                    RPMProduction100 = table.Column<string>(nullable: true),
                    Location = table.Column<string>(nullable: true),
                    MachineType = table.Column<string>(nullable: true),
                    MachineNameType = table.Column<string>(nullable: true),
                    Block = table.Column<string>(nullable: true),
                    BlockName = table.Column<string>(nullable: true),
                    MTCLock = table.Column<string>(nullable: true),
                    MTC = table.Column<string>(nullable: true),
                    MTCName = table.Column<string>(nullable: true),
                    MCNo2 = table.Column<string>(nullable: true),
                    MCRPM = table.Column<string>(nullable: true),
                    Row = table.Column<string>(nullable: true),
                    Operator = table.Column<string>(nullable: true),
                    SPYear = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeavingDailyOperationLoomMachines", x => x.Identity);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WeavingDailyOperationLoomMachines");
        }
    }
}
