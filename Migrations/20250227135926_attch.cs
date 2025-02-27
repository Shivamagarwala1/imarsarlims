using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace iMARSARLIMS.Migrations
{
    /// <inheritdoc />
    public partial class attch : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "oldreading",
                table: "ResultEntryResponseModle",
                type: "longtext",
                nullable: false);

            migrationBuilder.AddColumn<int>(
                name: "empid",
                table: "DoctorApprovalSign",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "tnx_InvestigationAttchment",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    testId = table.Column<int>(type: "int", nullable: false),
                    Attachment = table.Column<string>(type: "longtext", nullable: false),
                    isactive = table.Column<int>(type: "int", nullable: false),
                    createdById = table.Column<int>(type: "int", nullable: false),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tnx_InvestigationAttchment", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tnx_InvestigationAttchment");

            migrationBuilder.DropColumn(
                name: "oldreading",
                table: "ResultEntryResponseModle");

            migrationBuilder.DropColumn(
                name: "empid",
                table: "DoctorApprovalSign");
        }
    }
}
