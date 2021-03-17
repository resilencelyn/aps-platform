using Microsoft.EntityFrameworkCore.Migrations;

namespace Aps.Migrations
{
    public partial class AddPreJobRelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApsJob_ApsJob_PreJobId",
                table: "ApsJob");

            migrationBuilder.DropIndex(
                name: "IX_ApsJob_PreJobId",
                table: "ApsJob");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_ApsJob_PreJobId",
                table: "ApsJob",
                column: "PreJobId");

            migrationBuilder.AddForeignKey(
                name: "FK_ApsJob_ApsJob_PreJobId",
                table: "ApsJob",
                column: "PreJobId",
                principalTable: "ApsJob",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
