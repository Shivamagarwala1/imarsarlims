using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace iMARSARLIMS.Migrations
{
    /// <inheritdoc />
    public partial class Doctorspec1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte>(
                name: "AllowOPD",
                table: "doctorReferalMaster",
                type: "tinyint unsigned",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<byte>(
                name: "Discount",
                table: "doctorReferalMaster",
                type: "tinyint unsigned",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<string>(
                name: "EmailReport",
                table: "doctorReferalMaster",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<byte>(
                name: "OPDFee",
                table: "doctorReferalMaster",
                type: "tinyint unsigned",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<byte>(
                name: "OnlineLogin",
                table: "doctorReferalMaster",
                type: "tinyint unsigned",
                nullable: false,
                defaultValue: (byte)0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AllowOPD",
                table: "doctorReferalMaster");

            migrationBuilder.DropColumn(
                name: "Discount",
                table: "doctorReferalMaster");

            migrationBuilder.DropColumn(
                name: "EmailReport",
                table: "doctorReferalMaster");

            migrationBuilder.DropColumn(
                name: "OPDFee",
                table: "doctorReferalMaster");

            migrationBuilder.DropColumn(
                name: "OnlineLogin",
                table: "doctorReferalMaster");
        }
    }
}
