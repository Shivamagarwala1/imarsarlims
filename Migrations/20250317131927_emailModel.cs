using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace iMARSARLIMS.Migrations
{
    /// <inheritdoc />
    public partial class emailModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "deliveryDate",
                table: "whatsapp");

            migrationBuilder.DropColumn(
                name: "reportPath",
                table: "whatsapp");

            migrationBuilder.DropColumn(
                name: "smsText",
                table: "whatsapp");

            migrationBuilder.DropColumn(
                name: "templateId",
                table: "whatsapp");

            migrationBuilder.RenameColumn(
                name: "transactionId",
                table: "whatsapp",
                newName: "Header");

            migrationBuilder.CreateTable(
                name: "ReportEmail",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    workOrderId = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false),
                    name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    emailId = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    isSend = table.Column<byte>(type: "tinyint unsigned", nullable: true),
                    isAutoSend = table.Column<byte>(type: "tinyint unsigned", nullable: true),
                    sentBy = table.Column<int>(type: "int", nullable: true),
                    sendDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    remarks = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    type = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true),
                    Header = table.Column<int>(type: "int", nullable: false),
                    createdDate = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportEmail", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReportEmail");

            migrationBuilder.RenameColumn(
                name: "Header",
                table: "whatsapp",
                newName: "transactionId");

            migrationBuilder.AddColumn<DateTime>(
                name: "deliveryDate",
                table: "whatsapp",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "reportPath",
                table: "whatsapp",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "smsText",
                table: "whatsapp",
                type: "longtext",
                nullable: false);

            migrationBuilder.AddColumn<string>(
                name: "templateId",
                table: "whatsapp",
                type: "varchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");
        }
    }
}
