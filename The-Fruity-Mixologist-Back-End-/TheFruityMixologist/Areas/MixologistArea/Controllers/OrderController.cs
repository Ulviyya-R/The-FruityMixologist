using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using TheFruityMixologist.DAL;
using TheFruityMixologist.Entities;
using TheFruityMixologist.Services.Interfaces;
using TheFruityMixologist.Utilities.Enum;

namespace TheFruityMixologist.Areas.MixologistArea.Controllers
{
    [Area("MixologistArea")]
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class OrderController:Controller
    {
        private readonly MixologistDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IEmailService _emailService;

        public OrderController(MixologistDbContext context,UserManager<User> userManager,IEmailService emailService)
        {
            _context = context;
            _userManager = userManager;
            _emailService = emailService;
        }

        public IActionResult Index()
        {
            List<Order> orders = _context.Orders.Include(o=>o.OrderItems).ThenInclude(oi=>oi.gifCart).ToList();
            return View(orders);
        }

        public IActionResult Edit(int id)
        {
            Order? order = _context.Orders.Include(o=>o.OrderItems).ThenInclude(oi=>oi.gifCart)
                                         .Include(o => o.OrderItems)
                                         .Include(o => o.User)
                                         .FirstOrDefault(o => o.Id == id);
            if (order == null) return NotFound();

            return View(order);
        }

        public async Task<IActionResult> Accept(int id)
        {


            Order order = _context.Orders.Include(o => o.OrderItems).ThenInclude(oi => oi.gifCart)
                                         .Include(o => o.OrderItems)
                                         .Include(o => o.User)
                                         .FirstOrDefault(o => o.Id == id);

            foreach (var orderItem in order.OrderItems)
            {
                gifCart gifCart = _context.GifCarts.FirstOrDefault(gc => gc.Id == orderItem.gifCartId);
            
                if (gifCart != null)
                {
                    User user =  _context.Users.Include(x=>x.GifCarts).FirstOrDefault(x=>x.Id==gifCart.RecipientName);
                    gifCart.status = OrderStatus.Accepted;
                    user.GifCarts.Add(gifCart);
                }

            }

            if (order == null) return NotFound();

            order.Status = true;

            _context.SaveChanges();
            string templatePath = Path.Combine("wwwroot/assets/template/htmlpage.html");
            string emailTemplate = await System.IO.File.ReadAllTextAsync(templatePath);
            string total = order.TotalPrice.ToString();
            string recipientEmail = order.User.Email;
            string subject = "Your order has been accepted";
            string body = emailTemplate.Replace("{SUBJECT}", subject).Replace("{total}", total);
            _emailService.Send(recipientEmail, subject, body);

            return RedirectToAction("Index", "Order");

        }
        public async Task<IActionResult> Reject(int id)
        {


            Order order = _context.Orders.Include(o => o.OrderItems).ThenInclude(oi => oi.gifCart)
                                         .Include(o => o.OrderItems)
                                         .Include(o => o.User)
                                         .FirstOrDefault(o => o.Id == id);
            if (order == null) return NotFound();

            order.Status = false;

            _context.SaveChanges();
            string templatePath = Path.Combine("wwwroot/assets/template/canceledRecipes.html");
            string emailTemplate = await System.IO.File.ReadAllTextAsync(templatePath);
            string total = order.TotalPrice.ToString();
            string recipientEmail = order.User.Email;
            string subject = "Your order has been accepted";
            string body = emailTemplate.Replace("{SUBJECT}", subject).Replace("{total}", total);
            _emailService.Send(recipientEmail, subject, body);

            return RedirectToAction("Index", "Order");
        }
    }
}
