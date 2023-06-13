namespace TheFruityMixologist.Entities
{
    public class Ingredient:BaseEntity
    {
     public string Ingredients { get; set; }
        public Recipe Recipes { get; set; }

    }
}
