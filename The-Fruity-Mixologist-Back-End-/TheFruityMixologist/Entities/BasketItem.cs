namespace TheFruityMixologist.Entities
{
    public class BasketItem : BaseEntity
    {
        public int Id { get; set; }
        public int Count { get; set; }
        public int RecipeId { get; set; }
        public Recipe Recipe { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
    }
}
