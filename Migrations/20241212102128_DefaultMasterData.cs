using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace iMARSARLIMS.Migrations
{
    /// <inheritdoc />
    public partial class DefaultMasterData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@" INSERT INTO MiscMaster ( misc_name, type, is_active, created_by, created_datetime)
            VALUES
            ('Mr.', 'Title',  1, 1, GETDATE()),
            ('Mrs.', 'Title',  1, 1, GETDATE()),
            ('Dr.', 'Title', 1, 1, GETDATE());");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
