using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace iMARSARLIMS.Migrations
{
    /// <inheritdoc />
    public partial class bimodelChange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "REfundDate",
                table: "tnx_BookingItem",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "isWhatsApp",
                table: "tnx_BookingItem",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "refundReason",
                table: "tnx_BookingItem",
                type: "varchar(50)",
                maxLength: 50,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "REfundDate",
                table: "tnx_BookingItem");

            migrationBuilder.DropColumn(
                name: "isWhatsApp",
                table: "tnx_BookingItem");

            migrationBuilder.DropColumn(
                name: "refundReason",
                table: "tnx_BookingItem");
        }
    }
}
