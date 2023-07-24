using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class initial_stockBeams : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WeavingBeamStocks",
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
                    Beam = table.Column<string>(nullable: true),
                    Code = table.Column<string>(nullable: true),
                    Sizing = table.Column<string>(nullable: true),
                    InReaching = table.Column<string>(nullable: true),
                    Reaching = table.Column<string>(nullable: true),
                    Information = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeavingBeamStocks", x => x.Identity);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WeavingBeamStocks");
        }
    }
}
