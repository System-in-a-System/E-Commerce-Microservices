using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FreakyFashionServices.OrderProcessor.Migrations
{
    public partial class AddOrderKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OrderKey",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderKey",
                table: "Orders");
        }
    }
}
