using Microsoft.EntityFrameworkCore.Migrations;

namespace Aps.Migrations
{
    public partial class ModifiedOnDeleteFifth : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApsProcess_ApsSemiProducts_ApsSemiProductId",
                table: "ApsProcess");

            migrationBuilder.AddForeignKey(
                name: "FK_ApsProcess_ApsSemiProducts_ApsSemiProductId",
                table: "ApsProcess",
                column: "ApsSemiProductId",
                principalTable: "ApsSemiProducts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApsProcess_ApsSemiProducts_ApsSemiProductId",
                table: "ApsProcess");

            migrationBuilder.AddForeignKey(
                name: "FK_ApsProcess_ApsSemiProducts_ApsSemiProductId",
                table: "ApsProcess",
                column: "ApsSemiProductId",
                principalTable: "ApsSemiProducts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
