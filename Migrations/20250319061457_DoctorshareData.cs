using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace iMARSARLIMS.Migrations
{
    /// <inheritdoc />
    public partial class DoctorshareData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "centreID",
                table: "doctorReferalMaster",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "doctorShareMaster",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Doctorid = table.Column<int>(type: "int", nullable: false),
                    deptid = table.Column<int>(type: "int", nullable: false),
                    ItemID = table.Column<int>(type: "int", nullable: false),
                    Centreid = table.Column<int>(type: "int", nullable: false),
                    percentage = table.Column<double>(type: "double", nullable: false),
                    Amount = table.Column<double>(type: "double", nullable: false),
                    type = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    CreatedBYID = table.Column<int>(type: "int", nullable: false),
                    createdbyName = table.Column<string>(type: "longtext", nullable: false),
                    createdDate = table.Column<byte[]>(type: "longblob", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_doctorShareMaster", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "doctorShareMaster");

            migrationBuilder.DropColumn(
                name: "centreID",
                table: "doctorReferalMaster");
        }
    }
}
