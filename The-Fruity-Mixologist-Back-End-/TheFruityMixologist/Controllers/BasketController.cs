using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using TheFruityMixologist.DAL;
using TheFruityMixologist.Entities;
using TheFruityMixologist.Utilities.Enum;
using TheFruityMixologist.ViewModels.BasketsVM;

namespace TheFruityMixologist.Controllers
{
    public class BasketController:Controller
    {
        private readonly MixologistDbContext _context;
        private readonly UserManager<User> _userManager;

        public BasketController(MixologistDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> AddBasket(int id,int count)
        {

            Recipe? recipe = _context.Recipes.FirstOrDefault(p => p.Id == id);



            if (User.Identity.IsAuthenticated)
            {
                User user = await _userManager.FindByNameAsync(User.Identity.Name);
                BasketItem? basketItem = _context.BasketItems.FirstOrDefault(b => b.RecipeId == recipe.Id && b.UserId == user.Id);
                if (basketItem == null)
                {
                    basketItem = new BasketItem()
                    {
                        UserId = user.Id,
                        RecipeId = recipe.Id,
                        Count = count,
                    };
                    _context.BasketItems.Add(basketItem);
                }
                else
                {
                    basketItem.Count++;
                }
                _context.SaveChanges();
            }
            else
            {
                string basket = HttpContext.Request.Cookies["Basket"];

                List<Recipe> recipes;

                if (basket == null)
                {

                    BasketVM basketVM = new BasketVM()
                    {
                        BasketItems = new List<BasketItemVM>(),
                        TotalPrice = recipe.Price,
                        Count = 1
                    };
                    BasketItemVM basketItem = new BasketItemVM
                    {
                        Recipe = recipe,
                        Count = 1
                    };
                    basketVM.BasketItems.Add(basketItem);
                    basketVM.TotalPrice = recipe.Price;
                    string basketStr = JsonConvert.SerializeObject(basketVM);

                    HttpContext.Response.Cookies.Append("Basket", basketStr);

                }
                else
                {
                    BasketVM basketVM = JsonConvert.DeserializeObject<BasketVM>(basket);
                    BasketItemVM basketItem = basketVM.BasketItems.FirstOrDefault(i => i.Recipe.Id == recipe.Id);
                    if (basketItem == null)
                    {
                        basketItem = new BasketItemVM
                        {
                            Recipe = recipe,
                            Count = 1,

                        };
                        basketVM.Count++;
                        basketVM.BasketItems.Add(basketItem);
                    }
                    else
                    {
                        basketItem.Count++;
                    }
                    basketVM.TotalPrice += basketItem.Recipe.Price;
                    string basketStr = JsonConvert.SerializeObject(basketVM);

                    HttpContext.Response.Cookies.Append("Basket", basketStr);
                }
            }



            return RedirectToAction("Index", "Shop");
        }



        public IActionResult ShowBasket()
        {
            string basketStr = HttpContext.Request.Cookies["Basket"];
            if (!string.IsNullOrEmpty(basketStr))
            {
                BasketVM basket = JsonConvert.DeserializeObject<BasketVM>(basketStr);
                return Json(basket);
            }
            return Content("Basket is empty");
        }

        public async Task<IActionResult> RemoveBasketItem(int id)
        {
            Recipe recipe = _context.Recipes.FirstOrDefault(p => p.Id == id);

            if (User.Identity.IsAuthenticated)
            {
                User user = await _userManager.FindByNameAsync(User.Identity.Name);
                BasketItem basketItem = _context.BasketItems.FirstOrDefault(b => b.RecipeId == recipe.Id && b.UserId == user.Id);
                if (basketItem != null)
                {
                    if (basketItem.Count > 1)
                    {
                        basketItem.Count--;
                    }
                    else
                    {
                        _context.BasketItems.Remove(basketItem);
                    }
                    _context.SaveChanges();
                }
            }
            else
            {
                string basket = HttpContext.Request.Cookies["Basket"];
                if (basket != null)
                {
                    BasketVM basketVM = JsonConvert.DeserializeObject<BasketVM>(basket);
                    BasketItemVM basketItem = basketVM.BasketItems.FirstOrDefault(i => i.Recipe.Id == recipe.Id);
                    if (basketItem != null)
                    {
                        if (basketItem.Count > 1)
                        {
                            basketItem.Count--;
                            basketVM.TotalPrice -= basketItem.Recipe.Price;
                        }
                        else
                        {
                            basketVM.BasketItems.Remove(basketItem);
                            basketVM.Count--;
                            basketVM.TotalPrice -= basketItem.Recipe.Price;
                        }
                        string basketStr = JsonConvert.SerializeObject(basketVM);
                        HttpContext.Response.Cookies.Append("Basket", basketStr);
                    }
                }
            }
            return Redirect(Request.Headers["Referer"].ToString());
        }
    }
}
