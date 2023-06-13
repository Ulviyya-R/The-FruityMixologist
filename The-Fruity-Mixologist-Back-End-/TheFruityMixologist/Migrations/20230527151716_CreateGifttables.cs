using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheFruityMixologist.Migrations
{
    public partial class CreateGifttables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GifCarts_giftCartColor_GiftCartColorId",
                table: "GifCarts");

            migrationBuilder.DropForeignKey(
                name: "FK_GifCarts_PriceOption_PriceOptionId",
                table: "GifCarts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PriceOption",
                table: "PriceOption");

            migrationBuilder.DropPrimaryKey(
                name: "PK_giftCartColor",
                table: "giftCartColor");

            migrationBuilder.RenameTable(
                name: "PriceOption",
                newName: "priceOptions");

            migrationBuilder.RenameTable(
                name: "giftCartColor",
                newName: "giftCartColors");

            migrationBuilder.AddPrimaryKey(
                name: "PK_priceOptions",
                table: "priceOptions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_giftCartColors",
                table: "giftCartColors",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GifCarts_giftCartColors_GiftCartColorId",
                table: "GifCarts",
                column: "GiftCartColorId",
                principalTable: "giftCartColors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GifCarts_priceOptions_PriceOptionId",
                table: "GifCarts",
                column: "PriceOptionId",
                principalTable: "priceOptions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GifCarts_giftCartColors_GiftCartColorId",
                table: "GifCarts");

            migrationBuilder.DropForeignKey(
                name: "FK_GifCarts_priceOptions_PriceOptionId",
                table: "GifCarts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_priceOptions",
                table: "priceOptions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_giftCartColors",
                table: "giftCartColors");

            migrationBuilder.RenameTable(
                name: "priceOptions",
                newName: "PriceOption");

            migrationBuilder.RenameTable(
                name: "giftCartColors",
                newName: "giftCartColor");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PriceOption",
                table: "PriceOption",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_giftCartColor",
                table: "giftCartColor",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GifCarts_giftCartColor_GiftCartColorId",
                table: "GifCarts",
                column: "GiftCartColorId",
                principalTable: "giftCartColor",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GifCarts_PriceOption_PriceOptionId",
                table: "GifCarts",
                column: "PriceOptionId",
                principalTable: "PriceOption",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
