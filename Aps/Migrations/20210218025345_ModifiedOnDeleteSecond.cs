using Microsoft.EntityFrameworkCore.Migrations;

namespace Aps.Migrations
{
    public partial class ModifiedOnDeleteSecond : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApsAssemblyProcessSemiProduct_ApsProcess_ApsAssemblyProcessId",
                table: "ApsAssemblyProcessSemiProduct");

            migrationBuilder.AddForeignKey(
                name: "FK_ApsAssemblyProcessSemiProduct_ApsProcess_ApsAssemblyProcessId",
                table: "ApsAssemblyProcessSemiProduct",
                column: "ApsAssemblyProcessId",
                principalTable: "ApsProcess",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApsAssemblyProcessSemiProduct_ApsProcess_ApsAssemblyProcessId",
                table: "ApsAssemblyProcessSemiProduct");

            migrationBuilder.AddForeignKey(
                name: "FK_ApsAssemblyProcessSemiProduct_ApsProcess_ApsAssemblyProcessId",
                table: "ApsAssemblyProcessSemiProduct",
                column: "ApsAssemblyProcessId",
                principalTable: "ApsProcess",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
