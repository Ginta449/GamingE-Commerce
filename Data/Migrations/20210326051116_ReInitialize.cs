using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GamingE_Commerce.Data.Migrations
{
    public partial class ReInitialize : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageName",
                table: "Game");

            migrationBuilder.AddColumn<string>(
                name: "MyProperty",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfileImageName",
                table: "AspNetUsers",
                nullable: true);

            //migrationBuilder.CreateTable(
            //    name: "Orders",
            //    columns: table => new
            //    {
            //        OrderId = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        OrderName = table.Column<string>(nullable: true),
            //        OrdrDate = table.Column<DateTime>(nullable: false),
            //        PaymentType = table.Column<string>(nullable: true),
            //        Status = table.Column<string>(nullable: true),
            //        FirstName = table.Column<string>(nullable: true),
            //        LastName = table.Column<string>(nullable: true),
            //        Address1 = table.Column<string>(nullable: true),
            //        Address2 = table.Column<string>(nullable: true),
            //        County = table.Column<string>(nullable: true),
            //        Town = table.Column<string>(nullable: true),
            //        Eircode = table.Column<string>(nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Orders", x => x.OrderId);
            //    });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropColumn(
                name: "MyProperty",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ProfileImageName",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "ImageName",
                table: "Game",
                type: "nvarchar(100)",
                nullable: true);
        }
    }
}
