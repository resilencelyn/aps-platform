using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Aps.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApsProduct",
                columns: table => new
                {
                    ProductId = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApsProduct", x => x.ProductId);
                });

            migrationBuilder.CreateTable(
                name: "ApsResources",
                columns: table => new
                {
                    ResourceId = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", nullable: false),
                    ResourceType = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<int>(type: "int", nullable: true),
                    ResourceAttributes = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: false),
                    BasicAttributes = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: false),
                    Workspace = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApsResources", x => x.ResourceId);
                });

            migrationBuilder.CreateTable(
                name: "ApsSemiProducts",
                columns: table => new
                {
                    SemiProductId = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApsSemiProducts", x => x.SemiProductId);
                });

            migrationBuilder.CreateTable(
                name: "ApsOrders",
                columns: table => new
                {
                    OrderId = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", nullable: false),
                    Description = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    EarliestStartTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    LatestEndTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ProductId = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", nullable: false),
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
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ApsProcess",
                columns: table => new
                {
                    PartId = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", nullable: false),
                    PartName = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    ProductionMode = table.Column<int>(type: "int", nullable: false),
                    ProductionTime = table.Column<uint>(type: "int unsigned", nullable: false),
                    MinimumProductionQuantity = table.Column<uint>(type: "int unsigned", nullable: true),
                    MaximumProductionQuantity = table.Column<uint>(type: "int unsigned", nullable: true),
                    Workspace = table.Column<int>(type: "int", nullable: false),
                    Discriminator = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: false),
                    OutputFinishedProductProductId = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", nullable: true),
                    PrevPartId = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    ApsSemiProductSemiProductId = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApsProcess", x => x.PartId);
                    table.ForeignKey(
                        name: "FK_ApsProcess_ApsProduct_OutputFinishedProductProductId",
                        column: x => x.OutputFinishedProductProductId,
                        principalTable: "ApsProduct",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ApsProcess_ApsSemiProducts_ApsSemiProductSemiProductId",
                        column: x => x.ApsSemiProductSemiProductId,
                        principalTable: "ApsSemiProducts",
                        principalColumn: "SemiProductId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ApsProductApsSemiProduct",
                columns: table => new
                {
                    ApsAssemblyProductsProductId = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", nullable: false),
                    AssembleBySemiProductsSemiProductId = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApsProductApsSemiProduct", x => new { x.ApsAssemblyProductsProductId, x.AssembleBySemiProductsSemiProductId });
                    table.ForeignKey(
                        name: "FK_ApsProductApsSemiProduct_ApsProduct_ApsAssemblyProductsProdu~",
                        column: x => x.ApsAssemblyProductsProductId,
                        principalTable: "ApsProduct",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApsProductApsSemiProduct_ApsSemiProducts_AssembleBySemiProdu~",
                        column: x => x.AssembleBySemiProductsSemiProductId,
                        principalTable: "ApsSemiProducts",
                        principalColumn: "SemiProductId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ApsAssemblyProcessSemiProducts",
                columns: table => new
                {
                    ApsAssemblyProcessId = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", nullable: false),
                    ApsSemiProductId = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", nullable: false),
                    Amount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApsAssemblyProcessSemiProducts", x => new { x.ApsAssemblyProcessId, x.ApsSemiProductId });
                    table.ForeignKey(
                        name: "FK_ApsAssemblyProcessSemiProducts_ApsProcess_ApsAssemblyProcess~",
                        column: x => x.ApsAssemblyProcessId,
                        principalTable: "ApsProcess",
                        principalColumn: "PartId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApsAssemblyProcessSemiProducts_ApsSemiProducts_ApsSemiProduc~",
                        column: x => x.ApsSemiProductId,
                        principalTable: "ApsSemiProducts",
                        principalColumn: "SemiProductId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ApsProcessResources",
                columns: table => new
                {
                    ApsProcessId = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", nullable: false),
                    ResourceAttribute = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", nullable: false),
                    Amount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApsProcessResources", x => new { x.ApsProcessId, x.ResourceAttribute });
                    table.ForeignKey(
                        name: "FK_ApsProcessResources_ApsProcess_ApsProcessId",
                        column: x => x.ApsProcessId,
                        principalTable: "ApsProcess",
                        principalColumn: "PartId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "ApsProcess",
                columns: new[] { "PartId", "Discriminator", "MaximumProductionQuantity", "MinimumProductionQuantity", "OutputFinishedProductProductId", "PartName", "ProductionMode", "ProductionTime", "Workspace" },
                values: new object[] { "process_end_A", "ApsAssemblyProcess", 1u, 1u, null, "process_end_A", 2, 4u, 2 });

            migrationBuilder.InsertData(
                table: "ApsProcess",
                columns: new[] { "PartId", "ApsSemiProductSemiProductId", "Discriminator", "MaximumProductionQuantity", "MinimumProductionQuantity", "PartName", "PrevPartId", "ProductionMode", "ProductionTime", "Workspace" },
                values: new object[] { "process_1_a", null, "ApsManufactureProcess", 1u, 1u, "process_1_a", null, 2, 1u, 1 });

            migrationBuilder.InsertData(
                table: "ApsProduct",
                column: "ProductId",
                values: new object[]
                {
                    "product_1",
                    "product_2",
                    "product_3",
                    "product_4",
                    "product_5",
                    "product_6"
                });

            migrationBuilder.InsertData(
                table: "ApsSemiProducts",
                column: "SemiProductId",
                values: new object[]
                {
                    "product_semi_l",
                    "product_semi_t",
                    "product_semi_c",
                    " product_semi_d",
                    "product_semi_g",
                    "product_semi_p",
                    " product_semi_a",
                    " product_semi_f",
                    "product_semi_f",
                    "product_semi_e",
                    "product_semi_s",
                    "product_semi_q",
                    "product_semi_r",
                    "product_semi_j",
                    "product_semi_a",
                    "product_semi_o",
                    "product_semi_d",
                    "product_semi_n",
                    ""
                });

            migrationBuilder.InsertData(
                table: "ApsAssemblyProcessSemiProducts",
                columns: new[] { "ApsAssemblyProcessId", "ApsSemiProductId", "Amount" },
                values: new object[,]
                {
                    { "process_end_A", "product_semi_f", 1 },
                    { "process_end_A", "product_semi_j", 1 },
                    { "process_end_A", "product_semi_a", 1 },
                    { "process_end_A", "product_semi_o", 1 },
                    { "process_end_A", "product_semi_d", 1 },
                    { "process_end_A", "product_semi_r", 1 }
                });

            migrationBuilder.InsertData(
                table: "ApsProcessResources",
                columns: new[] { "ApsProcessId", "ResourceAttribute", "Amount" },
                values: new object[,]
                {
                    { "process_1_a", "高级设备", 2 },
                    { "process_1_a", "设备", 3 },
                    { "process_1_a", "高级人员", 1 },
                    { "process_1_a", "人员", 1 },
                    { "process_1_a", "机床", 3 },
                    { "process_end_A", "高级设备", 2 },
                    { "process_end_A", "设备", 3 },
                    { "process_end_A", "高级人员", 1 },
                    { "process_end_A", "人员", 1 },
                    { "process_end_A", "高级机床", 2 },
                    { "process_1_a", "高级机床", 2 },
                    { "process_end_A", "机床", 3 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApsAssemblyProcessSemiProducts_ApsSemiProductId",
                table: "ApsAssemblyProcessSemiProducts",
                column: "ApsSemiProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ApsOrders_ProductId",
                table: "ApsOrders",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ApsProcess_ApsSemiProductSemiProductId",
                table: "ApsProcess",
                column: "ApsSemiProductSemiProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ApsProcess_OutputFinishedProductProductId",
                table: "ApsProcess",
                column: "OutputFinishedProductProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ApsProductApsSemiProduct_AssembleBySemiProductsSemiProductId",
                table: "ApsProductApsSemiProduct",
                column: "AssembleBySemiProductsSemiProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApsAssemblyProcessSemiProducts");

            migrationBuilder.DropTable(
                name: "ApsOrders");

            migrationBuilder.DropTable(
                name: "ApsProcessResources");

            migrationBuilder.DropTable(
                name: "ApsProductApsSemiProduct");

            migrationBuilder.DropTable(
                name: "ApsResources");

            migrationBuilder.DropTable(
                name: "ApsProcess");

            migrationBuilder.DropTable(
                name: "ApsProduct");

            migrationBuilder.DropTable(
                name: "ApsSemiProducts");
        }
    }
}
