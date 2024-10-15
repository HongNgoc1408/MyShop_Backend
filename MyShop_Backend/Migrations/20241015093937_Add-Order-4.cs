using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyShop_Backend.Migrations
{
    /// <inheritdoc />
    public partial class AddOrder4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PaymentMethodName",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PaymentTranId",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentMethodName",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "PaymentTranId",
                table: "Orders");
        }
    }
}
