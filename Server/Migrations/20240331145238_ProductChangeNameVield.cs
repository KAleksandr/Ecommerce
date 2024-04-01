using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce.Server.Migrations
{
    /// <inheritdoc />
    public partial class ProductChangeNameVield : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Delited",
                table: "ProductTypes",
                newName: "Deleted");

            migrationBuilder.RenameColumn(
                name: "Delited",
                table: "Products",
                newName: "Deleted");

            migrationBuilder.RenameColumn(
                name: "Delited",
                table: "Categories",
                newName: "Deleted");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Deleted",
                table: "ProductTypes",
                newName: "Delited");

            migrationBuilder.RenameColumn(
                name: "Deleted",
                table: "Products",
                newName: "Delited");

            migrationBuilder.RenameColumn(
                name: "Deleted",
                table: "Categories",
                newName: "Delited");
        }
    }
}
