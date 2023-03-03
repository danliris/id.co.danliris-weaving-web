using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class AddWrappingTroubleMachineDocument : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "GradeD",
                table: "Weaving_EstimationProductDetails",
                nullable: true,
                oldClrType: typeof(double));

            migrationBuilder.CreateTable(
                name: "Weaving_WarpingMachineTroubleDocuments",
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
                    ContinueDate = table.Column<DateTimeOffset>(nullable: false),
                    StopDate = table.Column<DateTimeOffset>(nullable: false),
                    OrderDocumentId = table.Column<Guid>(nullable: true),
                    Process = table.Column<string>(nullable: true),
                    OperatorDocumentId = table.Column<Guid>(nullable: true),
                    MachineDocumentId = table.Column<Guid>(nullable: true),
                    Trouble = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    WeavingUnitId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Weaving_WarpingMachineTroubleDocuments", x => x.Identity);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Weaving_WarpingMachineTroubleDocuments");

            migrationBuilder.AlterColumn<double>(
                name: "GradeD",
                table: "Weaving_EstimationProductDetails",
                nullable: false,
                oldClrType: typeof(double),
                oldNullable: true);
        }
    }
}
