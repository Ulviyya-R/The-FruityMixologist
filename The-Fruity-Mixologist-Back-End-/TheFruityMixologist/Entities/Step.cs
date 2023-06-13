namespace TheFruityMixologist.Entities
{
    public class Step:BaseEntity
    {
        public string Steps { get; set; }
        public Recipe Recipes { get; set; }
    }
}
