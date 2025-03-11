using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace iMARSARLIMS.Migrations
{
    /// <inheritdoc />
    public partial class vendorModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "createdById",
                table: "VendorMaster",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "createdDateTime",
                table: "VendorMaster",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<byte>(
                name: "isActive",
                table: "VendorMaster",
                type: "tinyint unsigned",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<int>(
                name: "updateById",
                table: "VendorMaster",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "updateDateTime",
                table: "VendorMaster",
                type: "datetime(6)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "createdById",
                table: "VendorMaster");

            migrationBuilder.DropColumn(
                name: "createdDateTime",
                table: "VendorMaster");

            migrationBuilder.DropColumn(
                name: "isActive",
                table: "VendorMaster");

            migrationBuilder.DropColumn(
                name: "updateById",
                table: "VendorMaster");

            migrationBuilder.DropColumn(
                name: "updateDateTime",
                table: "VendorMaster");
        }
    }
}
