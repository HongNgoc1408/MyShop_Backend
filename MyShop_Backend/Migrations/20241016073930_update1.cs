using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyShop_Backend.Migrations
{
    /// <inheritdoc />
    public partial class update1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_DeliveryStatuses_DeliveryStatusName",
                table: "Orders");

            migrationBuilder.DropTable(
                name: "DeliveryStatuses");

            migrationBuilder.DropIndex(
                name: "IX_Orders_DeliveryStatusName",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "DeliveryStatusName",
                table: "Orders");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DeliveryStatusName",
                table: "Orders",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DeliveryStatuses",
                columns: table => new
                {
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryStatuses", x => x.Name);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_DeliveryStatusName",
                table: "Orders",
                column: "DeliveryStatusName");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_DeliveryStatuses_DeliveryStatusName",
                table: "Orders",
                column: "DeliveryStatusName",
                principalTable: "DeliveryStatuses",
                principalColumn: "Name");
        }
    }
}
