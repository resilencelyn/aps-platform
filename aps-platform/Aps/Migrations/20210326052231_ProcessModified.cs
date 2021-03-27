using Microsoft.EntityFrameworkCore.Migrations;

namespace Aps.Migrations
{
    public partial class ProcessModified : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProcessId",
                table: "ApsJob",
                type: "varchar(255) CHARACTER SET utf8mb4",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ApsJob_ProcessId",
                table: "ApsJob",
                column: "ProcessId");

            migrationBuilder.AddForeignKey(
                name: "FK_ApsJob_ApsProcess_ProcessId",
                table: "ApsJob",
                column: "ProcessId",
                principalTable: "ApsProcess",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApsJob_ApsProcess_ProcessId",
                table: "ApsJob");

            migrationBuilder.DropIndex(
                name: "IX_ApsJob_ProcessId",
                table: "ApsJob");

            migrationBuilder.DropColumn(
                name: "ProcessId",
                table: "ApsJob");
        }
    }
}
