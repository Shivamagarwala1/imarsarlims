using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace iMARSARLIMS.Migrations
{
    /// <inheritdoc />
    public partial class ReportFormat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InvestigationMasterUD",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    investigationId = table.Column<int>(type: "int", nullable: false),
                    investigationName = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    formatUrl = table.Column<string>(type: "varchar(300)", maxLength: 300, nullable: false),
                    isActive = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    createdById = table.Column<int>(type: "int", nullable: true),
                    createdDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    updateById = table.Column<int>(type: "int", nullable: true),
                    updateDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvestigationMasterUD", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "VendorMaster",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    supplierName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    supplierCode = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    supplierType = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    supplierCategory = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    Address = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false),
                    country = table.Column<int>(type: "int", nullable: false),
                    state = table.Column<int>(type: "int", nullable: false),
                    city = table.Column<int>(type: "int", nullable: false),
                    pinCode = table.Column<string>(type: "varchar(6)", maxLength: 6, nullable: false),
                    landline = table.Column<string>(type: "varchar(15)", maxLength: 15, nullable: false),
                    faxNo = table.Column<string>(type: "varchar(15)", maxLength: 15, nullable: false),
                    emailId = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    website = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    contactPerson = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    ContactNo = table.Column<string>(type: "varchar(15)", maxLength: 15, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VendorMaster", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InvestigationMasterUD");

            migrationBuilder.DropTable(
                name: "VendorMaster");
        }
    }
}
