using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Aps.Migrations
{
    public partial class AddPropertyToAssemblyJob : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ApsAssemblyJobId",
                table: "ApsJob",
                type: "char(36)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ApsJob_ApsAssemblyJobId",
                table: "ApsJob",
                column: "ApsAssemblyJobId");

            migrationBuilder.AddForeignKey(
                name: "FK_ApsJob_ApsJob_ApsAssemblyJobId",
                table: "ApsJob",
                column: "ApsAssemblyJobId",
                principalTable: "ApsJob",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApsJob_ApsJob_ApsAssemblyJobId",
                table: "ApsJob");

            migrationBuilder.DropIndex(
                name: "IX_ApsJob_ApsAssemblyJobId",
                table: "ApsJob");

            migrationBuilder.DropColumn(
                name: "ApsAssemblyJobId",
                table: "ApsJob");
        }
    }
}
