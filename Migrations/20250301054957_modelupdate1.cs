using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace iMARSARLIMS.Migrations
{
    /// <inheritdoc />
    public partial class modelupdate1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "isactive",
                table: "tnx_InvestigationAttchment",
                newName: "isActive");

            migrationBuilder.RenameColumn(
                name: "CreatedDateTime",
                table: "tnx_InvestigationAttchment",
                newName: "createdDateTime");

            migrationBuilder.AlterColumn<byte>(
                name: "isActive",
                table: "tnx_InvestigationAttchment",
                type: "tinyint unsigned",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "createdById",
                table: "tnx_InvestigationAttchment",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Attachment",
                table: "tnx_InvestigationAttchment",
                type: "varchar(300)",
                maxLength: 300,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext");

            migrationBuilder.AddColumn<int>(
                name: "updateById",
                table: "tnx_InvestigationAttchment",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "updateDateTime",
                table: "tnx_InvestigationAttchment",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "tnx_InvestigationAddReport",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    testId = table.Column<int>(type: "int", nullable: false),
                    Attachment = table.Column<string>(type: "varchar(300)", maxLength: 300, nullable: false),
                    isActive = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    createdById = table.Column<int>(type: "int", nullable: true),
                    createdDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    updateById = table.Column<int>(type: "int", nullable: true),
                    updateDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tnx_InvestigationAddReport", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tnx_InvestigationAddReport");

            migrationBuilder.DropColumn(
                name: "updateById",
                table: "tnx_InvestigationAttchment");

            migrationBuilder.DropColumn(
                name: "updateDateTime",
                table: "tnx_InvestigationAttchment");

            migrationBuilder.RenameColumn(
                name: "isActive",
                table: "tnx_InvestigationAttchment",
                newName: "isactive");

            migrationBuilder.RenameColumn(
                name: "createdDateTime",
                table: "tnx_InvestigationAttchment",
                newName: "CreatedDateTime");

            migrationBuilder.AlterColumn<int>(
                name: "isactive",
                table: "tnx_InvestigationAttchment",
                type: "int",
                nullable: false,
                oldClrType: typeof(byte),
                oldType: "tinyint unsigned");

            migrationBuilder.AlterColumn<int>(
                name: "createdById",
                table: "tnx_InvestigationAttchment",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Attachment",
                table: "tnx_InvestigationAttchment",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(300)",
                oldMaxLength: 300);
        }
    }
}
