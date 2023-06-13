using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TheFruityMixologist.Entities;
using TheFruityMixologist.ViewModels;
using TheFruityMixologist.Areas.MixologistArea.ViewModels;

namespace TheFruityMixologist.DAL
{
    public class MixologistDbContext : IdentityDbContext<User>
    {
        public MixologistDbContext(DbContextOptions<MixologistDbContext> options) : base(options)
        {

        }
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<RecipesCategory> RecipesCategories { get; set; }
        public DbSet<RecipesImages> RecipesImages { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Step> Steps { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<PopularSlider> PopularSliders { get; set; }

        public DbSet<BasketItem> BasketItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<gifCart> GifCarts { get; set; }
        public DbSet<giftCartColor> giftCartColors { get; set; }
        public DbSet<PriceOption> priceOptions { get; set; }
        //public DbSet<Subscribe> Subscribe { get; set; }
        public DbSet<WishlistItem> WishlistItems { get; set; }
        public DbSet<EventSlider> EventSliders { get; set; }
        public DbSet<EventSliderImages> EventSliderImages { get; set; }
        public DbSet<Subscribe> Subscribes { get; set; }
        public DbSet<Setting> Settings { get; set; }





        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

           

            modelBuilder.Entity<IdentityUserToken<string>>().HasKey(t => new { t.UserId, t.LoginProvider, t.Name });

            modelBuilder.Entity<IdentityUserLogin<string>>().HasKey(l => new { l.LoginProvider, l.ProviderKey });
            foreach (var item in modelBuilder.Model
                                .GetEntityTypes()
                                        .SelectMany(e => e.GetProperties()
                                                    .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?))))
            {
                item.SetColumnType("decimal(6,2)");
            }

        }



















    }
}
