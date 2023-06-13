using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using TheFruityMixologist.DAL;
using TheFruityMixologist.Entities;
using TheFruityMixologist.ViewModels;
using TheFruityMixologist.ViewModels.gifCard;
using TheFruityMixologist.ViewModels.Order;

namespace TheFruityMixologist.Controllers
{
    public class GifcartController:Controller
    {
        private readonly MixologistDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IWebHostEnvironment _env;


        public GifcartController(MixologistDbContext context,UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<IdentityRole> roleManager, IWebHostEnvironment env)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _env = env;
        }

        public IActionResult Index()
            {
                List<User> users = _context.Users.ToList();

                ViewBag.UserList = new SelectList(users, "Id", "UserName");

                List<gifCart> gifCarts = _context.GifCarts.Include(g => g.GiftCartColor)
                                                 .Include(g => g.PriceOption)
                                                 .ToList();

                gifCartVM VM = new()
                {
                    PriceOptionList =_context.priceOptions.ToList(),
                    GiftCartColors = _context.giftCartColors.ToList()
                };

                return View(VM);
            }


        [HttpPost]
        public async Task<IActionResult> GiftCard(gifCartVM model)
        {
            string message = model.Message;
            string selectedUserName = model.RecipientName;

            User user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (user is null)
            {
                return RedirectToAction("Login", "Account");
            }
            User selectedUser = _context.Users.FirstOrDefault(u => u.Id == selectedUserName);

            gifCart gift = new ()
            {
                User = user,
                RecipientName = selectedUserName, 
                Message = model.Message,
                PriceOptionId = model.SelectedAmountId,
                GiftCartColorId = model.SelectedCardColorId,
                status = Utilities.Enum.OrderStatus.Default,
            };
            if(gift.Message is null || gift.RecipientName is null)
            {
                return BadRequest("Melumatlari tam doldurun!");
            }
            
            _context.GifCarts.Add(gift);
            _context.SaveChanges();

            return RedirectToAction("Index","Home");
        }

        public IActionResult Delete(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            gifCart gifCart = _context.GifCarts.FirstOrDefault(x => x.Id == id);

            _context.GifCarts.Remove(gifCart);
            _context.SaveChanges();
            return RedirectToAction("Index", "home");
        }
    }




    }

