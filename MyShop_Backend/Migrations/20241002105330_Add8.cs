using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyShop_Backend.Migrations
{
    /// <inheritdoc />
    public partial class Add8 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ColorModelSizeModel");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SizeModel",
                table: "SizeModel");

            migrationBuilder.RenameTable(
                name: "SizeModel",
                newName: "Sizes");

            migrationBuilder.AddColumn<int>(
                name: "Color_SizeModelColorId",
                table: "Sizes",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Color_SizeModelSizeId",
                table: "Sizes",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Sizes",
                table: "Sizes",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "ColorSizes",
                columns: table => new
                {
                    ColorId = table.Column<int>(type: "int", nullable: false),
                    SizeId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    ColorModelId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ColorSizes", x => new { x.ColorId, x.SizeId });
                    table.ForeignKey(
                        name: "FK_ColorSizes_Colors_ColorModelId",
                        column: x => x.ColorModelId,
                        principalTable: "Colors",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Sizes_Color_SizeModelColorId_Color_SizeModelSizeId",
                table: "Sizes",
                columns: new[] { "Color_SizeModelColorId", "Color_SizeModelSizeId" });

            migrationBuilder.CreateIndex(
                name: "IX_ColorSizes_ColorModelId",
                table: "ColorSizes",
                column: "ColorModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sizes_ColorSizes_Color_SizeModelColorId_Color_SizeModelSizeId",
                table: "Sizes",
                columns: new[] { "Color_SizeModelColorId", "Color_SizeModelSizeId" },
                principalTable: "ColorSizes",
                principalColumns: new[] { "ColorId", "SizeId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sizes_ColorSizes_Color_SizeModelColorId_Color_SizeModelSizeId",
                table: "Sizes");

            migrationBuilder.DropTable(
                name: "ColorSizes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Sizes",
                table: "Sizes");

            migrationBuilder.DropIndex(
                name: "IX_Sizes_Color_SizeModelColorId_Color_SizeModelSizeId",
                table: "Sizes");

            migrationBuilder.DropColumn(
                name: "Color_SizeModelColorId",
                table: "Sizes");

            migrationBuilder.DropColumn(
                name: "Color_SizeModelSizeId",
                table: "Sizes");

            migrationBuilder.RenameTable(
                name: "Sizes",
                newName: "SizeModel");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SizeModel",
                table: "SizeModel",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "ColorModelSizeModel",
                columns: table => new
                {
                    ColorsId = table.Column<int>(type: "int", nullable: false),
                    SizesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ColorModelSizeModel", x => new { x.ColorsId, x.SizesId });
                    table.ForeignKey(
                        name: "FK_ColorModelSizeModel_Colors_ColorsId",
                        column: x => x.ColorsId,
                        principalTable: "Colors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ColorModelSizeModel_SizeModel_SizesId",
                        column: x => x.SizesId,
                        principalTable: "SizeModel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ColorModelSizeModel_SizesId",
                table: "ColorModelSizeModel",
                column: "SizesId");
        }
    }
}
