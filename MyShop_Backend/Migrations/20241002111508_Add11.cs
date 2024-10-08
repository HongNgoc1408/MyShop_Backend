using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyShop_Backend.Migrations
{
    /// <inheritdoc />
    public partial class Add11 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sizes_Sizes_SizeModelId",
                table: "Sizes");

            migrationBuilder.DropIndex(
                name: "IX_Sizes_SizeModelId",
                table: "Sizes");

            migrationBuilder.DropColumn(
                name: "SizeModelId",
                table: "Sizes");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SizeModelId",
                table: "Sizes",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sizes_SizeModelId",
                table: "Sizes",
                column: "SizeModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sizes_Sizes_SizeModelId",
                table: "Sizes",
                column: "SizeModelId",
                principalTable: "Sizes",
                principalColumn: "Id");
        }
    }
}
