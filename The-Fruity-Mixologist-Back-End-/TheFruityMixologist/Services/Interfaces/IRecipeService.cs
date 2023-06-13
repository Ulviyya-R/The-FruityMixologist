using TheFruityMixologist.Entities;

namespace TheFruityMixologist.Services.Interfaces
{
    public interface IRecipeService
    {
        Task<Recipe> GetByIdAsync(int id);
        Task<IEnumerable<Recipe>> GetAllAsyncwithPaginated(int page);
        Task<IEnumerable<Recipe>> GetAllAsync();
        Task<gifCart> GiftGetFullDataByIdAsync(int id);
        Task<Recipe> GetFullDataByIdAsync(int id);
    }
}
