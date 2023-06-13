namespace TheFruityMixologist.Entities
{
    public class Category : BaseEntity
    {
        public string Name { get; set; }
        public List<RecipesCategory> RecipesCategories { get; set; }

        public Category()
        {
            RecipesCategories = new();
        }
    }
}
