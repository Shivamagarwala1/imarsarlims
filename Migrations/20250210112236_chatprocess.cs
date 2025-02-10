using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace iMARSARLIMS.Migrations
{
    /// <inheritdoc />
    public partial class chatprocess : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "chatGroupMaster",
                columns: table => new
                {
                    groupMasterId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    groupMasterName = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    isActive = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    createdById = table.Column<int>(type: "int", nullable: true),
                    createdDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP(6)"),
                    updateById = table.Column<int>(type: "int", nullable: true),
                    updateDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP(6)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_chatGroupMaster", x => x.groupMasterId);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "chatGroupMasterEmployee",
                columns: table => new
                {
                    groupMasterEmployeeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    groupMasterId = table.Column<int>(type: "int", nullable: false),
                    empId = table.Column<int>(type: "int", nullable: false),
                    isActive = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    createdById = table.Column<int>(type: "int", nullable: true),
                    createdDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP(6)"),
                    updateById = table.Column<int>(type: "int", nullable: true),
                    updateDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP(6)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_chatGroupMasterEmployee", x => x.groupMasterEmployeeId);
                    table.ForeignKey(
                        name: "FK_chatGroupMasterEmployee_chatGroupMaster_groupMasterId",
                        column: x => x.groupMasterId,
                        principalTable: "chatGroupMaster",
                        principalColumn: "groupMasterId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_chatGroupMasterEmployee_empMaster_empId",
                        column: x => x.empId,
                        principalTable: "empMaster",
                        principalColumn: "empId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "chatMessage",
                columns: table => new
                {
                    messageId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    content = table.Column<string>(type: "varchar(2000)", maxLength: 2000, nullable: false),
                    isSeen = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    empId = table.Column<int>(type: "int", nullable: true),
                    groupMasterId = table.Column<int>(type: "int", nullable: true),
                    fileName = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true),
                    fileUrl = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true),
                    isActive = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    createdById = table.Column<int>(type: "int", nullable: true),
                    createdDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP(6)"),
                    updateById = table.Column<int>(type: "int", nullable: true),
                    updateDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP(6)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_chatMessage", x => x.messageId);
                    table.ForeignKey(
                        name: "FK_chatMessage_chatGroupMaster_groupMasterId",
                        column: x => x.groupMasterId,
                        principalTable: "chatGroupMaster",
                        principalColumn: "groupMasterId");
                    table.ForeignKey(
                        name: "FK_chatMessage_empMaster_empId",
                        column: x => x.empId,
                        principalTable: "empMaster",
                        principalColumn: "empId");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_chatGroupMasterEmployee_empId",
                table: "chatGroupMasterEmployee",
                column: "empId");

            migrationBuilder.CreateIndex(
                name: "IX_chatGroupMasterEmployee_groupMasterId",
                table: "chatGroupMasterEmployee",
                column: "groupMasterId");

            migrationBuilder.CreateIndex(
                name: "IX_chatMessage_empId",
                table: "chatMessage",
                column: "empId");

            migrationBuilder.CreateIndex(
                name: "IX_chatMessage_groupMasterId",
                table: "chatMessage",
                column: "groupMasterId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "chatGroupMasterEmployee");

            migrationBuilder.DropTable(
                name: "chatMessage");

            migrationBuilder.DropTable(
                name: "chatGroupMaster");
        }
    }
}
