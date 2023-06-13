using TheFruityMixologist.ViewModels.gifCard;

namespace TheFruityMixologist.ViewModels.BasketsVM
{
    public class BasketVM
    {
        public List<BasketItemVM> BasketItems { get; set; }
        public List<gifCartItemVM> gifCartItemVMs { get; set; }
        public decimal TotalPrice { get; set; }
        public int Count { get; set; }

        public BasketVM()
        {
            BasketItems = new();
            gifCartItemVMs = new();
        }
    }
}
