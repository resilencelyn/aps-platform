using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Aps.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApsSemiProducts",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApsSemiProducts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ResourceClasses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ResourceClassName = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResourceClasses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ApsProcess",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", nullable: false),
                    PartName = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    ProductionMode = table.Column<int>(type: "int", nullable: false),
                    ProductionTime = table.Column<TimeSpan>(type: "time(6)", nullable: false),
                    MinimumProductionQuantity = table.Column<int>(type: "int", nullable: true),
                    MaximumProductionQuantity = table.Column<int>(type: "int", nullable: true),
                    Workspace = table.Column<int>(type: "int", nullable: false),
                    Discriminator = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: false),
                    OutputFinishedProductId = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    PrevPartId = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    ApsSemiProductId = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApsProcess", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApsProcess_ApsSemiProducts_ApsSemiProductId",
                        column: x => x.ApsSemiProductId,
                        principalTable: "ApsSemiProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApsAssemblyProcessSemiProduct_ApsSemiProducts_ApsSemiProduct~",
                        column: x => x.ApsSemiProductId,
                        principalTable: "ApsSemiProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ApsProcessResource",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", nullable: false),
                    ResourceClassId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApsProcessResource", x => new { x.Id, x.ResourceClassId });
                    table.ForeignKey(
                        name: "FK_ApsProcessResource_ApsProcess_Id",
                        column: x => x.Id,
                        principalTable: "ApsProcess",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApsProcessResource_ResourceClasses_ResourceClassId",
                        column: x => x.ResourceClassId,
                        principalTable: "ResourceClasses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ApsProducts",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", nullable: false),
                    ApsAssemblyProcessId = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApsProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApsProducts_ApsProcess_ApsAssemblyProcessId",
                        column: x => x.ApsAssemblyProcessId,
                        principalTable: "ApsProcess",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ApsOrders",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", nullable: false),
                    Description = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    EarliestStartTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    LatestEndTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ProductId = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", nullable: false),
                    Amount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApsOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApsOrders_ApsProducts_ProductId",
                        column: x => x.ProductId,
                        principalTable: "ApsProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ApsProductSemiProduct",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", nullable: false),
                    ApsSemiProductId = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", nullable: false),
                    ApsProductId = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", nullable: true),
                    Amount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApsProductSemiProduct", x => new { x.ApsSemiProductId, x.Id });
                    table.ForeignKey(
                        name: "FK_ApsProductSemiProduct_ApsProducts_ApsProductId",
                        column: x => x.ApsProductId,
                        principalTable: "ApsProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ApsProductSemiProduct_ApsSemiProducts_ApsSemiProductId",
                        column: x => x.ApsSemiProductId,
                        principalTable: "ApsSemiProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductInstances",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    ApsProductId = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", nullable: true),
                    OrderedById = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductInstances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductInstances_ApsOrders_OrderedById",
                        column: x => x.OrderedById,
                        principalTable: "ApsOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductInstances_ApsProducts_ApsProductId",
                        column: x => x.ApsProductId,
                        principalTable: "ApsProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ApsAssemblyJobs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ApsAssemblyProcessId = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", nullable: true),
                    ApsOrderId = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", nullable: true),
                    ApsProductId = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", nullable: true),
                    ProductInstanceId = table.Column<Guid>(type: "char(36)", nullable: true),
                    Start = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    End = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Duration = table.Column<TimeSpan>(type: "time(6)", nullable: false)
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

            migrationBuilder.CreateTable(
                name: "SemiProductInstances",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    ApsSemiProductId = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", nullable: true),
                    ProductAssemblyToId = table.Column<Guid>(type: "char(36)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SemiProductInstances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SemiProductInstances_ApsSemiProducts_ApsSemiProductId",
                        column: x => x.ApsSemiProductId,
                        principalTable: "ApsSemiProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SemiProductInstances_ProductInstances_ProductAssemblyToId",
                        column: x => x.ProductAssemblyToId,
                        principalTable: "ProductInstances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ApsManufactureJobs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ApsSemiProductId = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", nullable: true),
                    SemiProductInstanceId = table.Column<Guid>(type: "char(36)", nullable: true),
                    ApsManufactureProcessId = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", nullable: true),
                    ApsOrderId = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", nullable: true),
                    ApsProductId = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", nullable: true),
                    ProductInstanceId = table.Column<Guid>(type: "char(36)", nullable: true),
                    Start = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    End = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Duration = table.Column<TimeSpan>(type: "time(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApsManufactureJobs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApsManufactureJobs_ApsOrders_ApsOrderId",
                        column: x => x.ApsOrderId,
                        principalTable: "ApsOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ApsManufactureJobs_ApsProcess_ApsManufactureProcessId",
                        column: x => x.ApsManufactureProcessId,
                        principalTable: "ApsProcess",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ApsManufactureJobs_ApsProducts_ApsProductId",
                        column: x => x.ApsProductId,
                        principalTable: "ApsProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ApsManufactureJobs_ApsSemiProducts_ApsSemiProductId",
                        column: x => x.ApsSemiProductId,
                        principalTable: "ApsSemiProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ApsManufactureJobs_ProductInstances_ProductInstanceId",
                        column: x => x.ProductInstanceId,
                        principalTable: "ProductInstances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ApsManufactureJobs_SemiProductInstances_SemiProductInstanceId",
                        column: x => x.SemiProductInstanceId,
                        principalTable: "SemiProductInstances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ApsResources",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<int>(type: "int", nullable: true),
                    Workspace = table.Column<int>(type: "int", nullable: false),
                    ApsAssemblyJobId = table.Column<int>(type: "int", nullable: true),
                    ApsManufactureJobId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApsResources", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApsResources_ApsAssemblyJobs_ApsAssemblyJobId",
                        column: x => x.ApsAssemblyJobId,
                        principalTable: "ApsAssemblyJobs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ApsResources_ApsManufactureJobs_ApsManufactureJobId",
                        column: x => x.ApsManufactureJobId,
                        principalTable: "ApsManufactureJobs",
                        principalColumn: "Id",
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
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ResourceClassWithResource_ResourceClasses_ResourceClassId",
                        column: x => x.ResourceClassId,
                        principalTable: "ResourceClasses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "ApsProcess",
                columns: new[] { "Id", "Discriminator", "MaximumProductionQuantity", "MinimumProductionQuantity", "OutputFinishedProductId", "PartName", "ProductionMode", "ProductionTime", "Workspace" },
                values: new object[] { "process_end_A", "ApsAssemblyProcess", 1, 1, null, "process_end_A", 2, new TimeSpan(0, 0, 4, 0, 0), 2 });

            migrationBuilder.InsertData(
                table: "ApsProcess",
                columns: new[] { "Id", "ApsSemiProductId", "Discriminator", "MaximumProductionQuantity", "MinimumProductionQuantity", "PartName", "PrevPartId", "ProductionMode", "ProductionTime", "Workspace" },
                values: new object[] { "process_1_a", null, "ApsManufactureProcess", 1, 1, "process_1_a", null, 2, new TimeSpan(0, 0, 0, 1, 0), 1 });

            migrationBuilder.InsertData(
                table: "ApsProducts",
                columns: new[] { "Id", "ApsAssemblyProcessId" },
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
                column: "Id",
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

            migrationBuilder.CreateIndex(
                name: "IX_ApsAssemblyProcessSemiProduct_ApsSemiProductId",
                table: "ApsAssemblyProcessSemiProduct",
                column: "ApsSemiProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ApsManufactureJobs_ApsManufactureProcessId",
                table: "ApsManufactureJobs",
                column: "ApsManufactureProcessId");

            migrationBuilder.CreateIndex(
                name: "IX_ApsManufactureJobs_ApsOrderId",
                table: "ApsManufactureJobs",
                column: "ApsOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_ApsManufactureJobs_ApsProductId",
                table: "ApsManufactureJobs",
                column: "ApsProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ApsManufactureJobs_ApsSemiProductId",
                table: "ApsManufactureJobs",
                column: "ApsSemiProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ApsManufactureJobs_ProductInstanceId",
                table: "ApsManufactureJobs",
                column: "ProductInstanceId");

            migrationBuilder.CreateIndex(
                name: "IX_ApsManufactureJobs_SemiProductInstanceId",
                table: "ApsManufactureJobs",
                column: "SemiProductInstanceId");

            migrationBuilder.CreateIndex(
                name: "IX_ApsOrders_ProductId",
                table: "ApsOrders",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ApsProcess_ApsSemiProductId",
                table: "ApsProcess",
                column: "ApsSemiProductId");

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
                name: "IX_ApsResources_ApsAssemblyJobId",
                table: "ApsResources",
                column: "ApsAssemblyJobId");

            migrationBuilder.CreateIndex(
                name: "IX_ApsResources_ApsManufactureJobId",
                table: "ApsResources",
                column: "ApsManufactureJobId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductInstances_ApsProductId",
                table: "ProductInstances",
                column: "ApsProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductInstances_OrderedById",
                table: "ProductInstances",
                column: "OrderedById");

            migrationBuilder.CreateIndex(
                name: "IX_ResourceClassWithResource_ApsResourceId",
                table: "ResourceClassWithResource",
                column: "ApsResourceId");

            migrationBuilder.CreateIndex(
                name: "IX_SemiProductInstances_ApsSemiProductId",
                table: "SemiProductInstances",
                column: "ApsSemiProductId");

            migrationBuilder.CreateIndex(
                name: "IX_SemiProductInstances_ProductAssemblyToId",
                table: "SemiProductInstances",
                column: "ProductAssemblyToId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApsAssemblyProcessSemiProduct");

            migrationBuilder.DropTable(
                name: "ApsProcessResource");

            migrationBuilder.DropTable(
                name: "ApsProductSemiProduct");

            migrationBuilder.DropTable(
                name: "ResourceClassWithResource");

            migrationBuilder.DropTable(
                name: "ApsResources");

            migrationBuilder.DropTable(
                name: "ResourceClasses");

            migrationBuilder.DropTable(
                name: "ApsAssemblyJobs");

            migrationBuilder.DropTable(
                name: "ApsManufactureJobs");

            migrationBuilder.DropTable(
                name: "SemiProductInstances");

            migrationBuilder.DropTable(
                name: "ProductInstances");

            migrationBuilder.DropTable(
                name: "ApsOrders");

            migrationBuilder.DropTable(
                name: "ApsProducts");

            migrationBuilder.DropTable(
                name: "ApsProcess");

            migrationBuilder.DropTable(
                name: "ApsSemiProducts");
        }
    }
}
