using TheFruityMixologist.Utilities.Enum;

namespace TheFruityMixologist.Entities
{
    public class gifCart:BaseEntity
    {
        public User User { get; set; }
        public string RecipientName { get; set; }
        public string Message { get; set; }
        public OrderStatus status { get; set; }
        public int PriceOptionId { get; set; }
        public int GiftCartColorId { get; set; }
        public PriceOption PriceOption { get; set; }
        public giftCartColor GiftCartColor { get; set; }
    }
}
