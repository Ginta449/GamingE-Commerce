using Microsoft.EntityFrameworkCore.Migrations;

namespace GamingE_Commerce.Data.Migrations
{
    public partial class spellingErrors : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PulbisherName",
                table: "Publishers");

            migrationBuilder.DropColumn(
                name: "PlatfromName",
                table: "Platforms");

            migrationBuilder.AddColumn<string>(
                name: "PublisherName",
                table: "Publishers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PlatformName",
                table: "Platforms",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PublisherName",
                table: "Publishers");

            migrationBuilder.DropColumn(
                name: "PlatformName",
                table: "Platforms");

            migrationBuilder.AddColumn<string>(
                name: "PulbisherName",
                table: "Publishers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PlatfromName",
                table: "Platforms",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
