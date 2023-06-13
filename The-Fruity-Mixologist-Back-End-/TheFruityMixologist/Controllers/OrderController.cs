using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TheFruityMixologist.DAL;
using TheFruityMixologist.Entities;
using TheFruityMixologist.ViewModels.Order;

namespace TheFruityMixologist.Controllers
{
    public class OrderController : Controller
    {
        private readonly MixologistDbContext _context;
        private readonly UserManager<User> _userManager;

        public OrderController(MixologistDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<IActionResult> Checkout()
        {
            User user = await _userManager.FindByNameAsync(User.Identity.Name);
            ViewBag.Users = _context.GifCarts.Include(g => g.GiftCartColor).Include(g => g.PriceOption).Where(g => g.RecipientName == user.Id).ToList();

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


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkout(OrderVM orderVM, int selectedColor)
        {
            User user = await _userManager.FindByNameAsync(User.Identity.Name);
            ViewBag.Users = _context.GifCarts.Include(g => g.GiftCartColor).Include(g => g.PriceOption).Where(g => g.RecipientName == user.Id).ToList();
            double cartprice = 0;
            OrderVM model = new OrderVM()
            {
                Fullname = orderVM.Fullname,
                Username = orderVM.Username,
                Email = orderVM.Email,
                Note = orderVM.Note,
                Address = orderVM.Address,
                Country = orderVM.Country,
                BasketItems = _context.BasketItems.Include(p => p.Recipe).ThenInclude(p => p.RecipesImages).Where(c => c.UserId == user.Id).ToList(),
                gifCarts = _context.GifCarts.Include(g => g.GiftCartColor).Include(g => g.PriceOption).Where(g => g.User.Id == user.Id).ToList(),



            };
            Order order = new Order()
            {
                Country = orderVM.Country,
                Adress = orderVM.Address,
                Note = orderVM.Note,
                TotalPrice = 0,
                Date = DateTime.Now,
                User = user,
                Status = null
            }; ;
            if (!ModelState.IsValid)
            {
                foreach (string message in ModelState.Values.SelectMany(e => e.Errors).Select(e => e.ErrorMessage))
                {
                    ModelState.AddModelError("", message);
                }
            }
            if (model.BasketItems.Count != 0)
            {
                foreach (BasketItem item in model.BasketItems)
                {
                    order.TotalPrice += ((double)item.Recipe.Price*item.Count);
                    OrderItem orderItem = new OrderItem
                    {
                        Name = item.Recipe.Name,
                        Price = (double)item.Recipe.Price,
                        RecipeId = item.RecipeId,
                        Order = order,
                        Count = item.Count,
                        
                    };

                    _context.OrderItems.Add(orderItem);
                }
                _context.BasketItems.RemoveRange(model.BasketItems);

            }

            if (model.gifCarts.Count != 0)
            {

                foreach (gifCart item in model.gifCarts.Where(x => x.status == Utilities.Enum.OrderStatus.Default))
                {
                    order.TotalPrice += (double)item.PriceOption.Amount;
                    OrderItem orderItem = new OrderItem
                    {
                        Name = item.GiftCartColor.Color,
                        Price = (double)item.PriceOption.Amount,
                        gifCartId = item.Id,
                        Order = order
                    };

                    _context.OrderItems.Add(orderItem);
                    gifCart giftcart = _context.GifCarts.FirstOrDefault(g => g.Id == item.Id);
                    giftcart.status = Utilities.Enum.OrderStatus.Pending;

                }


            }
            if (selectedColor != 0)
            {
                gifCart? cart = _context.GifCarts.Include(x => x.GiftCartColor).Include(x => x.PriceOption).Include(x => x.User).FirstOrDefault(x => x.Id == selectedColor && x.User.Id == user.Id);
                if (cart is null) return Redirect("~/Error/Error");
                OrderItem item = _context.OrderItems.Include(x => x.gifCart).FirstOrDefault(x => x.gifCartId == cart.Id);

                cartprice = (double)cart.PriceOption.Amount;
                _context.OrderItems.Remove(item);
                _context.GifCarts.Remove(cart); 


            }
            if (order.TotalPrice > cartprice)
            {
                order.TotalPrice -= cartprice;
            }
            _context.Orders.Add(order);
            _context.SaveChanges();
            return RedirectToAction("Index", "Home");
        }



    }
}