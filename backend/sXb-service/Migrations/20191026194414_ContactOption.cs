using Microsoft.EntityFrameworkCore.Migrations;

namespace sXb_service.Migrations
{
    public partial class ContactOption : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ContactOption",
                table: "Listings",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContactOption",
                table: "Listings");
        }
    }
}
