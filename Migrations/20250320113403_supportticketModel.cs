using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace iMARSARLIMS.Migrations
{
    /// <inheritdoc />
    public partial class supportticketModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "supportTicket",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    clientId = table.Column<int>(type: "int", nullable: false),
                    clientName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    taskTypeName = table.Column<string>(type: "varchar(40)", maxLength: 40, nullable: false),
                    taskTypeId = table.Column<int>(type: "int", nullable: false),
                    task = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false),
                    assignedTo = table.Column<int>(type: "int", nullable: false),
                    AssignedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CompletedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Deliverydate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ActionTaken = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_supportTicket", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "supportTicket");
        }
    }
}
