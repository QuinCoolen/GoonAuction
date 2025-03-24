using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GoonAuctionDAL.Migrations
{
    /// <inheritdoc />
    public partial class RemoveUserIdFromAuction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Auctions_AspNetUsers_UserId",
                table: "Auctions");

            migrationBuilder.DropForeignKey(
                name: "FK_Bids_AspNetUsers_UserId",
                table: "Bids");

            migrationBuilder.DropIndex(
                name: "IX_Bids_UserId",
                table: "Bids");

            migrationBuilder.DropIndex(
                name: "IX_Auctions_UserId",
                table: "Auctions");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Bids");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Auctions");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Bids",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Auctions",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Bids_UserId",
                table: "Bids",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Auctions_UserId",
                table: "Auctions",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Auctions_AspNetUsers_UserId",
                table: "Auctions",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Bids_AspNetUsers_UserId",
                table: "Bids",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
