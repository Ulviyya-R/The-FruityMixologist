namespace TheFruityMixologist.Entities
{
    public class OrderItem:BaseEntity
    {
        public string Name { get; set; }
        public double Price { get; set; }

        public int OrderId { get; set; }

        public int? RecipeId { get; set; }
        public int Count { get; set; }

        public Recipe? Recipe { get; set; }

        public int? gifCartId { get; set; }

        public gifCart? gifCart { get; set; }

        public Order Order { get; set; }

    }
}
