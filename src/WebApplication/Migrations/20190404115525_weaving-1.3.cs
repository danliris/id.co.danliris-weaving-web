using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class weaving13 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Weaving_DailyOperationMachineDocuments",
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
                    Time = table.Column<DateTimeOffset>(nullable: false),
                    Information = table.Column<string>(maxLength: 255, nullable: true),
                    MachineId = table.Column<Guid>(nullable: true),
                    UnitId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Weaving_DailyOperationMachineDocuments", x => x.Identity);
                });

            migrationBuilder.CreateTable(
                name: "Weaving_DailyOperationMachineDetails",
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
                    OrderDocument = table.Column<string>(maxLength: 2000, nullable: true),
                    Shift = table.Column<string>(nullable: true),
                    Beam = table.Column<string>(nullable: true),
                    DOMTime = table.Column<string>(maxLength: 2000, nullable: true),
                    DailyOperationMachineDocumentId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Weaving_DailyOperationMachineDetails", x => x.Identity);
                    table.ForeignKey(
                        name: "FK_Weaving_DailyOperationMachineDetails_Weaving_DailyOperationMachineDocuments_DailyOperationMachineDocumentId",
                        column: x => x.DailyOperationMachineDocumentId,
                        principalTable: "Weaving_DailyOperationMachineDocuments",
                        principalColumn: "Identity",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Weaving_DailyOperationMachineDetails_DailyOperationMachineDocumentId",
                table: "Weaving_DailyOperationMachineDetails",
                column: "DailyOperationMachineDocumentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Weaving_DailyOperationMachineDetails");

            migrationBuilder.DropTable(
                name: "Weaving_DailyOperationMachineDocuments");
        }
    }
}
