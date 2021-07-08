using Microsoft.EntityFrameworkCore.Migrations;

namespace GamingE_Commerce.Data.Migrations
{
    public partial class SearchBar : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Search",
                table: "Game",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Search",
                table: "Game");
        }
    }
}
