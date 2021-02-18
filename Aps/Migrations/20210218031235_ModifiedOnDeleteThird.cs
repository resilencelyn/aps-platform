using Microsoft.EntityFrameworkCore.Migrations;

namespace Aps.Migrations
{
    public partial class ModifiedOnDeleteThird : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApsProcessResource_ApsProcess_ProcessId",
                table: "ApsProcessResource");

            migrationBuilder.AddForeignKey(
                name: "FK_ApsProcessResource_ApsProcess_ProcessId",
                table: "ApsProcessResource",
                column: "ProcessId",
                principalTable: "ApsProcess",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApsProcessResource_ApsProcess_ProcessId",
                table: "ApsProcessResource");

            migrationBuilder.AddForeignKey(
                name: "FK_ApsProcessResource_ApsProcess_ProcessId",
                table: "ApsProcessResource",
                column: "ProcessId",
                principalTable: "ApsProcess",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
