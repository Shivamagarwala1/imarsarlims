using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace iMARSARLIMS.Migrations
{
    /// <inheritdoc />
    public partial class tatmaster : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SampleremarkMaster",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    remark = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    isActive = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    createdById = table.Column<int>(type: "int", nullable: true),
                    createdDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    updateById = table.Column<int>(type: "int", nullable: true),
                    updateDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SampleremarkMaster", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tat_master",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    centreid = table.Column<int>(type: "int", nullable: true),
                    Deptid = table.Column<int>(type: "int", nullable: true),
                    itemid = table.Column<int>(type: "int", nullable: true),
                    StartTime = table.Column<TimeSpan>(type: "time(6)", nullable: true),
                    EndTime = table.Column<TimeSpan>(type: "time(6)", nullable: true),
                    Mins = table.Column<int>(type: "int", nullable: true),
                    Days = table.Column<int>(type: "int", nullable: true),
                    Sun = table.Column<int>(type: "int", nullable: true),
                    Mon = table.Column<int>(type: "int", nullable: true),
                    Tue = table.Column<int>(type: "int", nullable: true),
                    Wed = table.Column<int>(type: "int", nullable: true),
                    Thu = table.Column<int>(type: "int", nullable: true),
                    Fri = table.Column<int>(type: "int", nullable: true),
                    Sat = table.Column<int>(type: "int", nullable: true),
                    Regcoll = table.Column<int>(type: "int", nullable: true),
                    collrecv = table.Column<int>(type: "int", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Createdby = table.Column<string>(type: "longtext", nullable: true),
                    CreatedByName = table.Column<string>(type: "longtext", nullable: true),
                    TATType = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tat_master", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SampleremarkMaster");

            migrationBuilder.DropTable(
                name: "tat_master");
        }
    }
}
