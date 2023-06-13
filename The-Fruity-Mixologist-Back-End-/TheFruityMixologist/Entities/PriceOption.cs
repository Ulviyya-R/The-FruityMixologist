namespace TheFruityMixologist.Entities
{
    public class PriceOption:BaseEntity
    {
        public decimal Amount { get; set; }
        public bool IsSelected { get; set; }
        public ICollection<gifCart> GiftCards { get; set; }

        public PriceOption()
        {
            GiftCards = new List<gifCart>();
        }
    }
}
