using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Aps.Migrations
{
    public partial class Jobs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApsProcessResource_ResourceClass_ResourceClassId",
                table: "ApsProcessResource");

            migrationBuilder.DropForeignKey(
                name: "FK_ResourceClassWithResource_ResourceClass_ResourceClassId",
                table: "ResourceClassWithResource");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ResourceClass",
                table: "ResourceClass");

            migrationBuilder.RenameTable(
                name: "ResourceClass",
                newName: "ResourceClasses");

            migrationBuilder.AddColumn<int>(
                name: "ApsAssemblyJobJobId",
                table: "ApsResources",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ApsManufactureJobJobId",
                table: "ApsResources",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ResourceClasses",
                table: "ResourceClasses",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "ProductInstances",
                columns: table => new
                {
                    ProductId = table.Column<Guid>(type: "char(36)", nullable: false),
                    ApsProductProductId = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", nullable: true),
                    OrderedByOrderId = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductInstances", x => x.ProductId);
                    table.ForeignKey(
                        name: "FK_ProductInstances_ApsOrders_OrderedByOrderId",
                        column: x => x.OrderedByOrderId,
                        principalTable: "ApsOrders",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductInstances_ApsProducts_ApsProductProductId",
                        column: x => x.ApsProductProductId,
                        principalTable: "ApsProducts",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ApsAssemblyJobs",
                columns: table => new
                {
                    JobId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ApsAssemblyProcessPartId = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", nullable: true),
                    ApsOrderOrderId = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", nullable: true),
                    ApsProductProductId = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", nullable: true),
                    ProductInstanceProductId = table.Column<Guid>(type: "char(36)", nullable: true),
                    Start = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    End = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Duration = table.Column<TimeSpan>(type: "time(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApsAssemblyJobs", x => x.JobId);
                    table.ForeignKey(
                        name: "FK_ApsAssemblyJobs_ApsOrders_ApsOrderOrderId",
                        column: x => x.ApsOrderOrderId,
                        principalTable: "ApsOrders",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ApsAssemblyJobs_ApsProcess_ApsAssemblyProcessPartId",
                        column: x => x.ApsAssemblyProcessPartId,
                        principalTable: "ApsProcess",
                        principalColumn: "PartId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ApsAssemblyJobs_ApsProducts_ApsProductProductId",
                        column: x => x.ApsProductProductId,
                        principalTable: "ApsProducts",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ApsAssemblyJobs_ProductInstances_ProductInstanceProductId",
                        column: x => x.ProductInstanceProductId,
                        principalTable: "ProductInstances",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SemiProductInstances",
                columns: table => new
                {
                    SemiProductId = table.Column<Guid>(type: "char(36)", nullable: false),
                    ApsSemiProductSemiProductId = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", nullable: true),
                    ProductAssemblyToProductId = table.Column<Guid>(type: "char(36)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SemiProductInstances", x => x.SemiProductId);
                    table.ForeignKey(
                        name: "FK_SemiProductInstances_ApsSemiProducts_ApsSemiProductSemiProdu~",
                        column: x => x.ApsSemiProductSemiProductId,
                        principalTable: "ApsSemiProducts",
                        principalColumn: "SemiProductId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SemiProductInstances_ProductInstances_ProductAssemblyToProdu~",
                        column: x => x.ProductAssemblyToProductId,
                        principalTable: "ProductInstances",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ApsManufactureJobs",
                columns: table => new
                {
                    JobId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ApsSemiProductSemiProductId = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", nullable: true),
                    SemiProductInstanceSemiProductId = table.Column<Guid>(type: "char(36)", nullable: true),
                    ApsManufactureProcessPartId = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", nullable: true),
                    ApsOrderOrderId = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", nullable: true),
                    ApsProductProductId = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", nullable: true),
                    ProductInstanceProductId = table.Column<Guid>(type: "char(36)", nullable: true),
                    Start = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    End = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Duration = table.Column<TimeSpan>(type: "time(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApsManufactureJobs", x => x.JobId);
                    table.ForeignKey(
                        name: "FK_ApsManufactureJobs_ApsOrders_ApsOrderOrderId",
                        column: x => x.ApsOrderOrderId,
                        principalTable: "ApsOrders",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ApsManufactureJobs_ApsProcess_ApsManufactureProcessPartId",
                        column: x => x.ApsManufactureProcessPartId,
                        principalTable: "ApsProcess",
                        principalColumn: "PartId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ApsManufactureJobs_ApsProducts_ApsProductProductId",
                        column: x => x.ApsProductProductId,
                        principalTable: "ApsProducts",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ApsManufactureJobs_ApsSemiProducts_ApsSemiProductSemiProduct~",
                        column: x => x.ApsSemiProductSemiProductId,
                        principalTable: "ApsSemiProducts",
                        principalColumn: "SemiProductId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ApsManufactureJobs_ProductInstances_ProductInstanceProductId",
                        column: x => x.ProductInstanceProductId,
                        principalTable: "ProductInstances",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ApsManufactureJobs_SemiProductInstances_SemiProductInstanceS~",
                        column: x => x.SemiProductInstanceSemiProductId,
                        principalTable: "SemiProductInstances",
                        principalColumn: "SemiProductId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApsResources_ApsAssemblyJobJobId",
                table: "ApsResources",
                column: "ApsAssemblyJobJobId");

            migrationBuilder.CreateIndex(
                name: "IX_ApsResources_ApsManufactureJobJobId",
                table: "ApsResources",
                column: "ApsManufactureJobJobId");

            migrationBuilder.CreateIndex(
                name: "IX_ApsAssemblyJobs_ApsAssemblyProcessPartId",
                table: "ApsAssemblyJobs",
                column: "ApsAssemblyProcessPartId");

            migrationBuilder.CreateIndex(
                name: "IX_ApsAssemblyJobs_ApsOrderOrderId",
                table: "ApsAssemblyJobs",
                column: "ApsOrderOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_ApsAssemblyJobs_ApsProductProductId",
                table: "ApsAssemblyJobs",
                column: "ApsProductProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ApsAssemblyJobs_ProductInstanceProductId",
                table: "ApsAssemblyJobs",
                column: "ProductInstanceProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ApsManufactureJobs_ApsManufactureProcessPartId",
                table: "ApsManufactureJobs",
                column: "ApsManufactureProcessPartId");

            migrationBuilder.CreateIndex(
                name: "IX_ApsManufactureJobs_ApsOrderOrderId",
                table: "ApsManufactureJobs",
                column: "ApsOrderOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_ApsManufactureJobs_ApsProductProductId",
                table: "ApsManufactureJobs",
                column: "ApsProductProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ApsManufactureJobs_ApsSemiProductSemiProductId",
                table: "ApsManufactureJobs",
                column: "ApsSemiProductSemiProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ApsManufactureJobs_ProductInstanceProductId",
                table: "ApsManufactureJobs",
                column: "ProductInstanceProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ApsManufactureJobs_SemiProductInstanceSemiProductId",
                table: "ApsManufactureJobs",
                column: "SemiProductInstanceSemiProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductInstances_ApsProductProductId",
                table: "ProductInstances",
                column: "ApsProductProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductInstances_OrderedByOrderId",
                table: "ProductInstances",
                column: "OrderedByOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_SemiProductInstances_ApsSemiProductSemiProductId",
                table: "SemiProductInstances",
                column: "ApsSemiProductSemiProductId");

            migrationBuilder.CreateIndex(
                name: "IX_SemiProductInstances_ProductAssemblyToProductId",
                table: "SemiProductInstances",
                column: "ProductAssemblyToProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_ApsProcessResource_ResourceClasses_ResourceClassId",
                table: "ApsProcessResource",
                column: "ResourceClassId",
                principalTable: "ResourceClasses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ApsResources_ApsAssemblyJobs_ApsAssemblyJobJobId",
                table: "ApsResources",
                column: "ApsAssemblyJobJobId",
                principalTable: "ApsAssemblyJobs",
                principalColumn: "JobId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ApsResources_ApsManufactureJobs_ApsManufactureJobJobId",
                table: "ApsResources",
                column: "ApsManufactureJobJobId",
                principalTable: "ApsManufactureJobs",
                principalColumn: "JobId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ResourceClassWithResource_ResourceClasses_ResourceClassId",
                table: "ResourceClassWithResource",
                column: "ResourceClassId",
                principalTable: "ResourceClasses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApsProcessResource_ResourceClasses_ResourceClassId",
                table: "ApsProcessResource");

            migrationBuilder.DropForeignKey(
                name: "FK_ApsResources_ApsAssemblyJobs_ApsAssemblyJobJobId",
                table: "ApsResources");

            migrationBuilder.DropForeignKey(
                name: "FK_ApsResources_ApsManufactureJobs_ApsManufactureJobJobId",
                table: "ApsResources");

            migrationBuilder.DropForeignKey(
                name: "FK_ResourceClassWithResource_ResourceClasses_ResourceClassId",
                table: "ResourceClassWithResource");

            migrationBuilder.DropTable(
                name: "ApsAssemblyJobs");

            migrationBuilder.DropTable(
                name: "ApsManufactureJobs");

            migrationBuilder.DropTable(
                name: "SemiProductInstances");

            migrationBuilder.DropTable(
                name: "ProductInstances");

            migrationBuilder.DropIndex(
                name: "IX_ApsResources_ApsAssemblyJobJobId",
                table: "ApsResources");

            migrationBuilder.DropIndex(
                name: "IX_ApsResources_ApsManufactureJobJobId",
                table: "ApsResources");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ResourceClasses",
                table: "ResourceClasses");

            migrationBuilder.DropColumn(
                name: "ApsAssemblyJobJobId",
                table: "ApsResources");

            migrationBuilder.DropColumn(
                name: "ApsManufactureJobJobId",
                table: "ApsResources");

            migrationBuilder.RenameTable(
                name: "ResourceClasses",
                newName: "ResourceClass");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ResourceClass",
                table: "ResourceClass",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ApsProcessResource_ResourceClass_ResourceClassId",
                table: "ApsProcessResource",
                column: "ResourceClassId",
                principalTable: "ResourceClass",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ResourceClassWithResource_ResourceClass_ResourceClassId",
                table: "ResourceClassWithResource",
                column: "ResourceClassId",
                principalTable: "ResourceClass",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
