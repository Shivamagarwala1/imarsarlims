using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace iMARSARLIMS.Migrations
{
    /// <inheritdoc />
    public partial class _2012 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "tempPassword",
                table: "empMaster",
                type: "varchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "ThemeColour",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    headerColor = table.Column<string>(type: "longtext", nullable: false),
                    menuColor = table.Column<string>(type: "longtext", nullable: false),
                    subMenuColor = table.Column<string>(type: "longtext", nullable: false),
                    textColor = table.Column<string>(type: "longtext", nullable: false),
                    blockColor = table.Column<string>(type: "longtext", nullable: false),
                    isdefault = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThemeColour", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ThemeColour");

            migrationBuilder.DropColumn(
                name: "tempPassword",
                table: "empMaster");
        }
    }
}
