using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.EntityFrameworkCore;
using TheFruityMixologist.DAL;
using TheFruityMixologist.Entities;
using TheFruityMixologist.ViewModels;

namespace TheFruityMixologist.Controllers
{
    public class PartnerEventsController:Controller
    {
        private readonly MixologistDbContext _context;
        private readonly UserManager<User> _userManager;

        public PartnerEventsController(MixologistDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            EventSliderVM model = new EventSliderVM();

            model.eventSliders = _context.EventSliders.Include(es=>es.eventSliderImages).ToList(); 
            return View(model);
        }

    }
}