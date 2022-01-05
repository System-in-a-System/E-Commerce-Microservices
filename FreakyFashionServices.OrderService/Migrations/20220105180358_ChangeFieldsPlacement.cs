using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FreakyFashionServices.OrderService.Migrations
{
    public partial class ChangeFieldsPlacement : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OrderKey",
                table: "Orders",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_OrderKey",
                table: "Orders",
                column: "OrderKey",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Orders_OrderKey",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "OrderKey",
                table: "Orders");
        }
    }
}
