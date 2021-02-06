using Microsoft.EntityFrameworkCore.Migrations;

namespace Aps.Migrations
{
    public partial class ModifyProductProcessOneToOne : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApsProcess_ApsProduct_OutputFinishedProductProductId",
                table: "ApsProcess");

            migrationBuilder.DropIndex(
                name: "IX_ApsProcess_OutputFinishedProductProductId",
                table: "ApsProcess");

            migrationBuilder.DropColumn(
                name: "OutputFinishedProductProductId",
                table: "ApsProcess");

            migrationBuilder.AddColumn<string>(
                name: "ApsAssemblyProcessId",
                table: "ApsProduct",
                type: "varchar(255) CHARACTER SET utf8mb4",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OutputFinishedProductId",
                table: "ApsProcess",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ApsProduct_ApsAssemblyProcessId",
                table: "ApsProduct",
                column: "ApsAssemblyProcessId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ApsProduct_ApsProcess_ApsAssemblyProcessId",
                table: "ApsProduct",
                column: "ApsAssemblyProcessId",
                principalTable: "ApsProcess",
                principalColumn: "PartId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApsProduct_ApsProcess_ApsAssemblyProcessId",
                table: "ApsProduct");

            migrationBuilder.DropIndex(
                name: "IX_ApsProduct_ApsAssemblyProcessId",
                table: "ApsProduct");

            migrationBuilder.DropColumn(
                name: "ApsAssemblyProcessId",
                table: "ApsProduct");

            migrationBuilder.DropColumn(
                name: "OutputFinishedProductId",
                table: "ApsProcess");

            migrationBuilder.AddColumn<string>(
                name: "OutputFinishedProductProductId",
                table: "ApsProcess",
                type: "varchar(255) CHARACTER SET utf8mb4",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ApsProcess_OutputFinishedProductProductId",
                table: "ApsProcess",
                column: "OutputFinishedProductProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_ApsProcess_ApsProduct_OutputFinishedProductProductId",
                table: "ApsProcess",
                column: "OutputFinishedProductProductId",
                principalTable: "ApsProduct",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
