using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace iMARSARLIMS.Migrations
{
    /// <inheritdoc />
    public partial class ticketSupport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Document",
                table: "supportTicket",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext");

            migrationBuilder.AddColumn<string>(
                name: "CompleteRemark",
                table: "supportTicket",
                type: "varchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "closedBy",
                table: "supportTicket",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "closedDate",
                table: "supportTicket",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "closedRemark",
                table: "supportTicket",
                type: "varchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "holdBy",
                table: "supportTicket",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "holdDate",
                table: "supportTicket",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "holdReason",
                table: "supportTicket",
                type: "varchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "isClosed",
                table: "supportTicket",
                type: "tinyint unsigned",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "isHold",
                table: "supportTicket",
                type: "tinyint unsigned",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "isRejected",
                table: "supportTicket",
                type: "tinyint unsigned",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "rejectDate",
                table: "supportTicket",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "rejectReason",
                table: "supportTicket",
                type: "varchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "rejectedBy",
                table: "supportTicket",
                type: "int",
                nullable: true);

            

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompleteRemark",
                table: "supportTicket");

            migrationBuilder.DropColumn(
                name: "closedBy",
                table: "supportTicket");

            migrationBuilder.DropColumn(
                name: "closedDate",
                table: "supportTicket");

            migrationBuilder.DropColumn(
                name: "closedRemark",
                table: "supportTicket");

            migrationBuilder.DropColumn(
                name: "holdBy",
                table: "supportTicket");

            migrationBuilder.DropColumn(
                name: "holdDate",
                table: "supportTicket");

            migrationBuilder.DropColumn(
                name: "holdReason",
                table: "supportTicket");

            migrationBuilder.DropColumn(
                name: "isClosed",
                table: "supportTicket");

            migrationBuilder.DropColumn(
                name: "isHold",
                table: "supportTicket");

            migrationBuilder.DropColumn(
                name: "isRejected",
                table: "supportTicket");

            migrationBuilder.DropColumn(
                name: "rejectDate",
                table: "supportTicket");

            migrationBuilder.DropColumn(
                name: "rejectReason",
                table: "supportTicket");

            migrationBuilder.DropColumn(
                name: "rejectedBy",
                table: "supportTicket");

            

            migrationBuilder.AlterColumn<string>(
                name: "Document",
                table: "supportTicket",
                type: "longtext",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true);
        }
    }
}
