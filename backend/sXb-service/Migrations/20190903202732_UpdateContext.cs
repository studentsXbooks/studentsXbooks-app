using Microsoft.EntityFrameworkCore.Migrations;

namespace sXb_service.Migrations
{
    public partial class UpdateContext : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_UserBooks_BookId",
                table: "UserBooks",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_Listings_UserBookId",
                table: "Listings",
                column: "UserBookId");

            migrationBuilder.AddForeignKey(
                name: "FK_Listings_UserBooks_UserBookId",
                table: "Listings",
                column: "UserBookId",
                principalTable: "UserBooks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserBooks_Books_BookId",
                table: "UserBooks",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Listings_UserBooks_UserBookId",
                table: "Listings");

            migrationBuilder.DropForeignKey(
                name: "FK_UserBooks_Books_BookId",
                table: "UserBooks");

            migrationBuilder.DropIndex(
                name: "IX_UserBooks_BookId",
                table: "UserBooks");

            migrationBuilder.DropIndex(
                name: "IX_Listings_UserBookId",
                table: "Listings");
        }
    }
}
