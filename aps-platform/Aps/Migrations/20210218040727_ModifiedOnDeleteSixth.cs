using Microsoft.EntityFrameworkCore.Migrations;

namespace Aps.Migrations
{
    public partial class ModifiedOnDeleteSixth : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ResourceClassWithResource_ApsResources_ApsResourceId",
                table: "ResourceClassWithResource");

            migrationBuilder.AddForeignKey(
                name: "FK_ResourceClassWithResource_ApsResources_ApsResourceId",
                table: "ResourceClassWithResource",
                column: "ApsResourceId",
                principalTable: "ApsResources",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ResourceClassWithResource_ApsResources_ApsResourceId",
                table: "ResourceClassWithResource");

            migrationBuilder.AddForeignKey(
                name: "FK_ResourceClassWithResource_ApsResources_ApsResourceId",
                table: "ResourceClassWithResource",
                column: "ApsResourceId",
                principalTable: "ApsResources",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
