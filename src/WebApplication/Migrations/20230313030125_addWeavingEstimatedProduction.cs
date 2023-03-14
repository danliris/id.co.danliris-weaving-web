using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class addWeavingEstimatedProduction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WeavingEstimatedProductions",
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
                    Month = table.Column<string>(maxLength: 50, nullable: true),
                    MonthId = table.Column<int>(nullable: false),
                    YearPeriode = table.Column<string>(nullable: true),
                    YearSP = table.Column<string>(nullable: true),
                    SPNo = table.Column<string>(nullable: true),
                    Plait = table.Column<string>(nullable: true),
                    WarpLength = table.Column<float>(nullable: false),
                    Weft = table.Column<float>(nullable: false),
                    Width = table.Column<float>(nullable: false),
                    WarpType = table.Column<string>(nullable: true),
                    WeftType1 = table.Column<string>(nullable: true),
                    WeftType2 = table.Column<string>(nullable: true),
                    AL = table.Column<string>(nullable: true),
                    AP1 = table.Column<string>(nullable: true),
                    AP2 = table.Column<string>(nullable: true),
                    Thread = table.Column<string>(nullable: true),
                    Construction1 = table.Column<string>(maxLength: 300, nullable: true),
                    Buyer = table.Column<string>(maxLength: 50, nullable: true),
                    NumberOrder = table.Column<float>(nullable: false),
                    Construction2 = table.Column<string>(maxLength: 300, nullable: true),
                    WarpXWeft = table.Column<string>(nullable: true),
                    GradeA = table.Column<float>(nullable: false),
                    GradeB = table.Column<float>(nullable: false),
                    GradeC = table.Column<float>(nullable: false),
                    Total = table.Column<float>(nullable: false),
                    WarpBale = table.Column<float>(nullable: false),
                    WeftBale = table.Column<float>(nullable: false),
                    TotalBale = table.Column<float>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeavingEstimatedProductions", x => x.Identity);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WeavingEstimatedProductions");
        }
    }
}
