using System.ComponentModel.DataAnnotations.Schema;
using TheFruityMixologist.ViewModels;

namespace TheFruityMixologist.Entities
{
    public class Recipe : BaseEntity
    {
        public string Name { get; set; }
        public string? Desc { get; set; }
        public decimal Price { get; set; }
        public decimal DiscountPrice { get; set; }
        public bool IsCreatingUserCock { get; set; }

        [NotMapped]
        public AddCartVM AddCart { get; set; }

        public List<RecipesImages> RecipesImages { get; set; }
        public List<RecipesCategory> RecipesCategories { get; set; }
        public List<Ingredient> Ingredients { get; set; }
        public double? PointReyting { get; set; }
        public  List<Step> Steps { get; set; }
        public List<Comment>? RecipeComments { get; set; }
        public List<BasketItem> BasketItem { get; set; }
        public List<Createcocktail>? CreateCocktails { get; set; }
        public List<WishlistItem>? WishListItems { get; set; }


        public Recipe()
        {
            WishListItems = new();
            RecipesImages = new();
            RecipesCategories = new();
            Ingredients = new();
            RecipeComments = new();
        }

    }
}
