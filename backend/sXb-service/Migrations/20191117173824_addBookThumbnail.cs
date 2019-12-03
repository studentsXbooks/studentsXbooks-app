using Microsoft.EntityFrameworkCore.Migrations;

namespace sXb_service.Migrations
{
    public partial class addBookThumbnail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SmallThumbnail",
                table: "Books",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Thumbnail",
                table: "Books",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SmallThumbnail",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "Thumbnail",
                table: "Books");
        }
    }
}
