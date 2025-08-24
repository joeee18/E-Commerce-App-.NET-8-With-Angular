using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ECom.Infrasructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedDelivery : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "DeliveryMethods",
                columns: new[] { "Id", "DeliveryTime", "Description", "Name", "Price" },
                values: new object[,]
                {
                    { 1, "only a week", "the fast Delivery in the world", "DHL", 15m },
                    { 2, "only a two week", "Make your Product save", "FAST", 12m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "DeliveryMethods",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "DeliveryMethods",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
