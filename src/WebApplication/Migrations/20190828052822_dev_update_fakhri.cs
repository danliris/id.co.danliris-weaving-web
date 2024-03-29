﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class dev_update_fakhri : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "YarnStrandsProcessed",
                table: "Weaving_DailyOperationReachingTyingDetails",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "YarnStrandsProcessed",
                table: "Weaving_DailyOperationReachingTyingDetails");
        }
    }
}
