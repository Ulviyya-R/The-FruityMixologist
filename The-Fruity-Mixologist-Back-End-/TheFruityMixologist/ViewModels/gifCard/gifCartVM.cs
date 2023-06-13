using TheFruityMixologist.Entities;

namespace TheFruityMixologist.ViewModels.gifCard
{
    public class gifCartVM
    {
        public string RecipientName { get; set; }
        public string Message { get; set; }
        public DateTime DeliveryDate { get; set; }
        public int SelectedCardColorId { get; set; }
        public int SelectedAmountId { get; set; }
        public List<PriceOption> PriceOptionList { get; set; }
        public List<giftCartColor> GiftCartColors { get; set; }
        public PriceOption PriceOption { get; set; }
        public giftCartColor GiftCartColor { get; set; }
    }
}
