using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyShop_Backend.Migrations
{
    /// <inheritdoc />
    public partial class Add10 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Colors_Products_ProductModelId",
                table: "Colors");

            migrationBuilder.DropForeignKey(
                name: "FK_ColorSizes_Colors_ColorModelId",
                table: "ColorSizes");

            migrationBuilder.DropIndex(
                name: "IX_ColorSizes_ColorModelId",
                table: "ColorSizes");

            migrationBuilder.DropIndex(
                name: "IX_Colors_ProductModelId",
                table: "Colors");

            migrationBuilder.DropColumn(
                name: "ColorModelId",
                table: "ColorSizes");

            migrationBuilder.DropColumn(
                name: "ProductModelId",
                table: "Colors");

            migrationBuilder.RenameColumn(
                name: "UpdateAt",
                table: "Sizes",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "CreateAt",
                table: "Sizes",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "UpdateAt",
                table: "Colors",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "CreateAt",
                table: "Colors",
                newName: "CreatedAt");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Sizes",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "ColorModelId",
                table: "Sizes",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SizeModelId",
                table: "Sizes",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ColorsId",
                table: "ColorSizes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "ColorSizes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "ColorSizes",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "Colors",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ProductsId",
                table: "Colors",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Sizes_ColorModelId",
                table: "Sizes",
                column: "ColorModelId");

            migrationBuilder.CreateIndex(
                name: "IX_Sizes_SizeModelId",
                table: "Sizes",
                column: "SizeModelId");

            migrationBuilder.CreateIndex(
                name: "IX_ColorSizes_ColorsId",
                table: "ColorSizes",
                column: "ColorsId");

            migrationBuilder.CreateIndex(
                name: "IX_ColorSizes_SizeId",
                table: "ColorSizes",
                column: "SizeId");

            migrationBuilder.CreateIndex(
                name: "IX_Colors_ProductsId",
                table: "Colors",
                column: "ProductsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Colors_Products_ProductsId",
                table: "Colors",
                column: "ProductsId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ColorSizes_Colors_ColorsId",
                table: "ColorSizes",
                column: "ColorsId",
                principalTable: "Colors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ColorSizes_Sizes_SizeId",
                table: "ColorSizes",
                column: "SizeId",
                principalTable: "Sizes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Sizes_Colors_ColorModelId",
                table: "Sizes",
                column: "ColorModelId",
                principalTable: "Colors",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Sizes_Sizes_SizeModelId",
                table: "Sizes",
                column: "SizeModelId",
                principalTable: "Sizes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Colors_Products_ProductsId",
                table: "Colors");

            migrationBuilder.DropForeignKey(
                name: "FK_ColorSizes_Colors_ColorsId",
                table: "ColorSizes");

            migrationBuilder.DropForeignKey(
                name: "FK_ColorSizes_Sizes_SizeId",
                table: "ColorSizes");

            migrationBuilder.DropForeignKey(
                name: "FK_Sizes_Colors_ColorModelId",
                table: "Sizes");

            migrationBuilder.DropForeignKey(
                name: "FK_Sizes_Sizes_SizeModelId",
                table: "Sizes");

            migrationBuilder.DropIndex(
                name: "IX_Sizes_ColorModelId",
                table: "Sizes");

            migrationBuilder.DropIndex(
                name: "IX_Sizes_SizeModelId",
                table: "Sizes");

            migrationBuilder.DropIndex(
                name: "IX_ColorSizes_ColorsId",
                table: "ColorSizes");

            migrationBuilder.DropIndex(
                name: "IX_ColorSizes_SizeId",
                table: "ColorSizes");

            migrationBuilder.DropIndex(
                name: "IX_Colors_ProductsId",
                table: "Colors");

            migrationBuilder.DropColumn(
                name: "ColorModelId",
                table: "Sizes");

            migrationBuilder.DropColumn(
                name: "SizeModelId",
                table: "Sizes");

            migrationBuilder.DropColumn(
                name: "ColorsId",
                table: "ColorSizes");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "ColorSizes");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "ColorSizes");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "Colors");

            migrationBuilder.DropColumn(
                name: "ProductsId",
                table: "Colors");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "Sizes",
                newName: "UpdateAt");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Sizes",
                newName: "CreateAt");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "Colors",
                newName: "UpdateAt");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Colors",
                newName: "CreateAt");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Sizes",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30);

            migrationBuilder.AddColumn<int>(
                name: "ColorModelId",
                table: "ColorSizes",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProductModelId",
                table: "Colors",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ColorSizes_ColorModelId",
                table: "ColorSizes",
                column: "ColorModelId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_ColorSizes_Colors_ColorModelId",
                table: "ColorSizes",
                column: "ColorModelId",
                principalTable: "Colors",
                principalColumn: "Id");
        }
    }
}
