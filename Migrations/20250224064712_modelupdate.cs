using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace iMARSARLIMS.Migrations
{
    /// <inheritdoc />
    public partial class modelupdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "customerID",
                table: "tnx_InvestigationRemarks");

            migrationBuilder.AlterColumn<byte>(
                name: "isActive",
                table: "tnx_InvestigationRemarks",
                type: "tinyint unsigned",
                nullable: false,
                defaultValue: (byte)0,
                oldClrType: typeof(byte),
                oldType: "tinyint unsigned",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WorkOrderId",
                table: "tnx_InvestigationRemarks",
                type: "longtext",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WorkOrderId",
                table: "tnx_InvestigationRemarks");

            migrationBuilder.AlterColumn<byte>(
                name: "isActive",
                table: "tnx_InvestigationRemarks",
                type: "tinyint unsigned",
                nullable: true,
                oldClrType: typeof(byte),
                oldType: "tinyint unsigned");

            migrationBuilder.AddColumn<int>(
                name: "customerID",
                table: "tnx_InvestigationRemarks",
                type: "int",
                nullable: true);
        }
    }
}
