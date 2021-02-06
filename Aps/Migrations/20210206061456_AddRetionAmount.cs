using Microsoft.EntityFrameworkCore.Migrations;

namespace Aps.Migrations
{
    public partial class AddRetionAmount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApsAssemblyProcessSemiProducts");

            migrationBuilder.DropTable(
                name: "ApsProcessResources");

            migrationBuilder.DropTable(
                name: "ApsProductApsSemiProduct");

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
                    ResourceAttribute = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", nullable: false),
                    Amount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApsProcessResource", x => new { x.ApsProcessId, x.ResourceAttribute });
                    table.ForeignKey(
                        name: "FK_ApsProcessResource_ApsProcess_ApsProcessId",
                        column: x => x.ApsProcessId,
                        principalTable: "ApsProcess",
                        principalColumn: "PartId",
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
                        name: "FK_ApsProductSemiProduct_ApsProduct_ApsProductId",
                        column: x => x.ApsProductId,
                        principalTable: "ApsProduct",
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

            migrationBuilder.InsertData(
                table: "ApsProcessResource",
                columns: new[] { "ApsProcessId", "ResourceAttribute", "Amount" },
                values: new object[,]
                {
                    { "process_1_a", "高级人员", 1 },
                    { "process_1_a", "人员", 1 },
                    { "process_1_a", "高级机床", 2 },
                    { "process_1_a", "机床", 3 },
                    { "process_end_A", "高级设备", 2 },
                    { "process_end_A", "人员", 1 },
                    { "process_end_A", "高级人员", 1 },
                    { "process_1_a", "设备", 0 },
                    { "process_end_A", "高级机床", 2 },
                    { "process_end_A", "机床", 3 },
                    { "process_end_A", "设备", 3 },
                    { "process_1_a", "高级设备", 2 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApsAssemblyProcessSemiProduct_ApsSemiProductId",
                table: "ApsAssemblyProcessSemiProduct",
                column: "ApsSemiProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ApsProductSemiProduct_ApsProductId",
                table: "ApsProductSemiProduct",
                column: "ApsProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApsAssemblyProcessSemiProduct");

            migrationBuilder.DropTable(
                name: "ApsProcessResource");

            migrationBuilder.DropTable(
                name: "ApsProductSemiProduct");

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

            migrationBuilder.InsertData(
                table: "ApsAssemblyProcessSemiProducts",
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

            migrationBuilder.InsertData(
                table: "ApsProcessResources",
                columns: new[] { "ApsProcessId", "ResourceAttribute", "Amount" },
                values: new object[,]
                {
                    { "process_1_a", "高级人员", 1 },
                    { "process_1_a", "人员", 1 },
                    { "process_1_a", "高级机床", 2 },
                    { "process_1_a", "机床", 3 },
                    { "process_end_A", "高级设备", 2 },
                    { "process_end_A", "人员", 1 },
                    { "process_end_A", "高级人员", 1 },
                    { "process_1_a", "设备", 3 },
                    { "process_end_A", "高级机床", 2 },
                    { "process_end_A", "机床", 3 },
                    { "process_end_A", "设备", 3 },
                    { "process_1_a", "高级设备", 2 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApsAssemblyProcessSemiProducts_ApsSemiProductId",
                table: "ApsAssemblyProcessSemiProducts",
                column: "ApsSemiProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ApsProductApsSemiProduct_AssembleBySemiProductsSemiProductId",
                table: "ApsProductApsSemiProduct",
                column: "AssembleBySemiProductsSemiProductId");
        }
    }
}
