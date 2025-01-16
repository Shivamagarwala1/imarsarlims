using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace iMARSARLIMS.Migrations
{
    /// <inheritdoc />
    public partial class itemmasteredit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            

            migrationBuilder.AddColumn<int>(
                name: "itemId",
                table: "itemSampleTypeMapping",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_itemSampleTypeMapping_itemId",
                table: "itemSampleTypeMapping",
                column: "itemId");

            migrationBuilder.AddForeignKey(
                name: "FK_itemSampleTypeMapping_itemMaster_itemId",
                table: "itemSampleTypeMapping",
                column: "itemId",
                principalTable: "itemMaster",
                principalColumn: "itemId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_itemSampleTypeMapping_itemMaster_itemId",
                table: "itemSampleTypeMapping");

            migrationBuilder.DropIndex(
                name: "IX_itemSampleTypeMapping_itemId",
                table: "itemSampleTypeMapping");

            migrationBuilder.DropColumn(
                name: "itemId",
                table: "itemSampleTypeMapping");

           
        }
    }
}
