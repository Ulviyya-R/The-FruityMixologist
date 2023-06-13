namespace TheFruityMixologist.Entities
{
    public class Comment:BaseEntity
    {
        public string Text { get; set; }
        public DateTime CreationTime { get; set; }
        public User User { get; set; }
        public int RecipeId { get; set; }
        public Recipe Recipe { get; set; }
        public Rating Rating { get; set; }
    }
}
