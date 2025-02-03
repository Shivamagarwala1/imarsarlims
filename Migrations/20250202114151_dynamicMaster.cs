using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace iMARSARLIMS.Migrations
{
    /// <inheritdoc />
    public partial class dynamicMaster : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MenuType",
                table: "menuMaster",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "createdById",
                table: "documentTypeMaster",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "createdDateTime",
                table: "documentTypeMaster",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<byte>(
                name: "isActive",
                table: "documentTypeMaster",
                type: "tinyint unsigned",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<int>(
                name: "updateById",
                table: "documentTypeMaster",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "updateDateTime",
                table: "documentTypeMaster",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "createdById",
                table: "discountTypeMaster",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "createdDateTime",
                table: "discountTypeMaster",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<byte>(
                name: "isActive",
                table: "discountTypeMaster",
                type: "tinyint unsigned",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<int>(
                name: "updateById",
                table: "discountTypeMaster",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "updateDateTime",
                table: "discountTypeMaster",
                type: "datetime(6)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MenuType",
                table: "menuMaster");

            migrationBuilder.DropColumn(
                name: "createdById",
                table: "documentTypeMaster");

            migrationBuilder.DropColumn(
                name: "createdDateTime",
                table: "documentTypeMaster");

            migrationBuilder.DropColumn(
                name: "isActive",
                table: "documentTypeMaster");

            migrationBuilder.DropColumn(
                name: "updateById",
                table: "documentTypeMaster");

            migrationBuilder.DropColumn(
                name: "updateDateTime",
                table: "documentTypeMaster");

            migrationBuilder.DropColumn(
                name: "createdById",
                table: "discountTypeMaster");

            migrationBuilder.DropColumn(
                name: "createdDateTime",
                table: "discountTypeMaster");

            migrationBuilder.DropColumn(
                name: "isActive",
                table: "discountTypeMaster");

            migrationBuilder.DropColumn(
                name: "updateById",
                table: "discountTypeMaster");

            migrationBuilder.DropColumn(
                name: "updateDateTime",
                table: "discountTypeMaster");
        }
    }
}
