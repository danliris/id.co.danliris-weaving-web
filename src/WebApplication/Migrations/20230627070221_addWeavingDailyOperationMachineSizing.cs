using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class addWeavingDailyOperationMachineSizing : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WeavingDailyOperationMachineSizing",
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
                    PeriodeId = table.Column<double>(nullable: false),
                    Periode = table.Column<string>(nullable: true),
                    Year = table.Column<string>(nullable: true),
                    Date = table.Column<int>(nullable: false),
                    Week = table.Column<int>(nullable: false),
                    MachineSizing = table.Column<string>(nullable: true),
                    Shift = table.Column<string>(nullable: true),
                    Group = table.Column<string>(nullable: true),
                    Lot = table.Column<string>(nullable: true),
                    SP = table.Column<double>(nullable: false),
                    YearProduction = table.Column<string>(nullable: true),
                    SPYear = table.Column<string>(nullable: true),
                    WarpType = table.Column<string>(nullable: true),
                    AL = table.Column<string>(nullable: true),
                    Construction = table.Column<string>(nullable: true),
                    Code = table.Column<string>(nullable: true),
                    ThreadOrigin = table.Column<string>(nullable: true),
                    Recipe = table.Column<string>(nullable: true),
                    Water = table.Column<double>(nullable: false),
                    BeamNo = table.Column<string>(nullable: true),
                    BeamWidth = table.Column<string>(nullable: true),
                    TekSQ = table.Column<string>(nullable: true),
                    ThreadCount = table.Column<double>(nullable: false),
                    Ne = table.Column<double>(nullable: false),
                    TempSD1 = table.Column<double>(nullable: false),
                    TempSD2 = table.Column<double>(nullable: false),
                    VisCoseSD1 = table.Column<double>(nullable: false),
                    VisCoseSD2 = table.Column<double>(nullable: false),
                    F1 = table.Column<double>(nullable: false),
                    F2 = table.Column<double>(nullable: false),
                    FDS = table.Column<double>(nullable: false),
                    FW = table.Column<double>(nullable: false),
                    FP = table.Column<double>(nullable: false),
                    A12 = table.Column<double>(nullable: false),
                    A34 = table.Column<double>(nullable: false),
                    B12 = table.Column<string>(nullable: true),
                    B34 = table.Column<string>(nullable: true),
                    C1234 = table.Column<double>(nullable: false),
                    Pis = table.Column<string>(nullable: true),
                    AddedLength = table.Column<string>(nullable: true),
                    Length = table.Column<double>(nullable: false),
                    EmptyBeamWeight = table.Column<double>(nullable: false),
                    Bruto = table.Column<double>(nullable: false),
                    Netto = table.Column<double>(nullable: false),
                    Teoritis = table.Column<double>(nullable: false),
                    SPU = table.Column<string>(nullable: true),
                    WarpingLenght = table.Column<double>(nullable: false),
                    FinalCounter = table.Column<double>(nullable: false),
                    Draft = table.Column<string>(nullable: true),
                    Speed = table.Column<double>(nullable: false),
                    Information = table.Column<string>(nullable: true),
                    SpeedMin = table.Column<double>(nullable: false),
                    Capacity = table.Column<double>(nullable: false),
                    Efficiency = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeavingDailyOperationMachineSizing", x => x.Identity);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WeavingDailyOperationMachineSizing");
        }
    }
}
