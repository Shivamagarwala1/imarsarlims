using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace iMARSARLIMS.Migrations
{
    /// <inheritdoc />
    public partial class CentrecertiGen : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "workOrderId",
                table: "machine_result",
                type: "varchar(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "status",
                table: "machine_result",
                type: "varchar(10)",
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "patientName",
                table: "machine_result",
                type: "varchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "machineName3",
                table: "machine_result",
                type: "varchar(30)",
                maxLength: 30,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "machineName2",
                table: "machine_result",
                type: "varchar(30)",
                maxLength: 30,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "machineName1",
                table: "machine_result",
                type: "varchar(30)",
                maxLength: 30,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "machineComments",
                table: "machine_result",
                type: "varchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "macReading1",
                table: "machine_result",
                type: "varchar(30)",
                maxLength: 30,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "labObservationName",
                table: "machine_result",
                type: "varchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "investigationName",
                table: "machine_result",
                type: "varchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "gender",
                table: "machine_result",
                type: "varchar(10)",
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "barcodeNo",
                table: "machine_result",
                type: "varchar(15)",
                maxLength: 15,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MacReading3",
                table: "machine_result",
                type: "varchar(30)",
                maxLength: 30,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MacReading2",
                table: "machine_result",
                type: "varchar(30)",
                maxLength: 30,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "CentreCertificate",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    centreId = table.Column<int>(type: "int", nullable: false),
                    centreName = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
                    certificateDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    isActive = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    createdById = table.Column<int>(type: "int", nullable: true),
                    createdDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    updateById = table.Column<int>(type: "int", nullable: true),
                    updateDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CentreCertificate", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CentreCertificate");

            migrationBuilder.AlterColumn<string>(
                name: "workOrderId",
                table: "machine_result",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "status",
                table: "machine_result",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(10)",
                oldMaxLength: 10,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "patientName",
                table: "machine_result",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "machineName3",
                table: "machine_result",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(30)",
                oldMaxLength: 30,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "machineName2",
                table: "machine_result",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(30)",
                oldMaxLength: 30,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "machineName1",
                table: "machine_result",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(30)",
                oldMaxLength: 30,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "machineComments",
                table: "machine_result",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "macReading1",
                table: "machine_result",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(30)",
                oldMaxLength: 30,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "labObservationName",
                table: "machine_result",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "investigationName",
                table: "machine_result",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "gender",
                table: "machine_result",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(10)",
                oldMaxLength: 10,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "barcodeNo",
                table: "machine_result",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(15)",
                oldMaxLength: 15,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MacReading3",
                table: "machine_result",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(30)",
                oldMaxLength: 30,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MacReading2",
                table: "machine_result",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(30)",
                oldMaxLength: 30,
                oldNullable: true);
        }
    }
}
