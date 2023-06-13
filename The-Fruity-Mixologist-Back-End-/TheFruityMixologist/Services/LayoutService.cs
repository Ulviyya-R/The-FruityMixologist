using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using TheFruityMixologist.DAL;
using TheFruityMixologist.Entities;
using TheFruityMixologist.Utilities.Enum;
using TheFruityMixologist.ViewModels.BasketsVM;
using TheFruityMixologist.ViewModels.gifCard;
using TheFruityMixologist.ViewModels.WishlistsVM;

namespace TheFruityMixologist.Services
{
    public class LayoutService
    {
        private readonly MixologistDbContext _context;
        private readonly IHttpContextAccessor _accessor;
        private readonly UserManager<User> _userManager;

        public LayoutService(MixologistDbContext context, IHttpContextAccessor accessor, UserManager<User> userManager)
        {
            _context = context;
            _accessor = accessor;
            _userManager = userManager;
        }

        public Dictionary<string, string> GetSettings()
        {
            Dictionary<string, string> settings = _context.Settings.ToDictionary(s => s.Key, s => s.Value);

            return settings;
        }

        public BasketVM ShowBasket()
        {
            string basket = _accessor.HttpContext.Request.Cookies["Basket"];
            BasketVM basketVM = new BasketVM();
            BasketVM basketData = new BasketVM()
            {
                TotalPrice = 0,
                BasketItems = new List<BasketItemVM>(),
                Count = 0
            };
            if (_accessor.HttpContext.User.Identity.IsAuthenticated)
            {

                List<BasketItem> basketItems = _context.BasketItems.Include(b => b.User).Where(b => b.User.UserName == _accessor.HttpContext.User.Identity.Name).ToList();
                List<gifCart> gifCarts = _context.GifCarts.Include(g => g.User).Include(g => g.PriceOption).Include(g => g.GiftCartColor).Where(g => g.User.UserName == _accessor.HttpContext.User.Identity.Name && g.status == OrderStatus.Default).ToList();
                foreach (BasketItem item in basketItems)
                {
                    Recipe recipe = _context.Recipes.FirstOrDefault(f => f.Id == item.RecipeId);
                    if (recipe != null)
                    {
                        BasketItemVM basketItemVM = new BasketItemVM()
                        {
                            Recipe = recipe,
                            Count = item.Count
                        };
                        basketItemVM.Recipe.Price = recipe.Price;
						basketData.BasketItems.Add(basketItemVM);
                        basketData.Count++;
                        basketData.TotalPrice += item.Recipe.Price * item.Count;
					}
                }
                foreach (gifCart item in gifCarts)
                {
                    gifCart gifCart = _context.GifCarts.FirstOrDefault(f => f.Id == item.Id);
                    if (gifCart != null)
                    {
                        gifCartItemVM gifCartItemVM = new gifCartItemVM()
                        {
                            gifCart = gifCart
                        };
                        gifCartItemVM.gifCart.PriceOption.Amount = gifCart.PriceOption.Amount;
                        basketData.gifCartItemVMs.Add(gifCartItemVM);
                        basketData.Count++;
                        basketData.TotalPrice += item.PriceOption.Amount;
                    }
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(basket))
                {
                    basketVM = JsonConvert.DeserializeObject<BasketVM>(basket);
                    foreach (BasketItemVM item in basketVM.BasketItems)
                    {
                        item.Recipe = _context.Recipes.FirstOrDefault(s => s.Id == item.Recipe.Id);
                        if (item.Recipe != null)
                        {
                            basketData.BasketItems.Add(item);
                            basketData.TotalPrice += item.Recipe.Price * item.Count;

                        }
                        basketData.Count++;
                    }

                }
            }

            return basketData;
        }

     
        public List<Recipe> GetRecipes()
        {
            List<Recipe> recipes = _context.Recipes.Include(p => p.RecipesImages).ToList();
            return recipes;
        }

        public List<gifCart> GetGifCarts()
        {
            List<gifCart> gifCarts = _context.GifCarts.Include(g => g.GiftCartColor).Include(g=>g.PriceOption).ToList();
            return gifCarts;

        }


        public List<Recipe> ShowBasketRecipe()
        {

            List<Recipe> recipes = _context.Recipes.Include(p => p.RecipesCategories).ThenInclude(rc => rc.Category)
                                             .Include(p => p.RecipesImages)
                                             .Take(4).
                                             ToList();
            return recipes;
        }


        public List<WishlistItem> ShowWishList()
        {
                List<WishlistItem> wishlistItems = _context.WishlistItems.Include(b => b.User).Where(b => b.User.UserName == _accessor.HttpContext.User.Identity.Name).ToList();
                foreach (WishlistItem item in wishlistItems)
                {
                    Recipe recipe = _context.Recipes.FirstOrDefault(f => f.Id == item.RecipeId);
                    if (recipe != null)
                    {
                        WishlistItem wishlistItem = new()
                        {
                            Recipe = recipe,
                        };
                        wishlistItem.Recipe.Price = recipe.Price;
                    }
                }
            return wishlistItems;
        }

        public List<Order> GetAllOrderItem()
        {
            List<Order> orders = _context.Orders.Include(o=>o.User).Include(o => o.OrderItems).ToList();
            return orders;
        }




    }
}
