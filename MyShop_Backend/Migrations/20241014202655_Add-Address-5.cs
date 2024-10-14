using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyShop_Backend.Migrations
{
    /// <inheritdoc />
    public partial class AddAddress5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DeliveryAddresses_AspNetUsers_UserId",
                table: "DeliveryAddresses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DeliveryAddresses",
                table: "DeliveryAddresses");

            migrationBuilder.DropIndex(
                name: "IX_DeliveryAddresses_UserId",
                table: "DeliveryAddresses");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "DeliveryAddresses");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "DeliveryAddresses",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_DeliveryAddresses",
                table: "DeliveryAddresses",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_DeliveryAddresses_AspNetUsers_UserId",
                table: "DeliveryAddresses",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DeliveryAddresses_AspNetUsers_UserId",
                table: "DeliveryAddresses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DeliveryAddresses",
                table: "DeliveryAddresses");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "DeliveryAddresses",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "Id",
                table: "DeliveryAddresses",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DeliveryAddresses",
                table: "DeliveryAddresses",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryAddresses_UserId",
                table: "DeliveryAddresses",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_DeliveryAddresses_AspNetUsers_UserId",
                table: "DeliveryAddresses",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
