using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Aps.Migrations
{
    public partial class AddScheduleRecord : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ScheduleRecordId",
                table: "ApsOrders",
                type: "char(36)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ScheduleRecordId",
                table: "ApsJob",
                type: "char(36)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ScheduleRecords",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    ScheduleFinishTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    RecordState = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScheduleRecords", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApsOrders_ScheduleRecordId",
                table: "ApsOrders",
                column: "ScheduleRecordId");

            migrationBuilder.CreateIndex(
                name: "IX_ApsJob_ScheduleRecordId",
                table: "ApsJob",
                column: "ScheduleRecordId");

            migrationBuilder.AddForeignKey(
                name: "FK_ApsJob_ScheduleRecords_ScheduleRecordId",
                table: "ApsJob",
                column: "ScheduleRecordId",
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApsJob_ScheduleRecords_ScheduleRecordId",
                table: "ApsJob");

            migrationBuilder.DropForeignKey(
                name: "FK_ApsOrders_ScheduleRecords_ScheduleRecordId",
                table: "ApsOrders");

            migrationBuilder.DropTable(
                name: "ScheduleRecords");

            migrationBuilder.DropIndex(
                name: "IX_ApsOrders_ScheduleRecordId",
                table: "ApsOrders");

            migrationBuilder.DropIndex(
                name: "IX_ApsJob_ScheduleRecordId",
                table: "ApsJob");

            migrationBuilder.DropColumn(
                name: "ScheduleRecordId",
                table: "ApsOrders");

            migrationBuilder.DropColumn(
                name: "ScheduleRecordId",
                table: "ApsJob");
        }
    }
}
