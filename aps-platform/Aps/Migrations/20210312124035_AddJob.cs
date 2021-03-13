using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Aps.Migrations
{
    public partial class AddJob : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApsManufactureJobs_ApsOrders_ApsOrderId",
                table: "ApsManufactureJobs");

            migrationBuilder.DropForeignKey(
                name: "FK_ApsManufactureJobs_ApsProcess_ApsManufactureProcessId",
                table: "ApsManufactureJobs");

            migrationBuilder.DropForeignKey(
                name: "FK_ApsManufactureJobs_ApsProducts_ApsProductId",
                table: "ApsManufactureJobs");

            migrationBuilder.DropForeignKey(
                name: "FK_ApsManufactureJobs_ApsSemiProducts_ApsSemiProductId",
                table: "ApsManufactureJobs");

            migrationBuilder.DropForeignKey(
                name: "FK_ApsManufactureJobs_ProductInstances_ProductInstanceId",
                table: "ApsManufactureJobs");

            migrationBuilder.DropForeignKey(
                name: "FK_ApsManufactureJobs_SemiProductInstances_SemiProductInstanceId",
                table: "ApsManufactureJobs");

            migrationBuilder.DropForeignKey(
                name: "FK_ApsResources_ApsAssemblyJobs_ApsAssemblyJobId",
                table: "ApsResources");

            migrationBuilder.DropForeignKey(
                name: "FK_ApsResources_ApsManufactureJobs_ApsManufactureJobId",
                table: "ApsResources");

            migrationBuilder.DropTable(
                name: "ApsAssemblyJobs");

            migrationBuilder.DropIndex(
                name: "IX_ApsResources_ApsAssemblyJobId",
                table: "ApsResources");

            migrationBuilder.DropIndex(
                name: "IX_ApsResources_ApsManufactureJobId",
                table: "ApsResources");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ApsManufactureJobs",
                table: "ApsManufactureJobs");

            migrationBuilder.DropColumn(
                name: "ApsAssemblyJobId",
                table: "ApsResources");

            migrationBuilder.DropColumn(
                name: "ApsManufactureJobId",
                table: "ApsResources");

            migrationBuilder.RenameTable(
                name: "ApsManufactureJobs",
                newName: "ApsJob");

            migrationBuilder.RenameIndex(
                name: "IX_ApsManufactureJobs_SemiProductInstanceId",
                table: "ApsJob",
                newName: "IX_ApsJob_SemiProductInstanceId");

            migrationBuilder.RenameIndex(
                name: "IX_ApsManufactureJobs_ProductInstanceId",
                table: "ApsJob",
                newName: "IX_ApsJob_ProductInstanceId");

            migrationBuilder.RenameIndex(
                name: "IX_ApsManufactureJobs_ApsSemiProductId",
                table: "ApsJob",
                newName: "IX_ApsJob_ApsSemiProductId");

            migrationBuilder.RenameIndex(
                name: "IX_ApsManufactureJobs_ApsProductId",
                table: "ApsJob",
                newName: "IX_ApsJob_ApsProductId");

            migrationBuilder.RenameIndex(
                name: "IX_ApsManufactureJobs_ApsOrderId",
                table: "ApsJob",
                newName: "IX_ApsJob_ApsOrderId");

            migrationBuilder.RenameIndex(
                name: "IX_ApsManufactureJobs_ApsManufactureProcessId",
                table: "ApsJob",
                newName: "IX_ApsJob_ApsManufactureProcessId");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "ApsJob",
                type: "char(36)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<string>(
                name: "ApsAssemblyProcessId",
                table: "ApsJob",
                type: "varchar(255) CHARACTER SET utf8mb4",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "ApsJob",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: false);

            migrationBuilder.AddColumn<int>(
                name: "Workspace",
                table: "ApsJob",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ApsJob",
                table: "ApsJob",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "ApsJobApsResource",
                columns: table => new
                {
                    ApsResourceId = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", nullable: false),
                    WorkJobsId = table.Column<Guid>(type: "char(36)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApsJobApsResource", x => new { x.ApsResourceId, x.WorkJobsId });
                    table.ForeignKey(
                        name: "FK_ApsJobApsResource_ApsJob_WorkJobsId",
                        column: x => x.WorkJobsId,
                        principalTable: "ApsJob",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ApsJobApsResource_ApsResources_ApsResourceId",
                        column: x => x.ApsResourceId,
                        principalTable: "ApsResources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApsJob_ApsAssemblyProcessId",
                table: "ApsJob",
                column: "ApsAssemblyProcessId");

            migrationBuilder.CreateIndex(
                name: "IX_ApsJobApsResource_WorkJobsId",
                table: "ApsJobApsResource",
                column: "WorkJobsId");

            migrationBuilder.AddForeignKey(
                name: "FK_ApsJob_ApsOrders_ApsOrderId",
                table: "ApsJob",
                column: "ApsOrderId",
                principalTable: "ApsOrders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ApsJob_ApsProcess_ApsAssemblyProcessId",
                table: "ApsJob",
                column: "ApsAssemblyProcessId",
                principalTable: "ApsProcess",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ApsJob_ApsProcess_ApsManufactureProcessId",
                table: "ApsJob",
                column: "ApsManufactureProcessId",
                principalTable: "ApsProcess",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ApsJob_ApsProducts_ApsProductId",
                table: "ApsJob",
                column: "ApsProductId",
                principalTable: "ApsProducts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ApsJob_ApsSemiProducts_ApsSemiProductId",
                table: "ApsJob",
                column: "ApsSemiProductId",
                principalTable: "ApsSemiProducts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ApsJob_ProductInstances_ProductInstanceId",
                table: "ApsJob",
                column: "ProductInstanceId",
                principalTable: "ProductInstances",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ApsJob_SemiProductInstances_SemiProductInstanceId",
                table: "ApsJob",
                column: "SemiProductInstanceId",
                principalTable: "SemiProductInstances",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApsJob_ApsOrders_ApsOrderId",
                table: "ApsJob");

            migrationBuilder.DropForeignKey(
                name: "FK_ApsJob_ApsProcess_ApsAssemblyProcessId",
                table: "ApsJob");

            migrationBuilder.DropForeignKey(
                name: "FK_ApsJob_ApsProcess_ApsManufactureProcessId",
                table: "ApsJob");

            migrationBuilder.DropForeignKey(
                name: "FK_ApsJob_ApsProducts_ApsProductId",
                table: "ApsJob");

            migrationBuilder.DropForeignKey(
                name: "FK_ApsJob_ApsSemiProducts_ApsSemiProductId",
                table: "ApsJob");

            migrationBuilder.DropForeignKey(
                name: "FK_ApsJob_ProductInstances_ProductInstanceId",
                table: "ApsJob");

            migrationBuilder.DropForeignKey(
                name: "FK_ApsJob_SemiProductInstances_SemiProductInstanceId",
                table: "ApsJob");

            migrationBuilder.DropTable(
                name: "ApsJobApsResource");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ApsJob",
                table: "ApsJob");

            migrationBuilder.DropIndex(
                name: "IX_ApsJob_ApsAssemblyProcessId",
                table: "ApsJob");

            migrationBuilder.DropColumn(
                name: "ApsAssemblyProcessId",
                table: "ApsJob");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "ApsJob");

            migrationBuilder.DropColumn(
                name: "Workspace",
                table: "ApsJob");

            migrationBuilder.RenameTable(
                name: "ApsJob",
                newName: "ApsManufactureJobs");

            migrationBuilder.RenameIndex(
                name: "IX_ApsJob_SemiProductInstanceId",
                table: "ApsManufactureJobs",
                newName: "IX_ApsManufactureJobs_SemiProductInstanceId");

            migrationBuilder.RenameIndex(
                name: "IX_ApsJob_ProductInstanceId",
                table: "ApsManufactureJobs",
                newName: "IX_ApsManufactureJobs_ProductInstanceId");

            migrationBuilder.RenameIndex(
                name: "IX_ApsJob_ApsSemiProductId",
                table: "ApsManufactureJobs",
                newName: "IX_ApsManufactureJobs_ApsSemiProductId");

            migrationBuilder.RenameIndex(
                name: "IX_ApsJob_ApsProductId",
                table: "ApsManufactureJobs",
                newName: "IX_ApsManufactureJobs_ApsProductId");

            migrationBuilder.RenameIndex(
                name: "IX_ApsJob_ApsOrderId",
                table: "ApsManufactureJobs",
                newName: "IX_ApsManufactureJobs_ApsOrderId");

            migrationBuilder.RenameIndex(
                name: "IX_ApsJob_ApsManufactureProcessId",
                table: "ApsManufactureJobs",
                newName: "IX_ApsManufactureJobs_ApsManufactureProcessId");

            migrationBuilder.AddColumn<int>(
                name: "ApsAssemblyJobId",
                table: "ApsResources",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ApsManufactureJobId",
                table: "ApsResources",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "ApsManufactureJobs",
                type: "int",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ApsManufactureJobs",
                table: "ApsManufactureJobs",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "ApsAssemblyJobs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ApsAssemblyProcessId = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", nullable: true),
                    ApsOrderId = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", nullable: true),
                    ApsProductId = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", nullable: true),
                    Duration = table.Column<TimeSpan>(type: "time(6)", nullable: false),
                    End = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    ProductInstanceId = table.Column<Guid>(type: "char(36)", nullable: true),
                    Start = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApsAssemblyJobs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApsAssemblyJobs_ApsOrders_ApsOrderId",
                        column: x => x.ApsOrderId,
                        principalTable: "ApsOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ApsAssemblyJobs_ApsProcess_ApsAssemblyProcessId",
                        column: x => x.ApsAssemblyProcessId,
                        principalTable: "ApsProcess",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ApsAssemblyJobs_ApsProducts_ApsProductId",
                        column: x => x.ApsProductId,
                        principalTable: "ApsProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ApsAssemblyJobs_ProductInstances_ProductInstanceId",
                        column: x => x.ProductInstanceId,
                        principalTable: "ProductInstances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApsResources_ApsAssemblyJobId",
                table: "ApsResources",
                column: "ApsAssemblyJobId");

            migrationBuilder.CreateIndex(
                name: "IX_ApsResources_ApsManufactureJobId",
                table: "ApsResources",
                column: "ApsManufactureJobId");

            migrationBuilder.CreateIndex(
                name: "IX_ApsAssemblyJobs_ApsAssemblyProcessId",
                table: "ApsAssemblyJobs",
                column: "ApsAssemblyProcessId");

            migrationBuilder.CreateIndex(
                name: "IX_ApsAssemblyJobs_ApsOrderId",
                table: "ApsAssemblyJobs",
                column: "ApsOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_ApsAssemblyJobs_ApsProductId",
                table: "ApsAssemblyJobs",
                column: "ApsProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ApsAssemblyJobs_ProductInstanceId",
                table: "ApsAssemblyJobs",
                column: "ProductInstanceId");

            migrationBuilder.AddForeignKey(
                name: "FK_ApsManufactureJobs_ApsOrders_ApsOrderId",
                table: "ApsManufactureJobs",
                column: "ApsOrderId",
                principalTable: "ApsOrders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ApsManufactureJobs_ApsProcess_ApsManufactureProcessId",
                table: "ApsManufactureJobs",
                column: "ApsManufactureProcessId",
                principalTable: "ApsProcess",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ApsManufactureJobs_ApsProducts_ApsProductId",
                table: "ApsManufactureJobs",
                column: "ApsProductId",
                principalTable: "ApsProducts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ApsManufactureJobs_ApsSemiProducts_ApsSemiProductId",
                table: "ApsManufactureJobs",
                column: "ApsSemiProductId",
                principalTable: "ApsSemiProducts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ApsManufactureJobs_ProductInstances_ProductInstanceId",
                table: "ApsManufactureJobs",
                column: "ProductInstanceId",
                principalTable: "ProductInstances",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ApsManufactureJobs_SemiProductInstances_SemiProductInstanceId",
                table: "ApsManufactureJobs",
                column: "SemiProductInstanceId",
                principalTable: "SemiProductInstances",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ApsResources_ApsAssemblyJobs_ApsAssemblyJobId",
                table: "ApsResources",
                column: "ApsAssemblyJobId",
                principalTable: "ApsAssemblyJobs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ApsResources_ApsManufactureJobs_ApsManufactureJobId",
                table: "ApsResources",
                column: "ApsManufactureJobId",
                principalTable: "ApsManufactureJobs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
