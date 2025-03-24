using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace iMARSARLIMS.Migrations
{
    /// <inheritdoc />
    public partial class reportHeader11 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {



            migrationBuilder.AddColumn<string>(
                name: "Alignment",
                table: "labReportHeader",
                type: "varchar(8)",
                maxLength: 8,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Bold",
                table: "labReportHeader",
                type: "varchar(5)",
                maxLength: 5,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Fname",
                table: "labReportHeader",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Fsize",
                table: "labReportHeader",
                type: "int",
                maxLength: 50,
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Height",
                table: "labReportHeader",
                type: "varchar(5)",
                maxLength: 5,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Italic",
                table: "labReportHeader",
                type: "varchar(5)",
                maxLength: 5,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Label",
                table: "labReportHeader",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "labReportHeader",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "P_forecolor",
                table: "labReportHeader",
                type: "varchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Print",
                table: "labReportHeader",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Under",
                table: "labReportHeader",
                type: "varchar(5)",
                maxLength: 5,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Width",
                table: "labReportHeader",
                type: "varchar(5)",
                maxLength: 5,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "printOrder",
                table: "labReportHeader",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<byte>(
                name: "allowTicket",
                table: "empMaster",
                type: "tinyint unsigned",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "allowTicketRole",
                table: "empMaster",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ReportHeader",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    reportHeader = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportHeader", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReportHeader");

            migrationBuilder.DropColumn(
                name: "roleId",
                table: "supportTicket");

            migrationBuilder.DropColumn(
                name: "Alignment",
                table: "labReportHeader");

            migrationBuilder.DropColumn(
                name: "Bold",
                table: "labReportHeader");

            migrationBuilder.DropColumn(
                name: "Fname",
                table: "labReportHeader");

            migrationBuilder.DropColumn(
                name: "Fsize",
                table: "labReportHeader");

            migrationBuilder.DropColumn(
                name: "Height",
                table: "labReportHeader");

            migrationBuilder.DropColumn(
                name: "Italic",
                table: "labReportHeader");

            migrationBuilder.DropColumn(
                name: "Label",
                table: "labReportHeader");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "labReportHeader");

            migrationBuilder.DropColumn(
                name: "P_forecolor",
                table: "labReportHeader");

            migrationBuilder.DropColumn(
                name: "Print",
                table: "labReportHeader");

            migrationBuilder.DropColumn(
                name: "Under",
                table: "labReportHeader");

            migrationBuilder.DropColumn(
                name: "Width",
                table: "labReportHeader");

            migrationBuilder.DropColumn(
                name: "printOrder",
                table: "labReportHeader");

            migrationBuilder.DropColumn(
                name: "allowTicket",
                table: "empMaster");

            migrationBuilder.DropColumn(
                name: "allowTicketRole",
                table: "empMaster");

            migrationBuilder.AddColumn<string>(
                name: "headerCSS",
                table: "labReportHeader",
                type: "longtext",
                nullable: true);
        }
    }
}
