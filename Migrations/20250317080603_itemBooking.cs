using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace iMARSARLIMS.Migrations
{
    /// <inheritdoc />
    public partial class itemBooking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "REfundDate",
                table: "tnx_BookingItem",
                newName: "RefundDate");

            migrationBuilder.AddColumn<int>(
                name: "Isprint",
                table: "tnx_BookingItem",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Isprint",
                table: "tnx_BookingItem");

            migrationBuilder.RenameColumn(
                name: "RefundDate",
                table: "tnx_BookingItem",
                newName: "REfundDate");
        }
    }
}
