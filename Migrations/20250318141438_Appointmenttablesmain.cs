using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace iMARSARLIMS.Migrations
{
    /// <inheritdoc />
    public partial class Appointmenttablesmain : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "isAppointment",
                table: "tnx_Booking",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "appointmentBooking",
                columns: table => new
                {
                    appointmentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    transactionId = table.Column<int>(type: "int", nullable: false),
                    WorkorderId = table.Column<string>(type: "longtext", nullable: false),
                    AppointmentType = table.Column<int>(type: "int", nullable: false),
                    AppointmentScheduledOn = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Pincode = table.Column<string>(type: "longtext", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    AssignedPhlebo = table.Column<int>(type: "int", nullable: true),
                    assignedBy = table.Column<int>(type: "int", nullable: true),
                    cancleBy = table.Column<int>(type: "int", nullable: true),
                    AssignedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CancelDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    rescheduleDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    rescheduleBy = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_appointmentBooking", x => x.appointmentId);
                    table.ForeignKey(
                        name: "FK_appointmentBooking_tnx_Booking_transactionId",
                        column: x => x.transactionId,
                        principalTable: "tnx_Booking",
                        principalColumn: "transactionId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_appointmentBooking_transactionId",
                table: "appointmentBooking",
                column: "transactionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "appointmentBooking");

            migrationBuilder.DropColumn(
                name: "isAppointment",
                table: "tnx_Booking");
        }
    }
}
