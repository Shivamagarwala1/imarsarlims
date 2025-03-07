using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace iMARSARLIMS.Migrations
{
    /// <inheritdoc />
    public partial class outsource11 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Companyid",
                table: "tnx_outhousedeatils");

            migrationBuilder.RenameColumn(
                name: "OutsouceDate",
                table: "tnx_outhousedeatils",
                newName: "outHouceDate");

            migrationBuilder.AddColumn<int>(
                name: "DLCCheck",
                table: "ResultEntryResponseModle",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "TatReportData",
                columns: table => new
                {
                    BookingDate = table.Column<string>(type: "longtext", nullable: true),
                    PatientName = table.Column<string>(type: "longtext", nullable: true),
                    itemId = table.Column<int>(type: "int", nullable: false),
                    TestName = table.Column<string>(type: "longtext", nullable: true),
                    RefDoctor = table.Column<string>(type: "longtext", nullable: true),
                    Department = table.Column<string>(type: "longtext", nullable: true),
                    DeptId = table.Column<int>(type: "int", nullable: false),
                    centreId = table.Column<int>(type: "int", nullable: false),
                    CentreCode = table.Column<string>(type: "longtext", nullable: true),
                    centreName = table.Column<string>(type: "longtext", nullable: true),
                    WorkorderId = table.Column<string>(type: "longtext", nullable: true),
                    SampleCollectionDate = table.Column<string>(type: "longtext", nullable: true),
                    SampleReceivedDate = table.Column<string>(type: "longtext", nullable: true),
                    ResultDate = table.Column<string>(type: "longtext", nullable: true),
                    ApproveDate = table.Column<string>(type: "longtext", nullable: true),
                    BTOS = table.Column<int>(type: "int", nullable: false),
                    STOD = table.Column<int>(type: "int", nullable: false),
                    DTOR = table.Column<int>(type: "int", nullable: false),
                    RTOA = table.Column<int>(type: "int", nullable: false),
                    BTOA = table.Column<int>(type: "int", nullable: false),
                    DeliveryTime = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tnx_OutsourceDetail",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    itemId = table.Column<int>(type: "int", nullable: true),
                    itemName = table.Column<string>(type: "longtext", nullable: true),
                    centreId = table.Column<int>(type: "int", nullable: true),
                    transactionId = table.Column<int>(type: "int", nullable: true),
                    workOrderId = table.Column<string>(type: "longtext", nullable: true),
                    bookingRate = table.Column<double>(type: "double", nullable: true),
                    outSourceRate = table.Column<double>(type: "double", nullable: true),
                    outSourceLabID = table.Column<int>(type: "int", nullable: true),
                    outSourceLabName = table.Column<string>(type: "longtext", nullable: true),
                    outSouceDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    sentByid = table.Column<int>(type: "int", nullable: true),
                    SentName = table.Column<string>(type: "longtext", nullable: true),
                    remarks = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tnx_OutsourceDetail", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TatReportData");

            migrationBuilder.DropTable(
                name: "tnx_OutsourceDetail");

            migrationBuilder.DropColumn(
                name: "DLCCheck",
                table: "ResultEntryResponseModle");

            migrationBuilder.RenameColumn(
                name: "outHouceDate",
                table: "tnx_outhousedeatils",
                newName: "OutsouceDate");

            migrationBuilder.AddColumn<int>(
                name: "Companyid",
                table: "tnx_outhousedeatils",
                type: "int",
                nullable: true);
        }
    }
}
