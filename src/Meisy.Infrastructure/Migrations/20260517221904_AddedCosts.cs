using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Meisy.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedCosts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "CostAtTheMoment",
                table: "Order_Products",
                type: "decimal(65,30)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CostAtTheMoment",
                table: "Order_Products");
        }
    }
}
