using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace iMARSARLIMS.Migrations
{
    /// <inheritdoc />
    public partial class legendsclr1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_tnx_ReceiptDetails_transactionId",
                table: "tnx_ReceiptDetails",
                column: "transactionId");

            migrationBuilder.AddForeignKey(
                name: "FK_tnx_ReceiptDetails_tnx_Booking_transactionId",
                table: "tnx_ReceiptDetails",
                column: "transactionId",
                principalTable: "tnx_Booking",
                principalColumn: "transactionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tnx_ReceiptDetails_tnx_Booking_transactionId",
                table: "tnx_ReceiptDetails");

            migrationBuilder.DropIndex(
                name: "IX_tnx_ReceiptDetails_transactionId",
                table: "tnx_ReceiptDetails");
        }
    }
}
