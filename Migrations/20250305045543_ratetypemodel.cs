using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace iMARSARLIMS.Migrations
{
    /// <inheritdoc />
    public partial class ratetypemodel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "panelId",
                table: "rateTypeWiseRateList");

            migrationBuilder.DropColumn(
                name: "panelItemName",
                table: "rateTypeWiseRateList");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "panelId",
                table: "rateTypeWiseRateList",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "panelItemName",
                table: "rateTypeWiseRateList",
                type: "longtext",
                nullable: true);
        }
    }
}
