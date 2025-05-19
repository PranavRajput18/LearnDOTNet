using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Repositories
{
    public class SQLWalkRepository : IWalkRepository
    {
        private readonly NZWalksDbContext dbContext;
        private readonly IMapper mapper;

        public SQLWalkRepository(NZWalksDbContext dbContext,IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }
        public async Task<Walk> CreateAsync(Walk walk )
        {
            await dbContext.AddAsync(walk);
            await dbContext.SaveChangesAsync();
            return walk;
        }

        public async Task<Walk> DeleteAsync(Guid id)
        {
            var existWalk = await dbContext.Walks.FirstOrDefaultAsync(x => x.Id == id);
            if (existWalk == null) 
            {
                return null;
            }
            dbContext.Walks.Remove(existWalk);
            await dbContext.SaveChangesAsync();
            return existWalk;
        }

        public async Task<List<Walk>> GetAllAsync(string? filtetOn,string? filterQuery,string? sortBy,bool isAsending,int pageNumber,int pageSize)
        {
            var walks = dbContext.Walks.Include("Difficulty").Include("Region").AsQueryable();

            //Filtering
            if (!string.IsNullOrEmpty(filtetOn) && !string.IsNullOrEmpty(filterQuery))
            {
                if (filtetOn.Equals("Name",StringComparison.OrdinalIgnoreCase))
                {
                    walks = walks.Where(x => x.Name.Contains(filterQuery));
                }
            }

            //Sorting
            if (!string.IsNullOrEmpty(sortBy))
            {
                walks = isAsending ? walks.OrderBy(x=>x.Name):walks.OrderByDescending(x=>x.Name);
            }

            //Pagination
            var skipResults = (pageNumber -1) * pageSize;

            return await walks.Skip(skipResults).Take(pageSize).ToListAsync();
            
        }

        public async Task<Walk> GetByIdAsync(Guid id)
        {
           var walk = await dbContext.Walks.Include("Difficulty").Include("Region").FirstOrDefaultAsync(x => x.Id==id);
            return walk;
        }

        public async Task<Walk> UpdateAsync(Walk walk,Guid id)
        {
            var existWalk =  await dbContext.Walks.FirstOrDefaultAsync(x => x.Id == id);
            if (existWalk == null)
            {
                return null;
            }
            //Map Model to Dto 

            existWalk.Name = walk.Name;
            existWalk.Description = walk.Description;
            existWalk.LengthInKm = walk.LengthInKm;
            existWalk.WalkImageUrl = walk.WalkImageUrl;
            existWalk.RegionId = walk.RegionId;
            existWalk.DifficultyId = walk.DifficultyId;


            await dbContext.SaveChangesAsync();
            return existWalk;
        }
    }
}
