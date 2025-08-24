using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECom.Infrasructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class EditDisplayName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "street",
                table: "Addresses",
                newName: "Street");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Street",
                table: "Addresses",
                newName: "street");
        }
    }
}
