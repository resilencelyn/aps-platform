using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace 高级计划与排产.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApsProduct",
                columns: table => new
                {
                    ProductId = table.Column<string>(type: "varchar(255)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApsProduct", x => x.ProductId);
                });

            migrationBuilder.CreateTable(
                name: "ApsAssemblyProcesses",
                columns: table => new
                {
                    PartId = table.Column<string>(type: "varchar(255)", nullable: false),
                    OutputFinishedProductProductId = table.Column<string>(type: "varchar(255)", nullable: true),
                    PartName = table.Column<string>(type: "longtext", nullable: true),
                    ProductionMode = table.Column<int>(type: "int", nullable: false),
                    ProductionTime = table.Column<uint>(type: "int unsigned", nullable: false),
                    MinimumProductionQuantity = table.Column<uint>(type: "int unsigned", nullable: true),
                    MaximumProductionQuantity = table.Column<uint>(type: "int unsigned", nullable: true),
                    Workspace = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApsAssemblyProcesses", x => x.PartId);
                    table.ForeignKey(
                        name: "FK_ApsAssemblyProcesses_ApsProduct_OutputFinishedProductProduct~",
                        column: x => x.OutputFinishedProductProductId,
                        principalTable: "ApsProduct",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ApsOrders",
                columns: table => new
                {
                    OrderId = table.Column<string>(type: "varchar(255)", nullable: false),
                    Description = table.Column<string>(type: "longtext", nullable: true),
                    EarliestStartTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    LatestEndTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ProductId = table.Column<string>(type: "varchar(255)", nullable: true),
                    Amount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApsOrders", x => x.OrderId);
                    table.ForeignKey(
                        name: "FK_ApsOrders_ApsProduct_ProductId",
                        column: x => x.ProductId,
                        principalTable: "ApsProduct",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ApsSemiProducts",
                columns: table => new
                {
                    SemiProductId = table.Column<string>(type: "varchar(255)", nullable: false),
                    ApsAssemblyProcessPartId = table.Column<string>(type: "varchar(255)", nullable: true),
                    ApsProductProductId = table.Column<string>(type: "varchar(255)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApsSemiProducts", x => x.SemiProductId);
                    table.ForeignKey(
                        name: "FK_ApsSemiProducts_ApsAssemblyProcesses_ApsAssemblyProcessPartId",
                        column: x => x.ApsAssemblyProcessPartId,
                        principalTable: "ApsAssemblyProcesses",
                        principalColumn: "PartId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ApsSemiProducts_ApsProduct_ApsProductProductId",
                        column: x => x.ApsProductProductId,
                        principalTable: "ApsProduct",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ApsProcesses",
                columns: table => new
                {
                    PartId = table.Column<string>(type: "varchar(255)", nullable: false),
                    PrevPartId = table.Column<string>(type: "longtext", nullable: true),
                    PartName = table.Column<string>(type: "longtext", nullable: true),
                    ProductionMode = table.Column<int>(type: "int", nullable: false),
                    ProductionTime = table.Column<uint>(type: "int unsigned", nullable: false),
                    MinimumProductionQuantity = table.Column<uint>(type: "int unsigned", nullable: true),
                    MaximumProductionQuantity = table.Column<uint>(type: "int unsigned", nullable: true),
                    Workspace = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApsProcesses", x => x.PartId);
                    table.ForeignKey(
                        name: "FK_ApsProcesses_ApsSemiProducts_PartId",
                        column: x => x.PartId,
                        principalTable: "ApsSemiProducts",
                        principalColumn: "SemiProductId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ApsResources",
                columns: table => new
                {
                    ResourceId = table.Column<string>(type: "varchar(255)", nullable: false),
                    ResourceType = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<int>(type: "int", nullable: true),
                    ResourceAttributes = table.Column<string>(type: "longtext", nullable: false),
                    BasicAttributes = table.Column<string>(type: "longtext", nullable: false),
                    Workspace = table.Column<int>(type: "int", nullable: false),
                    ApsAssemblyProcessPartId = table.Column<string>(type: "varchar(255)", nullable: true),
                    ApsManufactureProcessPartId = table.Column<string>(type: "varchar(255)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApsResources", x => x.ResourceId);
                    table.ForeignKey(
                        name: "FK_ApsResources_ApsAssemblyProcesses_ApsAssemblyProcessPartId",
                        column: x => x.ApsAssemblyProcessPartId,
                        principalTable: "ApsAssemblyProcesses",
                        principalColumn: "PartId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ApsResources_ApsProcesses_ApsManufactureProcessPartId",
                        column: x => x.ApsManufactureProcessPartId,
                        principalTable: "ApsProcesses",
                        principalColumn: "PartId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApsAssemblyProcesses_OutputFinishedProductProductId",
                table: "ApsAssemblyProcesses",
                column: "OutputFinishedProductProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ApsOrders_ProductId",
                table: "ApsOrders",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ApsResources_ApsAssemblyProcessPartId",
                table: "ApsResources",
                column: "ApsAssemblyProcessPartId");

            migrationBuilder.CreateIndex(
                name: "IX_ApsResources_ApsManufactureProcessPartId",
                table: "ApsResources",
                column: "ApsManufactureProcessPartId");

            migrationBuilder.CreateIndex(
                name: "IX_ApsSemiProducts_ApsAssemblyProcessPartId",
                table: "ApsSemiProducts",
                column: "ApsAssemblyProcessPartId");

            migrationBuilder.CreateIndex(
                name: "IX_ApsSemiProducts_ApsProductProductId",
                table: "ApsSemiProducts",
                column: "ApsProductProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApsOrders");

            migrationBuilder.DropTable(
                name: "ApsResources");

            migrationBuilder.DropTable(
                name: "ApsProcesses");

            migrationBuilder.DropTable(
                name: "ApsSemiProducts");

            migrationBuilder.DropTable(
                name: "ApsAssemblyProcesses");

            migrationBuilder.DropTable(
                name: "ApsProduct");
        }
    }
}
