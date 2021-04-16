using Microsoft.EntityFrameworkCore.Migrations;

namespace Aps.Migrations
{
    public partial class ModifiedJobResourceRelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ApsResourceId",
                table: "ApsJobApsResource",
                newName: "ApsResourceId1");

            migrationBuilder.AddForeignKey(
                name: "FK_ApsJobApsResource_ApsJob_WorkJobsId",
                table: "ApsJobApsResource",
                column: "WorkJobsId",
                principalTable: "ApsJob",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ApsJobApsResource_ApsResources_ApsResourceId1",
                table: "ApsJobApsResource",
                column: "ApsResourceId1",
                principalTable: "ApsResources",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApsJobApsResource_ApsJob_WorkJobsId",
                table: "ApsJobApsResource");

            migrationBuilder.DropForeignKey(
                name: "FK_ApsJobApsResource_ApsResources_ApsResourceId1",
                table: "ApsJobApsResource");

            migrationBuilder.RenameColumn(
                name: "ApsResourceId1",
                table: "ApsJobApsResource",
                newName: "ApsResourceId");

            migrationBuilder.AddForeignKey(
                name: "FK_ApsJobApsResource_ApsJob_WorkJobsId",
                table: "ApsJobApsResource",
                column: "WorkJobsId",
                principalTable: "ApsJob",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ApsJobApsResource_ApsResources_ApsResourceId",
                table: "ApsJobApsResource",
                column: "ApsResourceId",
                principalTable: "ApsResources",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
