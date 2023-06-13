using Microsoft.AspNetCore.Mvc;
using TheFruityMixologist.DAL;

namespace TheFruityMixologist.Controllers
{
    public class OurStoryController:Controller
    {
        private readonly MixologistDbContext _context;

        public OurStoryController(MixologistDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
