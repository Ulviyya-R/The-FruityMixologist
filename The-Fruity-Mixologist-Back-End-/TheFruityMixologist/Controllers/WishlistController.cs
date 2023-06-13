using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TheFruityMixologist.DAL;
using TheFruityMixologist.Entities;

namespace TheFruityMixologist.Controllers
{
    public class WishlistController:Controller
    {
        private readonly MixologistDbContext _context;
        private readonly UserManager<User> _userManager;

        public WishlistController(MixologistDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> AddToWishList(int recipeId)
        {
            Recipe? product = await _context.Recipes.FindAsync(recipeId);

            if (product is null)
            {
                return NotFound();
            }

            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            User user = await _userManager.FindByNameAsync(User.Identity.Name);

            WishlistItem? userWishlistItem = await _context.WishlistItems
                .FirstOrDefaultAsync(x => x.UserId == user.Id && x.RecipeId == recipeId);

            if (userWishlistItem is null)
            {
                userWishlistItem = new WishlistItem
                {
                    UserId = user.Id,
                    RecipeId = recipeId,
                    isLiked = true,
                };
                _context.WishlistItems.Add(userWishlistItem);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("Index","Shop");
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFromWishList(int wishListItemId)
        {
            User user = await _userManager.FindByNameAsync(User.Identity.Name);
            WishlistItem? wishListItem = await _context.WishlistItems
                .FirstOrDefaultAsync(x => x.UserId == user.Id && x.RecipeId == wishListItemId);
            if (wishListItem is null)
            {
                return NotFound();
            }
            _context.WishlistItems.Remove(wishListItem);
            await _context.SaveChangesAsync();

            return Redirect(Request.Headers["Referer"].ToString());
        }
    }
}
