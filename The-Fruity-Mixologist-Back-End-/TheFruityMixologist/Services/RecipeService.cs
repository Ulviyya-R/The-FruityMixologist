using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TheFruityMixologist.DAL;
using TheFruityMixologist.Entities;
using TheFruityMixologist.Services.Interfaces;

namespace TheFruityMixologist.Services
{
    public class RecipeService : IRecipeService
    {
        private readonly MixologistDbContext _context;

        public RecipeService(MixologistDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Recipe>> GetAllAsyncwithPaginated(int page = 1) => await _context.Recipes.Include(m => m.RecipesImages).
            Include(m => m.RecipesCategories).
            ThenInclude(m => m.Category).
            Include(m => m.RecipeComments).
            ThenInclude(m => m.Rating).
            Skip((page - 1) * 9).Take(8).
            ToListAsync();

        public async Task<IEnumerable<Recipe>> GetAllAsync() => await _context.Recipes.Include(m => m.RecipesImages).
            Include(m => m.RecipesCategories).
            ThenInclude(m => m.Category).
            Include(m => m.RecipeComments).
            ThenInclude(m => m.Rating).
            ToListAsync();
       

        public async Task<Recipe> GetByIdAsync(int id) => await _context.Recipes.FindAsync(id);


        public async Task<Recipe> GetFullDataByIdAsync(int id) => await _context.Recipes.Include(m => m.RecipesImages).
            Include(m => m.RecipesCategories).
            ThenInclude(m => m.Category).
            Include(m => m.RecipeComments).
            ThenInclude(m => m.Rating).
            Include(m => m.RecipeComments).
            ThenInclude(m => m.User).
            Include(m => m.Ingredients).
            Include(m => m.Steps).
            Include(m=>m.WishListItems).
            ThenInclude(wl=>wl.User).

            FirstOrDefaultAsync(m => m.Id == id);

        public async Task<gifCart> GiftGetFullDataByIdAsync(int id) => await _context.GifCarts.
                                  Include(g => g.GiftCartColor).
                                  Include(g => g.PriceOption).
                                  FirstOrDefaultAsync(g => g.Id == id);
                                                 
    }
}
