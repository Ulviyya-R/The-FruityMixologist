using Microsoft.EntityFrameworkCore;
using TheFruityMixologist.DAL;
using TheFruityMixologist.Entities;
using TheFruityMixologist.Services.Interfaces;

namespace TheFruityMixologist.Services
{
    public class RatingService : IRatingService
    {
        private readonly MixologistDbContext _context;

        public RatingService(MixologistDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Rating>> GetAllAsync() => await _context.Ratings.Include(m => m.Comment).ToListAsync();

    }
}
