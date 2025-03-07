using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace iMARSARLIMS.Migrations
{
    /// <inheritdoc />
    public partial class textreport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ReportType",
                table: "ResultEntryResponseModle",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "hold",
                table: "ResultEntryResponseModle",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "isapproved",
                table: "ResultEntryResponseModle",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "rateListData",
                columns: table => new
                {
                    ratetype = table.Column<string>(type: "longtext", nullable: true),
                    itemid = table.Column<int>(type: "int", nullable: true),
                    itemCode = table.Column<string>(type: "longtext", nullable: true),
                    itemname = table.Column<string>(type: "longtext", nullable: true),
                    rateTypeId = table.Column<int>(type: "int", nullable: true),
                    mrp = table.Column<double>(type: "double", nullable: true),
                    rate = table.Column<double>(type: "double", nullable: true)
                },
                constraints: table =>
                {
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tnx_investigationtext_Report",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    testId = table.Column<int>(type: "int", nullable: false),
                    value = table.Column<string>(type: "longtext", nullable: false),
                    createdbyId = table.Column<int>(type: "int", nullable: false),
                    createdate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tnx_investigationtext_Report", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "rateListData");

            migrationBuilder.DropTable(
                name: "tnx_investigationtext_Report");

            migrationBuilder.DropColumn(
                name: "ReportType",
                table: "ResultEntryResponseModle");

            migrationBuilder.DropColumn(
                name: "hold",
                table: "ResultEntryResponseModle");

            migrationBuilder.DropColumn(
                name: "isapproved",
                table: "ResultEntryResponseModle");
        }
    }
}
