using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Repositories
{
    public interface IWalkRepository
    {
        Task<List<Walk>> GetAllAsync(string? filterOn ,string? filterQuery,string? sortBy,bool asending,int pageSize,int pageNumber);
        Task<Walk> CreateAsync(Walk walk);
        Task<Walk> GetByIdAsync(Guid id);
        Task<Walk> UpdateAsync(Walk walk,Guid id);
        Task<Walk> DeleteAsync(Guid id);
    }
}
