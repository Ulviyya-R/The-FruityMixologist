namespace TheFruityMixologist.Entities
{
    public class WishlistItem:BaseEntity
    {
        public int RecipeId { get; set; }

        public string UserId { get; set; }
        public bool isLiked { get; set; }

        public Recipe Recipe { get; set; }

        public User User { get; set; }
    }
}
