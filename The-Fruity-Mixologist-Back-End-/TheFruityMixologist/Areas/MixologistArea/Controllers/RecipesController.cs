using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TheFruityMixologist.DAL;
using TheFruityMixologist.Entities;
using TheFruityMixologist.Migrations;
using TheFruityMixologist.Services;
using TheFruityMixologist.Services.Interfaces;
using TheFruityMixologist.Utilities.Extensions;
using TheFruityMixologist.ViewModels;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace TheFruityMixologist.Areas.MixologistArea.Controllers
{
    [Area("MixologistArea")]
    [Authorize(Roles = "SuperAdmin,Admin")]

    public class RecipesController:Controller
    {
        private readonly MixologistDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly IRecipeService _recipeService;
        private readonly IEmailService _emailService;


        public RecipesController(MixologistDbContext context, IWebHostEnvironment env,IRecipeService recipeService,IEmailService emailService)
        {
            _context = context;
            _env = env;
            _recipeService = recipeService;
            _emailService = emailService;
        }
        public async Task<IActionResult> Index(int page = 1)
        {
            if (page <= 0)
            {
                page = 1;
            }
            var query = _context.Recipes.AsQueryable();
            ViewBag.SelectedPage = page;
            ViewBag.TotalPage = Math.Ceiling(query.Count() / 9m);

            IEnumerable<Recipe> recipes = await _recipeService.GetAllAsyncwithPaginated(page);
            return View(recipes);
        }

        public IActionResult Create()
        {
            AllViewBagsData();
            return View();
        }
        

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RecipeVM newRecipe)
        {
            AllViewBagsData();
            TempData["InvalidImages"] = string.Empty;
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (!newRecipe.MainPhoto.IsValidFile("image/") || !newRecipe.FalseImages.IsValidFile("image/"))
            {
                ModelState.AddModelError(string.Empty, "Please choose image file");
                return View();
            }
            if (!newRecipe.MainPhoto.IsValidLength(5) || !newRecipe.FalseImages.IsValidLength(5))
            {
                ModelState.AddModelError(string.Empty, "Please choose image which size is maximum 1MB");
                return View();
            }

            Recipe recipe = new()
            {
                Name = newRecipe.Name,
                Desc = newRecipe.Desc,
                Price = newRecipe.Price,
            };
            string imageFolderPath = Path.Combine(_env.WebRootPath, "assets", "photos");
          

                RecipesImages falsePhoto = new()
                {
                    IsMain = false,
                    Path = await newRecipe.FalseImages.CreateImage(imageFolderPath, "Recipes")
                };
            recipe.RecipesImages.Add(falsePhoto);


            RecipesImages main = new()
            {
                IsMain = true,

                Path = await newRecipe.MainPhoto.CreateImage(imageFolderPath, "Recipes")
            };
            recipe.RecipesImages.Add(main);

            foreach (int id in newRecipe.CategoryIds)
            {
                RecipesCategory category = new()
                {
                    CategoryId = id,
                };
                recipe.RecipesCategories.Add(category);
            }

            ///STEPS

            if (newRecipe.Steps is null)
            {
                ModelState.AddModelError("", "Please write work's info");
                return View();
            }
            else
            {
                string[] step_info = newRecipe.Steps.Split('/');
                foreach (string steps in step_info)
                {
                    Step step = new()
                    {
                        Recipes = recipe,
                        Steps = steps,
                    };

                    _context.Steps.Add(step);
                }
            }

            //INGREDIENTS

            if (newRecipe.Ingredients is null)
            {
                ModelState.AddModelError("", "Please write work's info");
                return View();
            }
            else
            {
                string[] ing_info = newRecipe.Ingredients.Split('/');
                foreach (string ings in ing_info)
                {
                    Ingredient ingredient = new()
                    {
                        Recipes = recipe,
                        Ingredients = ings,
                    };

                    _context.Ingredients.Add(ingredient);
                }
            }

         

            _context.Recipes.Add(recipe);
            _context.SaveChanges();
            string urlMessage = Url.Action("Detail", "Shop", new { id = recipe.Id }, Request.Scheme);
            urlMessage = urlMessage.Replace("/MixologistArea", "");
            return RedirectToAction("SendMail", new { urlMessage });
        }
        public IActionResult Edit(int id)
        {
            if (id == 0) return BadRequest();
            RecipeVM? model = EditedModel(id);
            AllViewBagsData();
            if (model is null) return BadRequest();
            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(int id, RecipeVM editedRecipes)
        {
            AllViewBagsData();
            RecipeVM? model = EditedModel(id);

            Recipe? recipe = await _context.Recipes.Include(p => p.RecipesCategories)
                                                .Include(p => p.RecipesImages).Include(p=>p.Ingredients).Include(p=>p.Steps)
                                                .FirstOrDefaultAsync(p => p.Id == id);
            if (recipe is null) return BadRequest();

            string imageFolderPath = Path.Combine(_env.WebRootPath, "assets", "photos");

            if (editedRecipes.MainPhoto is not null)
            {
                if (!editedRecipes.MainPhoto.IsValidFile("image/"))
                {
                    ModelState.AddModelError(string.Empty, "Please choose image file");
                    return View();
                }
                if (!editedRecipes.MainPhoto.IsValidLength(5))
                {
                    ModelState.AddModelError(string.Empty, "Please choose image which size is maximum 5MB");
                    return View();
                }
                await AdjustPlantPhoto(true, editedRecipes.MainPhoto, recipe);
            }

            if (editedRecipes.FalseImages is not null)
            {
                if (!editedRecipes.FalseImages.IsValidFile("image/"))
                {
                    ModelState.AddModelError(string.Empty, "Please choose image file");
                    return View();
                }
                if (!editedRecipes.FalseImages.IsValidLength(5))
                {
                    ModelState.AddModelError(string.Empty, "Please choose image which size is maximum 5MB");
                    return View();
                }
                await AdjustPlantPhoto(false, editedRecipes.FalseImages, recipe);
            }
            recipe.Name = editedRecipes.Name;
            recipe.Price = editedRecipes.Price;
            recipe.DiscountPrice = (decimal)editedRecipes.DiscountPrice;
            recipe.Desc = editedRecipes.Desc;
            //TODO Edit Category 
            recipe.RecipesCategories.Clear();
            EditCategories(recipe, editedRecipes);      
            //TODO Edit Ingredients
            recipe.Ingredients.RemoveAll(p => !editedRecipes.DeleteIngredients.Contains(p.Id));
            if (editedRecipes.Ingredients is not null)
            {
                string[] ing_info = editedRecipes.Ingredients.Split('/');
                foreach (string ings in ing_info)
                {
                    Ingredient ing = new()
                    {
                        Recipes = recipe,
                        Ingredients = ings,
                    };

                    _context.Ingredients.Add(ing);
                }
            }

            //TODO Edit Steps
            recipe.Steps.RemoveAll(p => !editedRecipes.DeleteSteps.Contains(p.Id));
            if (editedRecipes.Steps is not null)
            {
                string[] step_info = editedRecipes.Steps.Split('/');
                foreach (string steps in step_info)
                {
                    Step step = new()
                    {
                        Recipes = recipe,
                        Steps = steps,
                    };

                    _context.Steps.Add(step);
                }
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Details(int id)
        {
            if (id == 0) return NotFound();
            AllViewBagsData();
            RecipeVM? VM = EditedModel(id);
            return VM is null ? BadRequest() : View(VM);
        }

        public IActionResult Delete(int id)
        {
            if (id == 0) return NotFound();
            AllViewBagsData();
            RecipeVM? VM = EditedModel(id);
            if (VM is null) return NotFound();
            return View(VM);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id, RecipeVM deleterecipe)
        {
            if (id != deleterecipe.Id) return NotFound();
            Recipe? recipe = await _recipeService.GetFullDataByIdAsync(id);
            if (recipe is null) return NotFound();
            List<RecipesImages> images = recipe.RecipesImages.ToList();
            string imagefolderPath = Path.Combine(_env.WebRootPath, "assets", "photos");
            foreach (var removable in images)
            {
                string filepath = Path.Combine(imagefolderPath, "Recipes", removable.Path);
                FileUpload.DeleteImage(filepath);
            }
            if(recipe != null)
{
                _context.Recipes.Remove(recipe);
            }
           
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> SendMail(string urlMessage)
        {
            List<Subscribe> subscribes = _context.Subscribes.ToList();
            foreach (Subscribe email in subscribes)
            {
                string recipientEmail = email.Email;
                string subject = "New Cocktail";
                string body = $"New Cocktail added: <br> {urlMessage}";

                _emailService.Send(recipientEmail, subject, body);
            }

            return RedirectToAction("Index");
        }






        private void AllViewBagsData()
        {
            ViewBag.Categories = _context.Categories.AsEnumerable();
        }

        private async Task AdjustPlantPhoto(bool? Ismain, IFormFile image, Recipe recipe)
        {
            string imagefolderPath = Path.Combine(_env.WebRootPath, "assets", "photos");
            string filePath = Path.Combine(imagefolderPath, "Recipes", recipe.RecipesImages.FirstOrDefault(p => p.IsMain == Ismain).Path);
            FileUpload.DeleteImage(filePath);
            recipe.RecipesImages.FirstOrDefault(p => p.IsMain == Ismain).Path = await image.CreateImage(imagefolderPath, "Recipes");
        }


        private void EditCategories(Recipe recipe, RecipeVM editedRecipe)
        {
            foreach (int categoryId in editedRecipe.CategoryIds)
            {
                Category? category = _context.Categories.FirstOrDefault(c => c.Id == categoryId);
                if (category != null)
                {
                    RecipesCategory recipeCategory = new()
                    {
                        Category = category
                    };
                    recipe.RecipesCategories.Add(recipeCategory);
                }
            }

        }

        private RecipeVM? EditedModel(int id)
        {
            RecipeVM? recipeVM = _context.Recipes.Include(v => v.RecipesImages).
               Include(e => e.RecipesCategories).
               Include(e => e.Steps).
               Include(c => c.Ingredients)
               .Select(r =>
                                        new RecipeVM
                                        {
                                            Id = r.Id,
                                            Name = r.Name,
                                            Desc = r.Desc,
                                            Price = r.Price,
                                            DiscountPrice = r.DiscountPrice,
                                            AllIngredient = r.Ingredients,
                                            AllSteps=  r.Steps,
                                            CategoryIds = r.RecipesCategories.Select(p => p.CategoryId).ToList(),
                                            AllImages = r.RecipesImages.Select(i => new RecipesImages
                                            {
                                                Id = i.Id,
                                                IsMain = i.IsMain,
                                                Path = i.Path
                                            }).ToList()
                                        }).FirstOrDefault(x => x.Id == id);
            return recipeVM;
        }


    }
    }



