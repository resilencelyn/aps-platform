using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Aps.Migrations
{
    public partial class AddPrejobInJob : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ApsManufactureJob_ScheduleRecordId",
                table: "ApsJob",
                type: "char(36)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ApsJob_ApsManufactureJob_ScheduleRecordId",
                table: "ApsJob",
                column: "ApsManufactureJob_ScheduleRecordId");

            migrationBuilder.AddForeignKey(
                name: "FK_ApsJob_ScheduleRecords_ApsManufactureJob_ScheduleRecordId",
                table: "ApsJob",
                column: "ApsManufactureJob_ScheduleRecordId",
                principalTable: "ScheduleRecords",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApsJob_ScheduleRecords_ApsManufactureJob_ScheduleRecordId",
                table: "ApsJob");

            migrationBuilder.DropIndex(
                name: "IX_ApsJob_ApsManufactureJob_ScheduleRecordId",
                table: "ApsJob");

            migrationBuilder.DropColumn(
                name: "ApsManufactureJob_ScheduleRecordId",
                table: "ApsJob");
        }
    }
}
