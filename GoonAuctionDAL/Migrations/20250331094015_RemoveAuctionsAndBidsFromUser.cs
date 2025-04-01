using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GoonAuctionDAL.Migrations
{
    /// <inheritdoc />
    public partial class RemoveAuctionsAndBidsFromUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Auctions_AspNetUsers_ApplicationUserId",
                table: "Auctions");

            migrationBuilder.DropForeignKey(
                name: "FK_Bids_AspNetUsers_ApplicationUserId",
                table: "Bids");

            migrationBuilder.DropIndex(
                name: "IX_Bids_ApplicationUserId",
                table: "Bids");

            migrationBuilder.DropIndex(
                name: "IX_Auctions_ApplicationUserId",
                table: "Auctions");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Bids");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Auctions");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Bids",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Auctions",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Bids_ApplicationUserId",
                table: "Bids",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Auctions_ApplicationUserId",
                table: "Auctions",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Auctions_AspNetUsers_ApplicationUserId",
                table: "Auctions",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Bids_AspNetUsers_ApplicationUserId",
                table: "Bids",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
