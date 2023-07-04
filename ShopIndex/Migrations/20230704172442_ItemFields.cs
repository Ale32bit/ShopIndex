using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShopIndex.Migrations
{
    /// <inheritdoc />
    public partial class ItemFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "DynamicPrices",
                table: "Items",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "MadeOnDemand",
                table: "Items",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ShopBuysItem",
                table: "Items",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DynamicPrices",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "MadeOnDemand",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "ShopBuysItem",
                table: "Items");
        }
    }
}
