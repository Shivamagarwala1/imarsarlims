using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace iMARSARLIMS.Migrations
{
    /// <inheritdoc />
    public partial class colourmas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "contantName",
                table: "LegendColorMaster",
                type: "longtext",
                nullable: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "contantName",
                table: "LegendColorMaster");
        }
    }
}
