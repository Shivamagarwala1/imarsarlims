using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace iMARSARLIMS.Migrations
{
    /// <inheritdoc />
    public partial class dynamicMaster1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "createdById",
                table: "labDepartment",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "createdDateTime",
                table: "labDepartment",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<byte>(
                name: "isActive",
                table: "labDepartment",
                type: "tinyint unsigned",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<int>(
                name: "updateById",
                table: "labDepartment",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "updateDateTime",
                table: "labDepartment",
                type: "datetime(6)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "createdById",
                table: "labDepartment");

            migrationBuilder.DropColumn(
                name: "createdDateTime",
                table: "labDepartment");

            migrationBuilder.DropColumn(
                name: "isActive",
                table: "labDepartment");

            migrationBuilder.DropColumn(
                name: "updateById",
                table: "labDepartment");

            migrationBuilder.DropColumn(
                name: "updateDateTime",
                table: "labDepartment");
        }
    }
}
