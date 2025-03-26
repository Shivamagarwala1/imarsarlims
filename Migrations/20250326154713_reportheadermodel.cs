using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace iMARSARLIMS.Migrations
{
    /// <inheritdoc />
    public partial class reportheadermodel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "addedBy",
                table: "SupportTicketRemarks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "Under",
                table: "labReportHeader",
                type: "int",
                maxLength: 5,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(5)",
                oldMaxLength: 5);

            migrationBuilder.AlterColumn<int>(
                name: "Italic",
                table: "labReportHeader",
                type: "int",
                maxLength: 5,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(5)",
                oldMaxLength: 5);

            migrationBuilder.AlterColumn<int>(
                name: "Bold",
                table: "labReportHeader",
                type: "int",
                maxLength: 5,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(5)",
                oldMaxLength: 5);

            migrationBuilder.CreateTable(
                name: "LedgerStatusModel",
                columns: table => new
                {
                    ParentCentreID = table.Column<int>(type: "int", nullable: true),
                    IsLock = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    CcreditLimt = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CentreType = table.Column<string>(type: "longtext", nullable: true),
                    LCentreId = table.Column<int>(type: "int", nullable: true),
                    CentreCode = table.Column<string>(type: "longtext", nullable: true),
                    CompanyName = table.Column<string>(type: "longtext", nullable: true),
                    CentreAdd = table.Column<string>(type: "longtext", nullable: true),
                    CentreMobile = table.Column<string>(type: "longtext", nullable: true),
                    CreditPeridos = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Cactive = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    UnlockBy = table.Column<string>(type: "longtext", nullable: true),
                    LockDate = table.Column<string>(type: "longtext", nullable: true),
                    UnlockDate = table.Column<string>(type: "longtext", nullable: true),
                    InvoiceNo = table.Column<string>(type: "longtext", nullable: true),
                    Remarks = table.Column<string>(type: "longtext", nullable: true),
                    CreatedDate = table.Column<string>(type: "longtext", nullable: true),
                    InvoiceAmt = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CreationPayment = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ApprovedPayment = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CurrentMPayment = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    UnPaid = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    RemainingPayment = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CurrentBuss = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    AvailableBalance = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TodayBussiness = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    YesterDayBussiness = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                })
                .Annotation("MySQL:Charset", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LedgerStatusModel");

            migrationBuilder.DropColumn(
                name: "addedBy",
                table: "SupportTicketRemarks");

            migrationBuilder.AlterColumn<string>(
                name: "Under",
                table: "labReportHeader",
                type: "varchar(5)",
                maxLength: 5,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldMaxLength: 5);

            migrationBuilder.AlterColumn<string>(
                name: "Italic",
                table: "labReportHeader",
                type: "varchar(5)",
                maxLength: 5,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldMaxLength: 5);

            migrationBuilder.AlterColumn<string>(
                name: "Bold",
                table: "labReportHeader",
                type: "varchar(5)",
                maxLength: 5,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldMaxLength: 5);
        }
    }
}
