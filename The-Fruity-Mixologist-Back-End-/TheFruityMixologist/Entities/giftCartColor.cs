namespace TheFruityMixologist.Entities
{
    public class giftCartColor:BaseEntity
    {
        public int Id { get; set; }
        public string ImagePath { get; set; }
        public string Color { get; set; }
        public bool IsSelected { get; set; }
        public List<gifCart> GiftCarts { get; set; }

        public giftCartColor()
        {
            GiftCarts = new List<gifCart>();
        }
    }
}
