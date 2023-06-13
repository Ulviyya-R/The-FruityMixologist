namespace TheFruityMixologist.Entities
{
    public class RecipesCategory : BaseEntity
    {
        public int CategoryId { get; set; }
        public Recipe Recipes { get; set; }
        public Category Category { get; set; }
    }
}
