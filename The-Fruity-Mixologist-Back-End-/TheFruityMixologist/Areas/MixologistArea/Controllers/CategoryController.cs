using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using TheFruityMixologist.DAL;
using TheFruityMixologist.Entities;

namespace TheFruityMixologist.Areas.MixologistArea.Controllers
{
    [Area("MixologistArea")]
    [Authorize(Roles = "SuperAdmin,Admin")]

    public class CategoryController : Controller
    {
        private readonly MixologistDbContext _context;

        public CategoryController(MixologistDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            IEnumerable<Category> category = _context.Categories.AsEnumerable();
            return View(category);
        }
        public IActionResult Create(Category newcategory)
        {

            if (!ModelState.IsValid)
            {
                foreach (string message in ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage))
                {
                    ModelState.AddModelError("", message);
                }
                return View();
            }
            bool Isdublicate = _context.Categories.Any(c => c.Name == newcategory.Name);

            if (Isdublicate)
            {
                ModelState.AddModelError("", "You cannot enter the same data again");
                return View();
            }
            _context.Categories.Add(newcategory);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            if (id == 0) return NotFound();
            Category? category = _context.Categories.FirstOrDefault(c => c.Id == id);
            if (category is null) return NotFound();
            return View(category);
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Edit(int id, Category editCategory)
        {
            if (id != editCategory.Id) return NotFound();
            Category? category = _context.Categories.FirstOrDefault(c => c.Id == id);
            if (category is null) return NotFound();
            bool duplicate = _context.Categories.Any(c => c.Name == editCategory.Name && category.Name != editCategory.Name);
            if (duplicate)
            {
                ModelState.AddModelError("Name", "This  category name is now available");
                return View();
            }
            category.Name = editCategory.Name;
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }



        public IActionResult Details(int id)
        {
            if (id == 0) return NotFound();
            Category? category = _context.Categories.FirstOrDefault(c => c.Id == id);
            return category is null ? BadRequest() : View(category);
        }




        public IActionResult Delete(int id)
        {
            if (id == 0) return NotFound();
            Category? category = _context.Categories.FirstOrDefault(c => c.Id == id);
            if (category is null) return NotFound();
            return View(category);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Delete(int id, Category deleteCategory)
        {
            if (id != deleteCategory.Id) return NotFound();
            Category? category = _context.Categories.FirstOrDefault(c => c.Id == id);
            if (category is null) return NotFound();
            _context.Categories.Remove(category);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }


    }
}
