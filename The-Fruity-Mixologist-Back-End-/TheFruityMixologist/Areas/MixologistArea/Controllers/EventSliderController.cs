using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Differencing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using TheFruityMixologist.DAL;
using TheFruityMixologist.Entities;
using TheFruityMixologist.Utilities.Extensions;
using TheFruityMixologist.ViewModels;

namespace TheFruityMixologist.Areas.MixologistArea.Controllers
{
    [Area("MixologistArea")]
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class EventSliderController : Controller
    {
        private readonly MixologistDbContext _context;
        private readonly IWebHostEnvironment _env;


        public EventSliderController(MixologistDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public IActionResult Index()
        {
            IEnumerable<EventSlider> eventSlider = _context.EventSliders.AsEnumerable();
            return View(eventSlider);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(EventSliderVM newSlider)
        {
            
            if (!ModelState.IsValid)
            {
                foreach (string message in ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage))
                {
                    ModelState.AddModelError("", message);
                }
                return View();
            }
            EventSlider slider = new EventSlider();
            slider.EventName = newSlider.EventName;
            slider.EventData = newSlider.EventData;
            string imageFolderPath = Path.Combine(_env.WebRootPath, "assets", "photos");

            foreach (var image in newSlider.Images)
            {
                if (!image.IsValidFile("image/") || !image.IsValidLength(5))
                {
                    TempData["InvalidImages"] += image.FileName;
                    continue;
                }

                EventSliderImages eventSliderImages = new()
                {
                    EventSlider = slider,
                    Path = await image.CreateImage(imageFolderPath, "partyEvents")
                };
                _context.EventSliderImages.Add(eventSliderImages);
                slider.eventSliderImages.Add(eventSliderImages);

            }
            _context.EventSliders.Add(slider);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));

        }



        public IActionResult Edit(int id)
        {
            if (id == 0) return BadRequest();
            EventSliderVM? model = EditedModel(id);
            if (model is null) return BadRequest();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, EventSliderVM edited)
        {
            EventSliderVM? model = EditedModel(id);
            if (id != edited.Id) return NotFound();
            EventSlider? slider = _context.EventSliders.Include(x=>x.eventSliderImages).FirstOrDefault(s => s.Id == id);
            if (slider is null) return BadRequest();
            if (!ModelState.IsValid) return View(slider);
            IEnumerable<string> removables = slider.eventSliderImages.Where(p => !edited.ImagesId.Contains(p.Id)).Select(i => i.Path).AsEnumerable();
            string imageFolderPath = Path.Combine(_env.WebRootPath, "assets","photos");
            foreach (string removable in removables)
            {
                string path = Path.Combine(imageFolderPath, "partyEvents", removable);
                FileUpload.DeleteImage(path);
            }
            if (edited.Images is not null)
            {
                foreach (var images in edited.Images)
                {
                    if (!images.IsValidFile("image/") || !images.IsValidLength(5))
                    {
                        TempData["NonSelect"] += images.FileName;
                        continue;
                    }
                    EventSliderImages eventImages = new()
                    {
                        Path = await images.CreateImage(imageFolderPath, "partyEvents")
                    };
                    slider.eventSliderImages.Add(eventImages);
                }
             }
            slider.eventSliderImages.RemoveAll(p => !edited.ImagesId.Contains(p.Id));
            slider.EventName = edited.EventName;
            slider.EventData = edited.EventData;

            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Details(int id)
        {
            if (id == 0) return NotFound();
            EventSliderVM? VM = EditedModel(id);
            return VM is null ? BadRequest() : View(VM);
        }

        public IActionResult Delete(int id)
        {
            if (id == 0) return NotFound();
            EventSliderVM? VM = EditedModel(id);
            if (VM is null) return NotFound();
            return View(VM);
        }

        [HttpPost]
        public IActionResult Delete(int id, EventSliderVM deleteevent)
        {
            if (id != deleteevent.Id) return NotFound();
            EventSlider? slider = _context.EventSliders.Include(x=>x.eventSliderImages).FirstOrDefault(e => e.Id == id);
            if (slider is null) return NotFound();
            List<EventSliderImages> images= slider.eventSliderImages.ToList();
            string imagefolderPath = Path.Combine(_env.WebRootPath, "assets", "photos");
            foreach (var removable in images)
            {
                string filepath = Path.Combine(imagefolderPath, "partyEvents", removable.Path);
                FileUpload.DeleteImage(filepath);
            }
            _context.EventSliders.Remove(slider);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        



        private EventSliderVM? EditedModel(int id)
        {
            EventSliderVM? model = _context.EventSliders.
                    Include(pc => pc.eventSliderImages).
                            Select(p => new EventSliderVM
                            {
                                Id= p.Id,
                                EventName=p.EventName,
                                EventData = p.EventData,
                                AllImages = p.eventSliderImages.Select(i => new EventSliderImages
                                {
                                    Id = i.Id,
                                    Path = i.Path
                                }).ToList()
                            }).FirstOrDefault(p => p.Id == id);
            return model;
        }
    }
}
