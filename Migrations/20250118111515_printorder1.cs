using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace iMARSARLIMS.Migrations
{
    /// <inheritdoc />
    public partial class printorder1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "mappedDate",
                table: "ItemObservationMapping",
                newName: "createdDateTime");

            migrationBuilder.AddColumn<int>(
                name: "createdById",
                table: "ItemObservationMapping",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "isActive",
                table: "ItemObservationMapping",
                type: "tinyint unsigned",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<int>(
                name: "updateById",
                table: "ItemObservationMapping",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "updateDateTime",
                table: "ItemObservationMapping",
                type: "datetime(6)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "createdById",
                table: "ItemObservationMapping");

            migrationBuilder.DropColumn(
                name: "isActive",
                table: "ItemObservationMapping");

            migrationBuilder.DropColumn(
                name: "updateById",
                table: "ItemObservationMapping");

            migrationBuilder.DropColumn(
                name: "updateDateTime",
                table: "ItemObservationMapping");

            migrationBuilder.RenameColumn(
                name: "createdDateTime",
                table: "ItemObservationMapping",
                newName: "mappedDate");
        }
    }
}
