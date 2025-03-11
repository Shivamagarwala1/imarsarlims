using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace iMARSARLIMS.Migrations
{
    /// <inheritdoc />
    public partial class indentdata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VendorMaster");

            migrationBuilder.CreateTable(
                name: "Indent",
                columns: table => new
                {
                    indentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    indentBy = table.Column<string>(type: "longtext", nullable: false),
                    indentById = table.Column<int>(type: "int", nullable: false),
                    indentStatus = table.Column<int>(type: "int", nullable: false),
                    isrejected = table.Column<int>(type: "int", nullable: false),
                    rejectedBy = table.Column<int>(type: "int", nullable: false),
                    RejectDatetime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    isActive = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    createdById = table.Column<int>(type: "int", nullable: true),
                    createdDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    updateById = table.Column<int>(type: "int", nullable: true),
                    updateDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Indent", x => x.indentId);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ItemMasterStore",
                columns: table => new
                {
                    itemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    itemName = table.Column<string>(type: "longtext", nullable: false),
                    Category = table.Column<int>(type: "int", nullable: false),
                    minQuantity = table.Column<int>(type: "int", nullable: false),
                    maxQuantity = table.Column<int>(type: "int", nullable: false),
                    isActive = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    createdById = table.Column<int>(type: "int", nullable: true),
                    createdDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    updateById = table.Column<int>(type: "int", nullable: true),
                    updateDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemMasterStore", x => x.itemId);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "machineRerunTestDetail",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    observationId = table.Column<int>(type: "int", nullable: false),
                    testID = table.Column<int>(type: "int", nullable: false),
                    workorderid = table.Column<string>(type: "varchar(15)", maxLength: 15, nullable: false),
                    MacReading = table.Column<string>(type: "varchar(15)", maxLength: 15, nullable: false),
                    MacID = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    LabObservationName = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    InvestigationName = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    RerunReason = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    Rerunbyid = table.Column<int>(type: "int", nullable: false),
                    RerunDate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_machineRerunTestDetail", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "indentDetail",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    indentId = table.Column<int>(type: "int", nullable: false),
                    itemId = table.Column<int>(type: "int", nullable: false),
                    itemName = table.Column<string>(type: "longtext", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    IssuedQuantity = table.Column<int>(type: "int", nullable: false),
                    isActive = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    createdById = table.Column<int>(type: "int", nullable: true),
                    createdDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    updateById = table.Column<int>(type: "int", nullable: true),
                    updateDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_indentDetail", x => x.id);
                    table.ForeignKey(
                        name: "FK_indentDetail_Indent_indentId",
                        column: x => x.indentId,
                        principalTable: "Indent",
                        principalColumn: "indentId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_indentDetail_indentId",
                table: "indentDetail",
                column: "indentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "indentDetail");

            migrationBuilder.DropTable(
                name: "ItemMasterStore");

            migrationBuilder.DropTable(
                name: "machineRerunTestDetail");

            migrationBuilder.DropTable(
                name: "Indent");

            migrationBuilder.CreateTable(
                name: "VendorMaster",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Address = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false),
                    ContactNo = table.Column<string>(type: "varchar(15)", maxLength: 15, nullable: false),
                    city = table.Column<int>(type: "int", nullable: false),
                    contactPerson = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    country = table.Column<int>(type: "int", nullable: false),
                    createdById = table.Column<int>(type: "int", nullable: true),
                    createdDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    emailId = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    faxNo = table.Column<string>(type: "varchar(15)", maxLength: 15, nullable: false),
                    isActive = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    landline = table.Column<string>(type: "varchar(15)", maxLength: 15, nullable: false),
                    pinCode = table.Column<string>(type: "varchar(6)", maxLength: 6, nullable: false),
                    state = table.Column<int>(type: "int", nullable: false),
                    supplierCategory = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    supplierCode = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    supplierName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    supplierType = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    updateById = table.Column<int>(type: "int", nullable: true),
                    updateDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    website = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VendorMaster", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");
        }
    }
}
