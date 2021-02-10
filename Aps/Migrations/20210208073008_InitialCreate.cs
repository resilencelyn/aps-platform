using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Aps.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApsResources",
                columns: table => new
                {
                    ResourceId = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", nullable: false),
                    ResourceType = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<int>(type: "int", nullable: true),
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
                name: "ResourceClass",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ResourceClassName = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResourceClass", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ApsProcess",
                columns: table => new
                {
                    PartId = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", nullable: false),
                    PartName = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    ProductionMode = table.Column<int>(type: "int", nullable: false),
                    ProductionTime = table.Column<TimeSpan>(type: "time(6)", nullable: false),
                    MinimumProductionQuantity = table.Column<int>(type: "int", nullable: true),
                    MaximumProductionQuantity = table.Column<int>(type: "int", nullable: true),
                    Workspace = table.Column<int>(type: "int", nullable: false),
                    Discriminator = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: false),
                    OutputFinishedProductId = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    PrevPartId = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    ApsSemiProductSemiProductId = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApsProcess", x => x.PartId);
                    table.ForeignKey(
                        name: "FK_ApsProcess_ApsSemiProducts_ApsSemiProductSemiProductId",
                        column: x => x.ApsSemiProductSemiProductId,
                        principalTable: "ApsSemiProducts",
                        principalColumn: "SemiProductId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ResourceClassWithResource",
                columns: table => new
                {
                    ApsResourceId = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", nullable: false),
                    ResourceClassId = table.Column<int>(type: "int", nullable: false),
                    IsBasic = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResourceClassWithResource", x => new { x.ResourceClassId, x.ApsResourceId });
                    table.ForeignKey(
                        name: "FK_ResourceClassWithResource_ApsResources_ApsResourceId",
                        column: x => x.ApsResourceId,
                        principalTable: "ApsResources",
                        principalColumn: "ResourceId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ResourceClassWithResource_ResourceClass_ResourceClassId",
                        column: x => x.ResourceClassId,
                        principalTable: "ResourceClass",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ApsAssemblyProcessSemiProduct",
                columns: table => new
                {
                    ApsAssemblyProcessId = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", nullable: false),
                    ApsSemiProductId = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", nullable: false),
                    Amount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApsAssemblyProcessSemiProduct", x => new { x.ApsAssemblyProcessId, x.ApsSemiProductId });
                    table.ForeignKey(
                        name: "FK_ApsAssemblyProcessSemiProduct_ApsProcess_ApsAssemblyProcessId",
                        column: x => x.ApsAssemblyProcessId,
                        principalTable: "ApsProcess",
                        principalColumn: "PartId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApsAssemblyProcessSemiProduct_ApsSemiProducts_ApsSemiProduct~",
                        column: x => x.ApsSemiProductId,
                        principalTable: "ApsSemiProducts",
                        principalColumn: "SemiProductId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ApsProcessResource",
                columns: table => new
                {
                    ApsProcessId = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", nullable: false),
                    ResourceClassId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApsProcessResource", x => new { x.ApsProcessId, x.ResourceClassId });
                    table.ForeignKey(
                        name: "FK_ApsProcessResource_ApsProcess_ApsProcessId",
                        column: x => x.ApsProcessId,
                        principalTable: "ApsProcess",
                        principalColumn: "PartId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApsProcessResource_ResourceClass_ResourceClassId",
                        column: x => x.ResourceClassId,
                        principalTable: "ResourceClass",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ApsProducts",
                columns: table => new
                {
                    ProductId = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", nullable: false),
                    ApsAssemblyProcessId = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApsProducts", x => x.ProductId);
                    table.ForeignKey(
                        name: "FK_ApsProducts_ApsProcess_ApsAssemblyProcessId",
                        column: x => x.ApsAssemblyProcessId,
                        principalTable: "ApsProcess",
                        principalColumn: "PartId",
                        onDelete: ReferentialAction.Restrict);
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
                        name: "FK_ApsOrders_ApsProducts_ProductId",
                        column: x => x.ProductId,
                        principalTable: "ApsProducts",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ApsProductSemiProduct",
                columns: table => new
                {
                    ApsProductId = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", nullable: false),
                    ApsSemiProductId = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", nullable: false),
                    Amount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApsProductSemiProduct", x => new { x.ApsSemiProductId, x.ApsProductId });
                    table.ForeignKey(
                        name: "FK_ApsProductSemiProduct_ApsProducts_ApsProductId",
                        column: x => x.ApsProductId,
                        principalTable: "ApsProducts",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApsProductSemiProduct_ApsSemiProducts_ApsSemiProductId",
                        column: x => x.ApsSemiProductId,
                        principalTable: "ApsSemiProducts",
                        principalColumn: "SemiProductId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "ApsProcess",
                columns: new[] { "PartId", "Discriminator", "MaximumProductionQuantity", "MinimumProductionQuantity", "OutputFinishedProductId", "PartName", "ProductionMode", "ProductionTime", "Workspace" },
                values: new object[] { "process_end_A", "ApsAssemblyProcess", 1, 1, null, "process_end_A", 2, new TimeSpan(0, 0, 4, 0, 0), 2 });

            migrationBuilder.InsertData(
                table: "ApsProcess",
                columns: new[] { "PartId", "ApsSemiProductSemiProductId", "Discriminator", "MaximumProductionQuantity", "MinimumProductionQuantity", "PartName", "PrevPartId", "ProductionMode", "ProductionTime", "Workspace" },
                values: new object[] { "process_1_a", null, "ApsManufactureProcess", 1, 1, "process_1_a", null, 2, new TimeSpan(0, 0, 0, 1, 0), 1 });

            migrationBuilder.InsertData(
                table: "ApsProducts",
                columns: new[] { "ProductId", "ApsAssemblyProcessId" },
                values: new object[,]
                {
                    { "product_1", null },
                    { "product_2", null },
                    { "product_3", null },
                    { "product_4", null },
                    { "product_5", null },
                    { "product_6", null }
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
                table: "ApsAssemblyProcessSemiProduct",
                columns: new[] { "ApsAssemblyProcessId", "ApsSemiProductId", "Amount" },
                values: new object[,]
                {
                    { "process_end_A", "product_semi_d", 1 },
                    { "process_end_A", "product_semi_o", 1 },
                    { "process_end_A", "product_semi_a", 1 },
                    { "process_end_A", "product_semi_j", 1 },
                    { "process_end_A", "product_semi_r", 1 },
                    { "process_end_A", "product_semi_f", 1 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApsAssemblyProcessSemiProduct_ApsSemiProductId",
                table: "ApsAssemblyProcessSemiProduct",
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
                name: "IX_ApsProcessResource_ResourceClassId",
                table: "ApsProcessResource",
                column: "ResourceClassId");

            migrationBuilder.CreateIndex(
                name: "IX_ApsProducts_ApsAssemblyProcessId",
                table: "ApsProducts",
                column: "ApsAssemblyProcessId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ApsProductSemiProduct_ApsProductId",
                table: "ApsProductSemiProduct",
                column: "ApsProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ResourceClassWithResource_ApsResourceId",
                table: "ResourceClassWithResource",
                column: "ApsResourceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApsAssemblyProcessSemiProduct");

            migrationBuilder.DropTable(
                name: "ApsOrders");

            migrationBuilder.DropTable(
                name: "ApsProcessResource");

            migrationBuilder.DropTable(
                name: "ApsProductSemiProduct");

            migrationBuilder.DropTable(
                name: "ResourceClassWithResource");

            migrationBuilder.DropTable(
                name: "ApsProducts");

            migrationBuilder.DropTable(
                name: "ApsResources");

            migrationBuilder.DropTable(
                name: "ResourceClass");

            migrationBuilder.DropTable(
                name: "ApsProcess");

            migrationBuilder.DropTable(
                name: "ApsSemiProducts");
        }
    }
}
