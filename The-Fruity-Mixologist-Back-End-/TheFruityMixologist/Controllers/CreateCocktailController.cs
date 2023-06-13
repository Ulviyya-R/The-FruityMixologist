using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TheFruityMixologist.DAL;
using TheFruityMixologist.Entities;
using TheFruityMixologist.ViewModels;

namespace TheFruityMixologist.Controllers
{
	public class CreateCocktailController : Controller
	{
		private readonly MixologistDbContext _context;
		private readonly UserManager<User> _userManager;
		private readonly SignInManager<User> _signInManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly IWebHostEnvironment _env;


		public CreateCocktailController(MixologistDbContext context, UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<IdentityRole> roleManager, IWebHostEnvironment env)
		{
			_context = context;
			_userManager = userManager;
			_signInManager = signInManager;
			_roleManager = roleManager;
			_env = env;
		}

		public IActionResult Index()
		{
			return View();
		}

        [HttpPost]
        public async Task<IActionResult> Index(int alcohol, int syrups, int aromatcBitters, int Ice, int Lemon)
        {
            int totalPrice = CalculateTotalPrice(alcohol, syrups, aromatcBitters, Ice, Lemon);

            if (totalPrice != 0 && totalPrice>=11)
            {
                Recipe userrecipe = new Recipe()
                {
                    Name = "Manually Created Recipe",
                    Desc = "This is a manually created recipe.",
                    Price = totalPrice,
                    
                    IsCreatingUserCock = true
                };

                RecipesImages falsePhoto = new RecipesImages()
                {
                    IsMain = false,
                    Path = "Ulviye.jpg"
                };
                userrecipe.RecipesImages.Add(falsePhoto);

                RecipesImages main = new RecipesImages()
                {
                    IsMain = true,
                    Path = "Ulviye.jpg"
                };
                userrecipe.RecipesImages.Add(main);

                RecipesCategory category1 = new RecipesCategory()
                {
                    CategoryId = 1
                };
                userrecipe.RecipesCategories.Add(category1);

                RecipesCategory category2 = new RecipesCategory()
                {
                    CategoryId = 2
                };
                userrecipe.RecipesCategories.Add(category2);

                Step step1 = new Step()
                {
                    Recipes = userrecipe,
                    Steps = "Step 1"
                };
                _context.Steps.Add(step1);

                Step step2 = new Step()
                {
                    Recipes = userrecipe,
                    Steps = "Step 2"
                };
                _context.Steps.Add(step2);

                Ingredient ingredient1 = new Ingredient()
                {
                    Recipes = userrecipe,
                    Ingredients = "Ingredient 1"
                };
                _context.Ingredients.Add(ingredient1);

                Ingredient ingredient2 = new Ingredient()
                {
                    Recipes = userrecipe,
                    Ingredients = "Ingredient 2"
                };
                _context.Ingredients.Add(ingredient2);

                _context.Recipes.Add(userrecipe);
                _context.SaveChanges();

                return RedirectToAction("AddBasket", "Basket", new { id = userrecipe.Id });

            }

            return RedirectToAction("Index", "Home");
        }

        private int CalculateTotalPrice(int alcohol, int syrups, int aromatcBitters, int Ice, int Lemon)
        {
            int totalPrice = 0;

            if (alcohol != 0 && alcohol == 2)
            {
                totalPrice += alcohol;
            }
            if (syrups != 0 && syrups == 4)
            {
                totalPrice += syrups;
            }
            if (aromatcBitters != 0 && aromatcBitters == 5)
            {
                totalPrice += aromatcBitters;
            }
            if (Ice != 0 && Ice==1)
            {
                totalPrice += Ice;
            }
            if (Lemon != 0 && Lemon==1)
            {
                totalPrice += Lemon;
            }

            return totalPrice;
        }

    }
}
