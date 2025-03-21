using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace iMARSARLIMS.Migrations
{
    /// <inheritdoc />
    public partial class Doctorshare11 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "rate",
                table: "item_outsourcemaster",
                type: "double",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AlterColumn<DateTime>(
                name: "createdDate",
                table: "doctorShareMaster",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(byte[]),
                oldType: "longblob");

            migrationBuilder.AddColumn<string>(
                name: "cancelreason",
                table: "appointmentBooking",
                type: "varchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "reschedulreason",
                table: "appointmentBooking",
                type: "varchar(50)",
                maxLength: 50,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "rate",
                table: "item_outsourcemaster");

            migrationBuilder.DropColumn(
                name: "cancelreason",
                table: "appointmentBooking");

            migrationBuilder.DropColumn(
                name: "reschedulreason",
                table: "appointmentBooking");

            migrationBuilder.AlterColumn<byte[]>(
                name: "createdDate",
                table: "doctorShareMaster",
                type: "longblob",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");
        }
    }
}
