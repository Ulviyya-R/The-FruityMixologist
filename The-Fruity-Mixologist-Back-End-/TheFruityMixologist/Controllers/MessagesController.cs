using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using TheFruityMixologist.DAL;
using TheFruityMixologist.Entities;
using TheFruityMixologist.Hubs;

namespace TheFruityMixologist.Controllers
{
    public class MessagesController:Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IHubContext<ChatHub> _hubContext;
        private readonly MixologistDbContext _context;

        public MessagesController(MixologistDbContext context,UserManager<User> userManager,  RoleManager<IdentityRole> roleManager, IHubContext<ChatHub> hubContext)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _hubContext = hubContext;
            _context = context;
        }


        public async Task<IActionResult> Index()
        
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (User.Identity.IsAuthenticated)
            {
                ViewBag.CurrentUserName = currentUser.UserName;
            }
            var messages = await _context.Messages.ToListAsync();
            return View(messages);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Message message,string text)
        {

            message.Username = User.Identity.Name;
            var sender = await _userManager.GetUserAsync(User);
            message.Image = sender.Path;
            message.When = DateTime.Now;
            message.Text = text;
            message.UserId = sender.Id;
            await _context.Messages.AddAsync(message);
            await _context.SaveChangesAsync();
        await _hubContext.Clients.All.SendAsync("ReceiveMessage", message.Username, message.Text);

            return Ok();
        }

        }
    }
