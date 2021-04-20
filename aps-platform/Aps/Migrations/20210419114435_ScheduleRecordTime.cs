using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Aps.Migrations
{
    public partial class ScheduleRecordTime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ScheduleStartTime",
                table: "ScheduleRecords",
                type: "datetime(6)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ScheduleStartTime",
                table: "ScheduleRecords");
        }
    }
}
