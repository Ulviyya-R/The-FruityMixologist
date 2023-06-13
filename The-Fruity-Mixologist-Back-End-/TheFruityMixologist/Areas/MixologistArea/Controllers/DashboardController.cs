using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using TheFruityMixologist.Areas.MixologistArea.ViewModels;
using TheFruityMixologist.DAL;
using TheFruityMixologist.Entities;
using TheFruityMixologist.Utilities.Enum;

namespace TheFruityMixologist.Areas.MixologistArea.Controllers
{
    [Area("MixologistArea")]
    [Authorize(Roles = "SuperAdmin")]
    public class DashboardController:Controller
    {
        private readonly MixologistDbContext _context;

        public DashboardController(MixologistDbContext context)
        {
            _context = context;
        }

        public IActionResult Dashboard()
        {
            List<Recipe> recipes = _context.Recipes.Include(x => x.RecipesCategories).ThenInclude(rc => rc.Category).ToList();
            List<Category> categories = _context.Categories.Include(x => x.RecipesCategories).ThenInclude(rc=>rc.Recipes).ToList();
            List<CategoryDashboardVM> categoryProductNames = categories.Select(cat => new CategoryDashboardVM
            {
                CategoryName = cat.Name,
                ProductCount = recipes.Count(p => p.RecipesCategories.Any(rc=>rc.CategoryId == cat.Id)),
            }).ToList();
            var categoryLabels = categories.Select(cat => cat.Name).ToList();
            var categoryData = categoryProductNames.Select(item => item.ProductCount).ToList();
            ViewBag.CategoryLabels = categoryLabels;
            ViewBag.CategoryData = categoryData;

            List<User> users = _context.Users.Include(x => x.Orders).ToList();
            List<UserOrderDashboardVM> userStatistics = users.Select(u => new UserOrderDashboardVM
            {
                UserId = u.Id,
                UserName = u.UserName,
                OrderCount = u.Orders.Count
            })
            .OrderByDescending(stat => stat.OrderCount)
            .ToList();

            var userLabels = userStatistics.Select(stat => stat.UserName).ToList();
            var userData = userStatistics.Select(stat => stat.OrderCount).ToList();

            ViewBag.UserLabels = userLabels;
            ViewBag.UserData = userData;

            return View();
        }


        ////public IActionResult Index()
        ////{
        ////    DashboardViewModel dashboardVM = new DashboardViewModel
        ////    {
        ////        AcceptedOrders = _context.Orders.Where(x => x.Status == OrderStatus.Accepted).Include(x => x.OrderItems).ToList(),
        ////        PendingOrders = _context.Orders.Where(x => x.Status == OrderStatus.Pending).Include(x => x.OrderItems).ToList(),
        ////        ////Users = _context.Users.Where(x => x.IsAdmin == false).ToList(),
        ////        LatestOrders = _context.Orders.OrderByDescending(x => x.OrderItems.).Take(5).ToList(),
        ////        RedWines = _context.Products.Where(x => x.Type.Name == "Red").Count(),
        ////        WhiteWines = _context.Products.Where(x => x.Type.Name == "White").Count(),
        ////        RoseWines = _context.Products.Where(x => x.Type.Name == "Rose").Count(),
        ////        SaturdayOrders = _context.Orders.Where(x => x.CreatedAt.Day == 1 && x.Status == OrderStatus.Accepted).Count(),
        ////        SaturdayIncome = _context.Orders.Where(x => x.CreatedAt.Day == 1 && x.Status == OrderStatus.Accepted).Sum(x => x.TotalAmount),
        ////        SundayOrders = _context.Orders.Where(x => x.CreatedAt.Day == 2 && x.Status == OrderStatus.Accepted).Count(),
        ////        SundayIncome = _context.Orders.Where(x => x.CreatedAt.Day == 2 && x.Status == OrderStatus.Accepted).Sum(x => x.TotalAmount),
        ////        MondayOrders = _context.Orders.Where(x => x.CreatedAt.Day == 3 && x.Status == OrderStatus.Accepted).Count(),
        ////        MondayIncome = _context.Orders.Where(x => x.CreatedAt.Day == 3 && x.Status == OrderStatus.Accepted).Sum(x => x.TotalAmount),
        ////        TuesdayOrders = _context.Orders.Where(x => x.CreatedAt.Day == 4 && x.Status == OrderStatus.Accepted).Count(),
        ////        TuesdayIncome = _context.Orders.Where(x => x.CreatedAt.Day == 4 && x.Status == OrderStatus.Accepted).Sum(x => x.TotalAmount),
        ////        WednesdayOrders = _context.Orders.Where(x => x.CreatedAt.Day == 5 && x.Status == OrderStatus.Accepted).Count(),
        ////        WednesdayIncome = _context.Orders.Where(x => x.CreatedAt.Day == 5 && x.Status == OrderStatus.Accepted).Sum(x => x.TotalAmount),
        ////        ThursdayOrders = _context.Orders.Where(x => x.CreatedAt.Day == 6 && x.Status == OrderStatus.Accepted).Count(),
        ////        ThursdayIncome = _context.Orders.Where(x => x.CreatedAt.Day == 6 && x.Status == OrderStatus.Accepted).Sum(x => x.TotalAmount),
        ////        FridayOrders = _context.Orders.Where(x => x.CreatedAt.Day == 7 && x.Status == OrderStatus.Accepted).Count(),
        ////        FridayIncome = _context.Orders.Where(x => x.CreatedAt.Day == 7 && x.Status == OrderStatus.Accepted).Sum(x => x.TotalAmount)

        ////    };
        ////}
    }
}
