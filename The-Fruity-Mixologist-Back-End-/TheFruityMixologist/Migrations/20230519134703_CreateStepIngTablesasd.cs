using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheFruityMixologist.Migrations
{
    public partial class CreateStepIngTablesasd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecipesImages_Recipes_RecipesId",
                table: "RecipesImages");

            migrationBuilder.DropColumn(
                name: "Compari",
                table: "Ingredients");

            migrationBuilder.DropColumn(
                name: "GratedCinnamon",
                table: "Ingredients");

            migrationBuilder.DropColumn(
                name: "Prosecco",
                table: "Ingredients");

            migrationBuilder.DropColumn(
                name: "SeasonalFruit",
                table: "Ingredients");

            migrationBuilder.RenameColumn(
                name: "RecipesId",
                table: "RecipesImages",
                newName: "RecipeId");

            migrationBuilder.RenameIndex(
                name: "IX_RecipesImages_RecipesId",
                table: "RecipesImages",
                newName: "IX_RecipesImages_RecipeId");

            migrationBuilder.RenameColumn(
                name: "SweetVermouth",
                table: "Ingredients",
                newName: "Ingredients");

            migrationBuilder.AddForeignKey(
                name: "FK_RecipesImages_Recipes_RecipeId",
                table: "RecipesImages",
                column: "RecipeId",
                principalTable: "Recipes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecipesImages_Recipes_RecipeId",
                table: "RecipesImages");

            migrationBuilder.RenameColumn(
                name: "RecipeId",
                table: "RecipesImages",
                newName: "RecipesId");

            migrationBuilder.RenameIndex(
                name: "IX_RecipesImages_RecipeId",
                table: "RecipesImages",
                newName: "IX_RecipesImages_RecipesId");

            migrationBuilder.RenameColumn(
                name: "Ingredients",
                table: "Ingredients",
                newName: "SweetVermouth");

            migrationBuilder.AddColumn<string>(
                name: "Compari",
                table: "Ingredients",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "GratedCinnamon",
                table: "Ingredients",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Prosecco",
                table: "Ingredients",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SeasonalFruit",
                table: "Ingredients",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_RecipesImages_Recipes_RecipesId",
                table: "RecipesImages",
                column: "RecipesId",
                principalTable: "Recipes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
