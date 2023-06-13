using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheFruityMixologist.Migrations
{
    public partial class CreateGifcartTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "giftCartColor",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImagePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsSelected = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_giftCartColor", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PriceOption",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Amount = table.Column<decimal>(type: "decimal(6,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PriceOption", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GifCarts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RecipientName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeliveryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CardColor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(6,2)", nullable: false),
                    PriceOptionId = table.Column<int>(type: "int", nullable: false),
                    GiftCartColorId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GifCarts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GifCarts_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_GifCarts_giftCartColor_GiftCartColorId",
                        column: x => x.GiftCartColorId,
                        principalTable: "giftCartColor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GifCarts_PriceOption_PriceOptionId",
                        column: x => x.PriceOptionId,
                        principalTable: "PriceOption",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GifCarts_GiftCartColorId",
                table: "GifCarts",
                column: "GiftCartColorId");

            migrationBuilder.CreateIndex(
                name: "IX_GifCarts_PriceOptionId",
                table: "GifCarts",
                column: "PriceOptionId");

            migrationBuilder.CreateIndex(
                name: "IX_GifCarts_UserId",
                table: "GifCarts",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GifCarts");

            migrationBuilder.DropTable(
                name: "giftCartColor");

            migrationBuilder.DropTable(
                name: "PriceOption");
        }
    }
}
