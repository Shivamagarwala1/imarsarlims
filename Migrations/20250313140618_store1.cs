using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace iMARSARLIMS.Migrations
{
    /// <inheritdoc />
    public partial class store1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "maxQuantity",
                table: "ItemMasterStore");

            migrationBuilder.RenameColumn(
                name: "minQuantity",
                table: "ItemMasterStore",
                newName: "Quantity");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Quantity",
                table: "ItemMasterStore",
                newName: "minQuantity");

            migrationBuilder.AddColumn<int>(
                name: "maxQuantity",
                table: "ItemMasterStore",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
