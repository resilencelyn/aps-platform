using Microsoft.EntityFrameworkCore.Migrations;

namespace Aps.Migrations
{
    public partial class ModifiedPreJobRelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApsJob_ScheduleRecords_ApsManufactureJob_ScheduleRecordId",
                table: "ApsJob");

            migrationBuilder.DropForeignKey(
                name: "FK_ApsOrders_ScheduleRecords_ScheduleRecordId",
                table: "ApsOrders");

            migrationBuilder.AddForeignKey(
                name: "FK_ApsJob_ScheduleRecords_ApsManufactureJob_ScheduleRecordId",
                table: "ApsJob",
                column: "ApsManufactureJob_ScheduleRecordId",
                principalTable: "ScheduleRecords",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ApsOrders_ScheduleRecords_ScheduleRecordId",
                table: "ApsOrders",
                column: "ScheduleRecordId",
                principalTable: "ScheduleRecords",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApsJob_ScheduleRecords_ApsManufactureJob_ScheduleRecordId",
                table: "ApsJob");

            migrationBuilder.DropForeignKey(
                name: "FK_ApsOrders_ScheduleRecords_ScheduleRecordId",
                table: "ApsOrders");

            migrationBuilder.AddForeignKey(
                name: "FK_ApsJob_ScheduleRecords_ApsManufactureJob_ScheduleRecordId",
                table: "ApsJob",
                column: "ApsManufactureJob_ScheduleRecordId",
                principalTable: "ScheduleRecords",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ApsOrders_ScheduleRecords_ScheduleRecordId",
                table: "ApsOrders",
                column: "ScheduleRecordId",
                principalTable: "ScheduleRecords",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
