using TheFruityMixologist.Entities;

namespace TheFruityMixologist.Services.Interfaces
{
    public interface IRatingService
    {
        Task<IEnumerable<Rating>> GetAllAsync();
    }
}
