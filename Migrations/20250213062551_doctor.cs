using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace iMARSARLIMS.Migrations
{
    /// <inheritdoc />
    public partial class doctor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "type",
                table: "doctorReferalMaster",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<DateTime>(
                name: "paymentDate",
                table: "CentrePayment",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "type",
                table: "doctorReferalMaster");

            migrationBuilder.AlterColumn<DateTime>(
                name: "paymentDate",
                table: "CentrePayment",
                type: "datetime(6)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");
        }
    }
}
