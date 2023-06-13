using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TheFruityMixologist.DAL;
using TheFruityMixologist.Entities;
using TheFruityMixologist.Services.Interfaces;
using TheFruityMixologist.Utilities.Comparer;
using TheFruityMixologist.Utilities.Enum;

namespace TheFruityMixologist.Controllers
{
    public class ShopController:Controller
    {
        private readonly MixologistDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IRecipeService _recipeService;
        private readonly IRatingService _ratingService;

        public ShopController(MixologistDbContext context, UserManager<User> userManager,IRecipeService recipeService,IRatingService ratingService)
        {
            _context = context;
            _userManager = userManager;
            _recipeService = recipeService;
            _ratingService = ratingService;
        }
        public async Task<IActionResult> Index(int page = 1)
        {
            if (page <= 0)
            {
                page = 1;
            }
            ViewBag.Recipes = await _recipeService.GetAllAsyncwithPaginated(page);
            ViewBag.Ratings = await _ratingService.GetAllAsync();
            var query = _context.Recipes.AsQueryable();
            var results = ViewBag.Recipes;
            ViewBag.SelectedPage = page;
            ViewBag.TotalPage = Math.Ceiling(query.Count() / 9m);
            return View(results);
        }
        public async Task<IActionResult> Detail(int id)
        {
            ViewBag.Ratings = await _ratingService.GetAllAsync();
            if (id == 0) return NotFound();
            IQueryable<Recipe> recipes = _context.Recipes.AsNoTracking().AsQueryable();
            Recipe? recipe = await _recipeService.GetFullDataByIdAsync(id);

            ViewBag.RelatedProduct = RelatedProducts(recipes, recipe, id);
            if (recipe == null) return NotFound();
            return View(recipe);
        }


        [HttpPost]
        public async Task<IActionResult> Sorts(string? sort)
        {
            ViewBag.Ratings = await _ratingService.GetAllAsync();

            IEnumerable<Recipe> recipes = await _recipeService.GetAllAsync();

            if (!string.IsNullOrEmpty(sort))
            {
                switch (sort)
                {
                    case "All":
                        recipes = recipes;
                        break;
                    case "Alcoholic":
                        recipes = recipes.Where(r => r.RecipesCategories.Any(rc => rc.Category.Name == "Alcoholic"));
                        break;
                    case "Non-Alcoholic":
                        recipes = recipes.Where(r => r.RecipesCategories.Any(rc => rc.Category.Name == "Non-Alcoholic"));
                        break;
                }
            }
            List<Recipe> res = recipes.ToList();

            return PartialView("_RecipesPartial", res);
        }


        [HttpPost]
        public async Task<IActionResult> SortPrice(string[] price)
        {
            ViewBag.Ratings = await _ratingService.GetAllAsync();
            IEnumerable<Recipe> queryPrice = await _recipeService.GetAllAsync();

            if (price != null && price.Length > 0)
            {
                if (price.Contains("expensive"))
                    queryPrice = queryPrice.Where(x => x.Price > 50);
                if (price.Contains("middle"))
                    queryPrice = queryPrice.Where(x => x.Price >= 30 && x.Price <= 50);
                if (price.Contains("cheap"))
                    queryPrice = queryPrice.Where(x => x.Price >= 5 && x.Price <= 10);
            }

            var results = queryPrice.ToList();
            return PartialView("_RecipesPartial", results);
        }
        static List<Recipe> RelatedProducts(IQueryable<Recipe> queryable, Recipe recipe, int id)
        {
            List<Recipe> relateds = new();

            recipe.RecipesCategories.ForEach(rc =>
            {
                List<Recipe> related = queryable.
                        Include(r => r.RecipesImages).
                        Include(r => r.RecipesCategories).ThenInclude(p => p.Category).
                       AsEnumerable().
                       Where(r => r.RecipesCategories.Contains(rc, new RecipeCategoryComparer())
                            && r.Id != id && !relateds.Contains(r, new RecipeComparer())).
                            Take(4).
                            ToList();
                relateds.AddRange(related);

            });
            return relateds;
        }

        [HttpPost]
        public async Task<IActionResult> AddComment(int id, Comment comment)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            if (comment.Text is null)
            {
                ModelState.AddModelError("", "Please add text ");
                return View();
            }

            if (User.Identity.IsAuthenticated)
            {
                Recipe recipe = await _recipeService.GetFullDataByIdAsync(id);
                if (recipe == null)
                {
                    return NotFound();
                }

                User user = await _userManager.FindByNameAsync(User.Identity.Name);

                Rating ratingRegistered = new()
                {
                    Point = comment.Rating.Point
                };

                Comment commentRegistered = new()
                {
                    Text = comment.Text,
                    User = user,
                    Recipe = recipe,
                    Rating = ratingRegistered,
                    CreationTime = DateTime.Now
                };

                double rateCount = _context.Ratings.Where(r => r.Comment.RecipeId == id).Count();

                comment.Text = string.Empty;

                if (recipe.PointReyting.HasValue)
                {
                    double currentTotalRating = recipe.PointReyting.Value * rateCount;
                    double newTotalRating = currentTotalRating + comment.Rating.Point;
                    rateCount++;
                    double averageRating = newTotalRating / rateCount;
                    recipe.PointReyting = Math.Min(10.0, averageRating);
                }
                else
                {
                    recipe.PointReyting = Math.Min(10.0, comment.Rating.Point);
                }

                _context.Comments.Add(commentRegistered);
                _context.Recipes.Update(recipe);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Detail), new { id });
        }


        /// <summary>
        /// I hid the viewbags I sent in actions here
        /// </summary>
        private async Task AllViewBagsData()
        {
            ViewBag.Categories = _context.Categories.AsEnumerable();

        }



    }
}


