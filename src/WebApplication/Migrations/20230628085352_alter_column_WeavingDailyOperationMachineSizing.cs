using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class alter_column_WeavingDailyOperationMachineSizing : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Week",
                table: "WeavingDailyOperationMachineSizing",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<string>(
                name: "Water",
                table: "WeavingDailyOperationMachineSizing",
                nullable: true,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<string>(
                name: "WarpingLenght",
                table: "WeavingDailyOperationMachineSizing",
                nullable: true,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<string>(
                name: "VisCoseSD2",
                table: "WeavingDailyOperationMachineSizing",
                nullable: true,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<string>(
                name: "VisCoseSD1",
                table: "WeavingDailyOperationMachineSizing",
                nullable: true,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<string>(
                name: "ThreadCount",
                table: "WeavingDailyOperationMachineSizing",
                nullable: true,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<string>(
                name: "Teoritis",
                table: "WeavingDailyOperationMachineSizing",
                nullable: true,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<string>(
                name: "TempSD2",
                table: "WeavingDailyOperationMachineSizing",
                nullable: true,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<string>(
                name: "TempSD1",
                table: "WeavingDailyOperationMachineSizing",
                nullable: true,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<string>(
                name: "SpeedMin",
                table: "WeavingDailyOperationMachineSizing",
                nullable: true,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<string>(
                name: "Speed",
                table: "WeavingDailyOperationMachineSizing",
                nullable: true,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<string>(
                name: "SP",
                table: "WeavingDailyOperationMachineSizing",
                nullable: true,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<int>(
                name: "PeriodeId",
                table: "WeavingDailyOperationMachineSizing",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<string>(
                name: "Netto",
                table: "WeavingDailyOperationMachineSizing",
                nullable: true,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<string>(
                name: "Ne",
                table: "WeavingDailyOperationMachineSizing",
                nullable: true,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<string>(
                name: "Length",
                table: "WeavingDailyOperationMachineSizing",
                nullable: true,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<string>(
                name: "FinalCounter",
                table: "WeavingDailyOperationMachineSizing",
                nullable: true,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<string>(
                name: "FW",
                table: "WeavingDailyOperationMachineSizing",
                nullable: true,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<string>(
                name: "FP",
                table: "WeavingDailyOperationMachineSizing",
                nullable: true,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<string>(
                name: "FDS",
                table: "WeavingDailyOperationMachineSizing",
                nullable: true,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<string>(
                name: "F2",
                table: "WeavingDailyOperationMachineSizing",
                nullable: true,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<string>(
                name: "F1",
                table: "WeavingDailyOperationMachineSizing",
                nullable: true,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<string>(
                name: "EmptyBeamWeight",
                table: "WeavingDailyOperationMachineSizing",
                nullable: true,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<string>(
                name: "Capacity",
                table: "WeavingDailyOperationMachineSizing",
                nullable: true,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<string>(
                name: "C1234",
                table: "WeavingDailyOperationMachineSizing",
                nullable: true,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<string>(
                name: "Bruto",
                table: "WeavingDailyOperationMachineSizing",
                nullable: true,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<string>(
                name: "A34",
                table: "WeavingDailyOperationMachineSizing",
                nullable: true,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<string>(
                name: "A12",
                table: "WeavingDailyOperationMachineSizing",
                nullable: true,
                oldClrType: typeof(double));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Week",
                table: "WeavingDailyOperationMachineSizing",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "Water",
                table: "WeavingDailyOperationMachineSizing",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "WarpingLenght",
                table: "WeavingDailyOperationMachineSizing",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "VisCoseSD2",
                table: "WeavingDailyOperationMachineSizing",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "VisCoseSD1",
                table: "WeavingDailyOperationMachineSizing",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "ThreadCount",
                table: "WeavingDailyOperationMachineSizing",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "Teoritis",
                table: "WeavingDailyOperationMachineSizing",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "TempSD2",
                table: "WeavingDailyOperationMachineSizing",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "TempSD1",
                table: "WeavingDailyOperationMachineSizing",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "SpeedMin",
                table: "WeavingDailyOperationMachineSizing",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "Speed",
                table: "WeavingDailyOperationMachineSizing",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "SP",
                table: "WeavingDailyOperationMachineSizing",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "PeriodeId",
                table: "WeavingDailyOperationMachineSizing",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<double>(
                name: "Netto",
                table: "WeavingDailyOperationMachineSizing",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "Ne",
                table: "WeavingDailyOperationMachineSizing",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "Length",
                table: "WeavingDailyOperationMachineSizing",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "FinalCounter",
                table: "WeavingDailyOperationMachineSizing",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "FW",
                table: "WeavingDailyOperationMachineSizing",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "FP",
                table: "WeavingDailyOperationMachineSizing",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "FDS",
                table: "WeavingDailyOperationMachineSizing",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "F2",
                table: "WeavingDailyOperationMachineSizing",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "F1",
                table: "WeavingDailyOperationMachineSizing",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "EmptyBeamWeight",
                table: "WeavingDailyOperationMachineSizing",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "Capacity",
                table: "WeavingDailyOperationMachineSizing",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "C1234",
                table: "WeavingDailyOperationMachineSizing",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "Bruto",
                table: "WeavingDailyOperationMachineSizing",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "A34",
                table: "WeavingDailyOperationMachineSizing",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "A12",
                table: "WeavingDailyOperationMachineSizing",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
