using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheFruityMixologist.Migrations
{
    public partial class addStatusColumngiftcart : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "status",
                table: "GifCarts",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "status",
                table: "GifCarts");
        }
    }
}
