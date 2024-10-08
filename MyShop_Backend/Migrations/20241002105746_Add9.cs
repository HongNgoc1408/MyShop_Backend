using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyShop_Backend.Migrations
{
    /// <inheritdoc />
    public partial class Add9 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sizes_ColorSizes_Color_SizeModelColorId_Color_SizeModelSizeId",
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.CreateIndex(
                name: "IX_Sizes_Color_SizeModelColorId_Color_SizeModelSizeId",
                table: "Sizes",
                columns: new[] { "Color_SizeModelColorId", "Color_SizeModelSizeId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Sizes_ColorSizes_Color_SizeModelColorId_Color_SizeModelSizeId",
                table: "Sizes",
                columns: new[] { "Color_SizeModelColorId", "Color_SizeModelSizeId" },
                principalTable: "ColorSizes",
                principalColumns: new[] { "ColorId", "SizeId" });
        }
    }
}
