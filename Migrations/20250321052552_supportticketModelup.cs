using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace iMARSARLIMS.Migrations
{
    /// <inheritdoc />
    public partial class supportticketModelup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "taskTypeName",
                table: "supportTicket");

            migrationBuilder.RenameColumn(
                name: "taskTypeId",
                table: "supportTicket",
                newName: "ticketTypeId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Deliverydate",
                table: "supportTicket",
                type: "datetime(6)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CompletedDate",
                table: "supportTicket",
                type: "datetime(6)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "AssignedDate",
                table: "supportTicket",
                type: "datetime(6)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");

            migrationBuilder.AlterColumn<string>(
                name: "ActionTaken",
                table: "supportTicket",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AddColumn<int>(
                name: "AssignedBy",
                table: "supportTicket",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateDate",
                table: "supportTicket",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Document",
                table: "supportTicket",
                type: "longtext",
                nullable: false);

            migrationBuilder.AddColumn<int>(
                name: "Priority",
                table: "supportTicket",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ReopenBy",
                table: "supportTicket",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "ReopenDate",
                table: "supportTicket",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReopenReason",
                table: "supportTicket",
                type: "varchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "completedBy",
                table: "supportTicket",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<byte>(
                name: "isAssigned",
                table: "supportTicket",
                type: "tinyint unsigned",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<byte>(
                name: "isCompleted",
                table: "supportTicket",
                type: "tinyint unsigned",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<byte>(
                name: "isReopen",
                table: "supportTicket",
                type: "tinyint unsigned",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.CreateTable(
                name: "SupportTicketType",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    ticketType = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    isActive = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    createdById = table.Column<int>(type: "int", nullable: true),
                    createdDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    updateById = table.Column<int>(type: "int", nullable: true),
                    updateDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupportTicketType", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SupportTicketType");

            migrationBuilder.DropColumn(
                name: "AssignedBy",
                table: "supportTicket");

            migrationBuilder.DropColumn(
                name: "CreateDate",
                table: "supportTicket");

            migrationBuilder.DropColumn(
                name: "Document",
                table: "supportTicket");

            migrationBuilder.DropColumn(
                name: "Priority",
                table: "supportTicket");

            migrationBuilder.DropColumn(
                name: "ReopenBy",
                table: "supportTicket");

            migrationBuilder.DropColumn(
                name: "ReopenDate",
                table: "supportTicket");

            migrationBuilder.DropColumn(
                name: "ReopenReason",
                table: "supportTicket");

            migrationBuilder.DropColumn(
                name: "completedBy",
                table: "supportTicket");

            migrationBuilder.DropColumn(
                name: "isAssigned",
                table: "supportTicket");

            migrationBuilder.DropColumn(
                name: "isCompleted",
                table: "supportTicket");

            migrationBuilder.DropColumn(
                name: "isReopen",
                table: "supportTicket");

            migrationBuilder.RenameColumn(
                name: "ticketTypeId",
                table: "supportTicket",
                newName: "taskTypeId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Deliverydate",
                table: "supportTicket",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CompletedDate",
                table: "supportTicket",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "AssignedDate",
                table: "supportTicket",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ActionTaken",
                table: "supportTicket",
                type: "varchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AddColumn<string>(
                name: "taskTypeName",
                table: "supportTicket",
                type: "varchar(40)",
                maxLength: 40,
                nullable: false,
                defaultValue: "");
        }
    }
}
