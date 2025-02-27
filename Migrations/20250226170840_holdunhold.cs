using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace iMARSARLIMS.Migrations
{
    /// <inheritdoc />
    public partial class holdunhold : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DoctorSignId",
                table: "tnx_BookingItem",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UnholdById",
                table: "tnx_BookingItem",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "hold",
                table: "tnx_BookingItem",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "holdById",
                table: "tnx_BookingItem",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "holdDate",
                table: "tnx_BookingItem",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "holdReason",
                table: "tnx_BookingItem",
                type: "varchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "unHoldDate",
                table: "tnx_BookingItem",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "isSign",
                table: "doctorApprovalMaster",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DoctorApprovalSign",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    DoctorId = table.Column<int>(type: "int", nullable: false),
                    DoctorSign = table.Column<string>(type: "longtext", nullable: true),
                    isActive = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    createdById = table.Column<int>(type: "int", nullable: true),
                    createdDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    updateById = table.Column<int>(type: "int", nullable: true),
                    updateDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoctorApprovalSign", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DoctorApprovalSign");

            migrationBuilder.DropColumn(
                name: "DoctorSignId",
                table: "tnx_BookingItem");

            migrationBuilder.DropColumn(
                name: "UnholdById",
                table: "tnx_BookingItem");

            migrationBuilder.DropColumn(
                name: "hold",
                table: "tnx_BookingItem");

            migrationBuilder.DropColumn(
                name: "holdById",
                table: "tnx_BookingItem");

            migrationBuilder.DropColumn(
                name: "holdDate",
                table: "tnx_BookingItem");

            migrationBuilder.DropColumn(
                name: "holdReason",
                table: "tnx_BookingItem");

            migrationBuilder.DropColumn(
                name: "unHoldDate",
                table: "tnx_BookingItem");

            migrationBuilder.DropColumn(
                name: "isSign",
                table: "doctorApprovalMaster");
        }
    }
}
