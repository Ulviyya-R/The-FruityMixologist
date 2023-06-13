using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheFruityMixologist.Migrations
{
    public partial class addcolumnWishlisttable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isLiked",
                table: "WishlistItems",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isLiked",
                table: "WishlistItems");
        }
    }
}
