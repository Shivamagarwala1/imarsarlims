using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace iMARSARLIMS.Migrations
{
    /// <inheritdoc />
    public partial class discountreson : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "createdById",
                table: "discountReasonMaster",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "createdDateTime",
                table: "discountReasonMaster",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<byte>(
                name: "isActive",
                table: "discountReasonMaster",
                type: "tinyint unsigned",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<int>(
                name: "updateById",
                table: "discountReasonMaster",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "updateDateTime",
                table: "discountReasonMaster",
                type: "datetime(6)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "createdById",
                table: "discountReasonMaster");

            migrationBuilder.DropColumn(
                name: "createdDateTime",
                table: "discountReasonMaster");

            migrationBuilder.DropColumn(
                name: "isActive",
                table: "discountReasonMaster");

            migrationBuilder.DropColumn(
                name: "updateById",
                table: "discountReasonMaster");

            migrationBuilder.DropColumn(
                name: "updateDateTime",
                table: "discountReasonMaster");
        }
    }
}
