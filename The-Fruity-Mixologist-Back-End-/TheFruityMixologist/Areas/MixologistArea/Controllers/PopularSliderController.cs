using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TheFruityMixologist.DAL;
using TheFruityMixologist.Entities;
using TheFruityMixologist.Utilities.Extensions;

namespace TheFruityMixologist.Areas.MixologistArea.Controllers
{
    [Area("MixologistArea")]
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class PopularSliderController:Controller
    {
        private readonly MixologistDbContext _context;
        private readonly IWebHostEnvironment _env;


        public PopularSliderController(MixologistDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public IActionResult Index()
        {
            IEnumerable<PopularSlider> popSlider = _context.PopularSliders.AsEnumerable();
            return View(popSlider);
        }

        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Create(PopularSlider newSlider)
        {
            if (newSlider.Image is null)
            {
                ModelState.AddModelError("Image", "Please Select Image");
                return View();
            }
            if (!newSlider.Image.IsValidFile("image/"))
            {
                ModelState.AddModelError("Image", "Please Select Image Tag");
                return View();
            }
            if (!newSlider.Image.IsValidLength(2))
            {
                ModelState.AddModelError("Image", "Please Select Image which size max 2MB");
                return View();
            }
            if (!Imports(newSlider))
            {
                return View();
            }
            var imagefolderPath = Path.Combine(_env.WebRootPath, "assets", "photos","Home");
            newSlider.ImagePath = await newSlider.Image.CreateImage(imagefolderPath, "Slider");
            _context.PopularSliders.Add(newSlider);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            if (id <= 0)
            {
                return NotFound();
            }
            PopularSlider slider = _context.PopularSliders.FirstOrDefault(s => s.Id == id);
            if (slider is null)
            {
                return BadRequest();
            }
            return View(slider);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, PopularSlider edited)
        {
            if (id != edited.Id) return NotFound();
            PopularSlider slider = _context.PopularSliders.FirstOrDefault(s => s.Id == id);
            if (!ModelState.IsValid) return View(slider);
            _context.Entry<PopularSlider>(slider).CurrentValues.SetValues(edited);

            if (edited.Image is not null)
            {
                string imagefolderPath = Path.Combine(_env.WebRootPath, "assets", "photos", "Home");
                string filepath = Path.Combine(imagefolderPath, "Slider", slider.ImagePath);
                FileUpload.DeleteImage(filepath);
                slider.ImagePath = await edited.Image.CreateImage(imagefolderPath, "Slider");
            }
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }


        public IActionResult Details(int id)
        {
            if (id == 0) return NotFound();
            PopularSlider? slider = _context.PopularSliders.FirstOrDefault(s => s.Id == id);
            return slider is null ? BadRequest() : View(slider);
        }


        public IActionResult Delete(int id)
        {
            if (id == 0) return NotFound();
            PopularSlider? slider = _context.PopularSliders.FirstOrDefault(s => s.Id == id);
            if (slider is null) return NotFound();
            return View(slider);
        }

        [HttpPost]
        public IActionResult Delete(int id, PopularSlider deleteslider)
        {
            if (id != deleteslider.Id) return NotFound();
            PopularSlider? slider = _context.PopularSliders.FirstOrDefault(s => s.Id == id);
            if (slider is null) return NotFound();
            string imagefolderPath = Path.Combine(_env.WebRootPath, "assets", "photos", "Home");
            string filepath = Path.Combine(imagefolderPath, "Slider", slider.ImagePath);
            FileUpload.DeleteImage(filepath);
            _context.PopularSliders.Remove(slider);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }





        bool Imports(PopularSlider newSlider)
        {
            if (newSlider.Name is null)
            {
                ModelState.AddModelError("", "Note the SliderName!");
                return false;
            }
            //if (newSlider.Date is null)
            //{
            //    ModelState.AddModelError("", "Note the Desc!");
            //    return false;
            //}
            //if (newSlider.Time is null)
            //{
            //    ModelState.AddModelError("", "Note the ButtonText!");
            //    return false;
            //}
            if (newSlider.Image is null)
            {
                ModelState.AddModelError("", "Note the LeftIcon!");
                return false;
            }
            return true;
        }
    }
}
