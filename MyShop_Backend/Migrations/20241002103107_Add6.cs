using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyShop_Backend.Migrations
{
    /// <inheritdoc />
    public partial class Add6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ColorModelProductModel");

            migrationBuilder.AddColumn<int>(
                name: "ProductModelId",
                table: "Colors",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Colors_ProductModelId",
                table: "Colors",
                column: "ProductModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Colors_Products_ProductModelId",
                table: "Colors",
                column: "ProductModelId",
                principalTable: "Products",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Colors_Products_ProductModelId",
                table: "Colors");

            migrationBuilder.DropIndex(
                name: "IX_Colors_ProductModelId",
                table: "Colors");

            migrationBuilder.DropColumn(
                name: "ProductModelId",
                table: "Colors");

            migrationBuilder.CreateTable(
                name: "ColorModelProductModel",
                columns: table => new
                {
                    ColorsId = table.Column<int>(type: "int", nullable: false),
                    ProductsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ColorModelProductModel", x => new { x.ColorsId, x.ProductsId });
                    table.ForeignKey(
                        name: "FK_ColorModelProductModel_Colors_ColorsId",
                        column: x => x.ColorsId,
                        principalTable: "Colors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ColorModelProductModel_Products_ProductsId",
                        column: x => x.ProductsId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ColorModelProductModel_ProductsId",
                table: "ColorModelProductModel",
                column: "ProductsId");
        }
    }
}
