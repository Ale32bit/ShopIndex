using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShopIndexWebApp.Server.Migrations
{
    /// <inheritdoc />
    public partial class ActualCoordinates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ActualLocation",
                table: "Shops",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActualLocation",
                table: "Shops");
        }
    }
}
