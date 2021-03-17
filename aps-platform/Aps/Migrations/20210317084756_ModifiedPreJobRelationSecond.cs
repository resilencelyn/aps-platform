using Microsoft.EntityFrameworkCore.Migrations;

namespace Aps.Migrations
{
    public partial class ModifiedPreJobRelationSecond : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApsOrders_ScheduleRecords_ScheduleRecordId",
                table: "ApsOrders");

            migrationBuilder.AddForeignKey(
                name: "FK_ApsOrders_ScheduleRecords_ScheduleRecordId",
                table: "ApsOrders",
                column: "ScheduleRecordId",
                principalTable: "ScheduleRecords",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApsOrders_ScheduleRecords_ScheduleRecordId",
                table: "ApsOrders");

            migrationBuilder.AddForeignKey(
                name: "FK_ApsOrders_ScheduleRecords_ScheduleRecordId",
                table: "ApsOrders",
                column: "ScheduleRecordId",
                principalTable: "ScheduleRecords",
                principalColumn: "Id");
        }
    }
}
