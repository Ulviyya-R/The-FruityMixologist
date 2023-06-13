using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TheFruityMixologist.DAL;
using TheFruityMixologist.Entities;
using TheFruityMixologist.Utilities.Extensions;

namespace TheFruityMixologist.Areas.MixologistArea.Controllers
{
    [Area("MixologistArea")]
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class SettingsController : Controller
    {


        private readonly MixologistDbContext _context;
        private readonly IWebHostEnvironment _env;

        public SettingsController(MixologistDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }


        public IActionResult Index()
        {
            IEnumerable<Setting> settings = _context.Settings.AsEnumerable();
            return View(settings);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Setting newSetting)
        {
            if (!ModelState.IsValid) return View();

            if (newSetting.Image != null && newSetting.Image.Length > 0)
            {
                var imagefolderPath = Path.Combine(_env.WebRootPath, "assets", "photos");
                newSetting.Value = await newSetting.Image.CreateImage(imagefolderPath, "Setting");

            }
            _context.Settings.Add(newSetting);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            if (id <= 0)
            {
                return NotFound();
            }
            Setting? setting = _context.Settings.FirstOrDefault(s => s.Id == id);
            if (setting is null)
            {
                return BadRequest();
            }
            return View(setting);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Setting editedSetting)
        {
            Setting? setting = _context.Settings.FirstOrDefault(s => s.Id == id);
            if (setting == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(setting);
            }

            _context.Entry<Setting>(setting).CurrentValues.SetValues(editedSetting);

            if (editedSetting.Image is not null)
            {
                string imagefolderPath = Path.Combine(_env.WebRootPath, "assets", "photos");
                string filepath = Path.Combine(imagefolderPath, "Setting", setting.Value);
                FileUpload.DeleteImage(filepath);
                setting.Value = await editedSetting.Image.CreateImage(imagefolderPath, "Setting");
            }

            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Details(int id)
        {
            if (id == 0) return NotFound();
            Setting? setting = _context.Settings.FirstOrDefault(s => s.Id == id);
            return setting is null ? BadRequest() : View(setting);
        }


        public IActionResult Delete(int id)
        {
            if (id == 0) return NotFound();
            Setting? setting = _context.Settings.FirstOrDefault(s => s.Id == id);
            if (setting is null) return NotFound();
            return View(setting);
        }

        [HttpPost]
        public IActionResult Delete(int id, Setting deleteSetting)
        {
            if (id != deleteSetting.Id) return NotFound();
            Setting? setting = _context.Settings.FirstOrDefault(s => s.Id == id);
            if (setting is null) return NotFound();
            string imagefolderPath = Path.Combine(_env.WebRootPath, "assets", "photos");
            string filepath = Path.Combine(imagefolderPath, "Setting");
            FileUpload.DeleteImage(filepath);
            _context.Settings.Remove(setting);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}