using TheFruityMixologist.Entities;

namespace TheFruityMixologist.ViewModels
{
	public class CocktailVM
	{

		public List<Createcocktail> createCocktails { get; set; }
		public List<int> SelectedProductIds { get; set; }
		public decimal TotalPrice { get; set; }
	}
}
