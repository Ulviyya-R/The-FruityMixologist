using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TheFruityMixologist.DAL;
using TheFruityMixologist.Entities;
using TheFruityMixologist.Services;
using TheFruityMixologist.Utilities.Comparer;
using TheFruityMixologist.Utilities.Extensions;
using TheFruityMixologist.ViewModels;
using TheFruityMixologist.ViewModels.BasketsVM;
using TheFruityMixologist.ViewModels.Order;

namespace TheFruityMixologist.Controllers
{
    public class CartController:Controller
    {

        private readonly MixologistDbContext _context;
        private readonly UserManager<User> _userManager;

        public CartController(MixologistDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {

            User user = await _userManager.FindByNameAsync(User.Identity.Name);
            List<BasketItem> items = _context.BasketItems.Include(p => p.Recipe).ThenInclude(p => p.RecipesImages).Where(c => c.UserId == user.Id).ToList();
            if (items is not null)
            {
                BasketItem itemss = _context.BasketItems.Include(p => p.Recipe).ThenInclude(p => p.RecipesImages).First();
                IQueryable<Recipe> recipes = _context.Recipes.Include(r => r.Ingredients)
                                 .Include(r => r.RecipesImages)
                                .Include(r => r.Steps)
                                .Include(r => r.RecipeComments).ThenInclude(rc => rc.User)
                                .Include(r => r.RecipesCategories).ThenInclude(rc => rc.Category).AsQueryable();


                Recipe? recipe = recipes.FirstOrDefault(x => x.Id == itemss.RecipeId);
                ViewBag.RelatedProduct = RelatedProducts(recipes, recipe, itemss.RecipeId);

            }
            OrderVM model = new OrderVM()
            {
                Fullname = user.Fullname,
                Username = user.UserName,
                Email = user.Email,
                BasketItems = _context.BasketItems.Include(p => p.Recipe).ThenInclude(p => p.RecipesImages).Where(c => c.UserId == user.Id).ToList(),
                gifCarts = _context.GifCarts.Include(g => g.GiftCartColor).Include(g => g.PriceOption).Where(g => g.User.Id == user.Id).ToList(),

            };




            return View(model);
        }
        static List<Recipe> RelatedProducts(IQueryable<Recipe> queryable, Recipe recipe, int id)
        {
            List<Recipe> relateds = new();

            recipe.RecipesCategories.ForEach(rc =>
            {
                List<Recipe> related = queryable
                      .Include(r => r.Ingredients)
                                    .Include(r => r.RecipesImages)
                                   .Include(r => r.Steps)
                                   .Include(r => r.RecipeComments).ThenInclude(rc => rc.User)
                                   .Include(r => r.RecipesCategories).ThenInclude(rc => rc.Category).
                                   Take(4).
                                   AsEnumerable().
                       Where(r => r.RecipesCategories.Contains(rc, new RecipeCategoryComparer())
                            && r.Id != id && !relateds.Contains(r, new RecipeComparer()) && r.IsCreatingUserCock ==false ).
                           
                            ToList();
                relateds.AddRange(related);

            });
            return relateds;
        }
    }
}
