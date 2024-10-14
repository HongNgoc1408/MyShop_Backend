using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyShop_Backend.Migrations
{
    /// <inheritdoc />
    public partial class AddAddress4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DeliveryAddresses_AspNetUsers_UserId1",
                table: "DeliveryAddresses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DeliveryAddresses",
                table: "DeliveryAddresses");

            migrationBuilder.DropIndex(
                name: "IX_DeliveryAddresses_UserId1",
                table: "DeliveryAddresses");

            migrationBuilder.RenameColumn(
                name: "UserId1",
                table: "DeliveryAddresses",
                newName: "Id");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "DeliveryAddresses",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "DeliveryAddresses",
                newName: "UserId1");

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

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryAddresses_UserId1",
                table: "DeliveryAddresses",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_DeliveryAddresses_AspNetUsers_UserId1",
                table: "DeliveryAddresses",
                column: "UserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
