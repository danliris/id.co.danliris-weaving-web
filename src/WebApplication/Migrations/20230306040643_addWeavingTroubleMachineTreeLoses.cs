using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class addWeavingTroubleMachineTreeLoses : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WeavingTroubleMachineTreeLoses",
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
                    Shift = table.Column<string>(nullable: true),
                    Description = table.Column<string>(maxLength: 300, nullable: true),
                    WarpingMachineNo = table.Column<string>(nullable: true),
                    Group = table.Column<string>(maxLength: 50, nullable: true),
                    Code = table.Column<string>(maxLength: 100, nullable: true),
                    DownTimeMC = table.Column<DateTime>(nullable: false),
                    TimePerMinutes = table.Column<int>(nullable: false),
                    Start = table.Column<DateTime>(nullable: false),
                    Finish = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeavingTroubleMachineTreeLoses", x => x.Identity);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WeavingTroubleMachineTreeLoses");
        }
    }
}
