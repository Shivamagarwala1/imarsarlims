using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace iMARSARLIMS.Migrations
{
    /// <inheritdoc />
    public partial class nablimagedata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NablImage",
                table: "centreMaster",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "waterMarkImage",
                table: "centreMaster",
                type: "longtext",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NablImage",
                table: "centreMaster");

            migrationBuilder.DropColumn(
                name: "waterMarkImage",
                table: "centreMaster");
        }
    }
}
