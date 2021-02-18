using Microsoft.EntityFrameworkCore.Migrations;

namespace Aps.Migrations
{
    public partial class ModifiedOnDelete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApsProductSemiProduct_ApsProducts_ApsProductId",
                table: "ApsProductSemiProduct");

            migrationBuilder.DropForeignKey(
                name: "FK_ApsProductSemiProduct_ApsSemiProducts_ApsSemiProductId",
                table: "ApsProductSemiProduct");

            migrationBuilder.AddForeignKey(
                name: "FK_ApsProductSemiProduct_ApsProducts_ApsProductId",
                table: "ApsProductSemiProduct",
                column: "ApsProductId",
                principalTable: "ApsProducts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ApsProductSemiProduct_ApsSemiProducts_ApsSemiProductId",
                table: "ApsProductSemiProduct",
                column: "ApsSemiProductId",
                principalTable: "ApsSemiProducts",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApsProductSemiProduct_ApsProducts_ApsProductId",
                table: "ApsProductSemiProduct");

            migrationBuilder.DropForeignKey(
                name: "FK_ApsProductSemiProduct_ApsSemiProducts_ApsSemiProductId",
                table: "ApsProductSemiProduct");

            migrationBuilder.AddForeignKey(
                name: "FK_ApsProductSemiProduct_ApsProducts_ApsProductId",
                table: "ApsProductSemiProduct",
                column: "ApsProductId",
                principalTable: "ApsProducts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ApsProductSemiProduct_ApsSemiProducts_ApsSemiProductId",
                table: "ApsProductSemiProduct",
                column: "ApsSemiProductId",
                principalTable: "ApsSemiProducts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
