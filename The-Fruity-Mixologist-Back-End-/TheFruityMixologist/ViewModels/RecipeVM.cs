using System.ComponentModel.DataAnnotations.Schema;
using TheFruityMixologist.Entities;

namespace TheFruityMixologist.ViewModels
{
    public class RecipeVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public decimal? DiscountPrice { get; set; }
        public string Desc { get; set; }

        [NotMapped]
        public ICollection<int> CategoryIds { get; set; } = null!;
        [NotMapped]
        public IFormFile? MainPhoto { get; set; }

        [NotMapped]
        public IFormFile? FalseImages { get; set; }
        [NotMapped]

        public ICollection<RecipesImages>? AllImages { get; set; }
        [NotMapped]

        public ICollection<int>? ImagesId { get; set; }
        public string?  Steps { get; set; }
        public List<Step>? AllSteps { get; set; }
        [NotMapped]
        public List<int>? DeleteSteps { get; set; }
        public string? Ingredients { get; set; }
        public List<Ingredient>? AllIngredient { get; set; }
        [NotMapped]

        public List<int>? DeleteIngredients { get; set; }



        public RecipeVM()
        {
            AllSteps = new();
            AllIngredient = new();
            DeleteSteps = new List<int>();
            DeleteIngredients = new List<int>();
        }


    }
}
