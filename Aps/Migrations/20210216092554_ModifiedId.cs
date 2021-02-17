using Microsoft.EntityFrameworkCore.Migrations;

namespace Aps.Migrations
{
    public partial class ModifiedId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApsProcessResource_ApsProcess_Id",
                table: "ApsProcessResource");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "ApsProductSemiProduct",
                newName: "ProductId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "ApsProcessResource",
                newName: "ProcessId");

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

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "ApsProductSemiProduct",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "ProcessId",
                table: "ApsProcessResource",
                newName: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ApsProcessResource_ApsProcess_Id",
                table: "ApsProcessResource",
                column: "Id",
                principalTable: "ApsProcess",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
