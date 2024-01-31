using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShopIndex.Migrations
{
    /// <inheritdoc />
    public partial class ActualDimensionField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ActualDimension",
                table: "Shops",
                type: "INTEGER",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActualDimension",
                table: "Shops");
        }
    }
}
