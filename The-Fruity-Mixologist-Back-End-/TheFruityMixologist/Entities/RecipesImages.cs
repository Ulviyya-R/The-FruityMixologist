namespace TheFruityMixologist.Entities
{
    public class RecipesImages : BaseEntity
    {
        public string Path { get; set; }
        public bool? IsMain { get; set; }
        public int  RecipeId { get; set; }

        public Recipe Recipe { get; set; }
    }
}
