using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyShop_Backend.Migrations
{
    /// <inheritdoc />
    public partial class AddOrder3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentTranId",
                table: "Orders");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PaymentTranId",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
