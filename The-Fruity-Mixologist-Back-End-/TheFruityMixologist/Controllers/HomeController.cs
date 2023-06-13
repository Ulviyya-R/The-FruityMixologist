using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
using System.Net;
using TheFruityMixologist.DAL;
using TheFruityMixologist.Entities;
using TheFruityMixologist.ViewModels;
using TheFruityMixologist.Services;
using TheFruityMixologist.Services.Interfaces;
using static System.Net.WebRequestMethods;

namespace TheFruityMixologist.Controllers
{
    public class HomeController : Controller
    {
        private readonly MixologistDbContext _context;
        private readonly IEmailService _emailService;

        //private readonly EmailService _emailService;

        public HomeController(MixologistDbContext context , IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        public IActionResult Index()
        {
            SlidersVM model   = new SlidersVM();

            model.slider = _context.PopularSliders.ToList();
            ViewBag.HomeRecipes = _context.Recipes.Include(p => p.RecipesCategories).ThenInclude(rc => rc.Category)
                                            .Include(p => p.RecipesImages)
                                            .Take(4).
                                            ToList();
            ViewBag.PartnerPartial = _context.Settings.ToList();

            ViewBag.RaytingRecipes = _context.Recipes.Include(p => p.RecipesCategories).ThenInclude(rc => rc.Category)
                                            .Include(p => p.RecipesImages)
                                            .Include(p=>p.RecipeComments).ThenInclude(rc=>rc.Rating)
                                            .ToList();
            var comments = _context.Comments.Include(c => c.User).Include(c => c.Recipe).ToList();
            model.comments = comments;
            return View(model);
        }

        public IActionResult Search(string search)
        {
            var query = _context.Recipes.Include(p => p.RecipesImages)
                                          .AsQueryable()
                                          .Where(p => p.Name.Contains(search));
            List<Recipe> recipes = query.OrderByDescending(p => p.Id).ToList();

            return PartialView("_SearchPartial", recipes);

        }

        [HttpPost]
        public IActionResult Sorts(string? sort)
        {
            IQueryable<Recipe> recipes = _context.Recipes.Include(r => r.RecipesImages).Include(v => v.RecipesCategories)
                    .ThenInclude(e => e.Category).Where(r=>r.IsCreatingUserCock == false);
            if (!string.IsNullOrEmpty(sort))
            {
                switch (sort)
                {
                    case "All":
                        recipes = recipes.Take(4);
                        break;
                    case "Alcoholic":
                        recipes = recipes.Where(r => r.RecipesCategories.Any(rc => rc.Category.Name == "Alcoholic")).Take(4);
                        break;
                    case "Non-Alcoholic":
                        recipes = recipes.Where(r => r.RecipesCategories.Any(rc => rc.Category.Name == "Non-Alcoholic")).Take(4);
                        break;
                }
            }
            List<Recipe> res = recipes.ToList();



            return PartialView("_HomeRecipePartial", res);
        }
        public ActionResult Subscribe(string email)
        {

            bool Isdublicate = _context.Subscribes.Any(c => c.Email == email);

            if (Isdublicate)
            {
                return Redirect(Request.Headers["Referer"].ToString());
            }
            Subscribe subscribe = new()
            {
                Email = email
            };
            _context.Subscribes.Add(subscribe);
            _context.SaveChanges();

            return Redirect(Request.Headers["Referer"].ToString());
        }


    }
}
