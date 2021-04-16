﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace Aps.Migrations
{
    public partial class ModifiedJobSceduleRecoordRelatioon : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApsJob_ScheduleRecords_ScheduleRecordId",
                table: "ApsJob");

            migrationBuilder.AddForeignKey(
                name: "FK_ApsJob_ScheduleRecords_ScheduleRecordId",
                table: "ApsJob",
                column: "ScheduleRecordId",
                principalTable: "ScheduleRecords",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApsJob_ScheduleRecords_ScheduleRecordId",
                table: "ApsJob");

            migrationBuilder.AddForeignKey(
                name: "FK_ApsJob_ScheduleRecords_ScheduleRecordId",
                table: "ApsJob",
                column: "ScheduleRecordId",
                principalTable: "ScheduleRecords",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
