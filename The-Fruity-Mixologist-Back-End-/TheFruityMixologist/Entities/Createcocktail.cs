namespace TheFruityMixologist.Entities
{
	public class Createcocktail:BaseEntity
	{
		public string Name { get; set; }
		public decimal Price { get; set; }
		public bool IsSelected { get; set; }
		public int RecipeId { get; set; }
	    public Recipe Recipe { get; set; }
	}
}
