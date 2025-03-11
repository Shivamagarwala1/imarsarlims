using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace iMARSARLIMS.Migrations
{
    /// <inheritdoc />
    public partial class CentrecertiGen1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Aggreement",
                table: "centreMaster",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CertificateImage",
                table: "centreMaster",
                type: "longtext",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Aggreement",
                table: "centreMaster");

            migrationBuilder.DropColumn(
                name: "CertificateImage",
                table: "centreMaster");
        }
    }
}
