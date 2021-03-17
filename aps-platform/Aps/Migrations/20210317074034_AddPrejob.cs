using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Aps.Migrations
{
    public partial class AddPrejob : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "BatchId",
                table: "ApsJob",
                type: "char(36)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "PreJobId",
                table: "ApsJob",
                type: "char(36)",
                nullable: true);

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApsJob_ApsJob_PreJobId",
                table: "ApsJob");

            migrationBuilder.DropIndex(
                name: "IX_ApsJob_PreJobId",
                table: "ApsJob");

            migrationBuilder.DropColumn(
                name: "BatchId",
                table: "ApsJob");

            migrationBuilder.DropColumn(
                name: "PreJobId",
                table: "ApsJob");
        }
    }
}
