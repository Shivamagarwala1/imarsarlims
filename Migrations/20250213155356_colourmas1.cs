using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace iMARSARLIMS.Migrations
{
    /// <inheritdoc />
    public partial class colourmas1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "contantName",
                table: "LegendColorMaster",
                type: "varchar(30)",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext");

            migrationBuilder.AddColumn<string>(
                name: "ShortName",
                table: "LegendColorMaster",
                type: "varchar(15)",
                maxLength: 15,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "createdById",
                table: "doctorReferalMaster",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "createdDateTime",
                table: "doctorReferalMaster",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<byte>(
                name: "isActive",
                table: "doctorReferalMaster",
                type: "tinyint unsigned",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<int>(
                name: "updateById",
                table: "doctorReferalMaster",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "updateDateTime",
                table: "doctorReferalMaster",
                type: "datetime(6)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShortName",
                table: "LegendColorMaster");

            migrationBuilder.DropColumn(
                name: "createdById",
                table: "doctorReferalMaster");

            migrationBuilder.DropColumn(
                name: "createdDateTime",
                table: "doctorReferalMaster");

            migrationBuilder.DropColumn(
                name: "isActive",
                table: "doctorReferalMaster");

            migrationBuilder.DropColumn(
                name: "updateById",
                table: "doctorReferalMaster");

            migrationBuilder.DropColumn(
                name: "updateDateTime",
                table: "doctorReferalMaster");

            migrationBuilder.AlterColumn<string>(
                name: "contantName",
                table: "LegendColorMaster",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(30)",
                oldMaxLength: 30);
        }
    }
}
