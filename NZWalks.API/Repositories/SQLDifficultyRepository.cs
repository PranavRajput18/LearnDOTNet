using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Repositories
{
    public class SQLDifficultyRepository : IDifficultyRepository
    {
        private readonly NZWalksDbContext dbContext;

        public SQLDifficultyRepository(NZWalksDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Difficulty> CreateAsync(Difficulty difficulty)
        {
             await dbContext.Difficulties.AddAsync(difficulty);
             await dbContext.SaveChangesAsync();
            return difficulty;
           
        }

        public async Task<Difficulty> DeleteAsync(Guid id)
        {
            var existDifficulties = await dbContext.Difficulties.FindAsync(id);
            if (existDifficulties == null)
            {
                return null;
            }
            dbContext.Difficulties.Remove(existDifficulties);
            await dbContext.SaveChangesAsync();
            return existDifficulties;
        }

        public async Task<List<Difficulty>> GetAllAsync()
        {
           var difficulites =  await dbContext.Difficulties.ToListAsync();
            return difficulites;
        }

        public async Task<Difficulty> GetByIdAsync(Guid id)
        {
            var difficulties = await dbContext.Difficulties.FirstOrDefaultAsync(x=>x.Id == id);
            if (difficulties == null)
            {
                return null;
            }
            return difficulties;
        }

        public async Task<Difficulty> UpdateAsync(Difficulty difficulty,Guid id)
        {
            var existDiffculties = await dbContext.Difficulties.FindAsync(id);
            if (existDiffculties == null)
            {
                return null;
            }
            existDiffculties.Name = difficulty.Name;
            await dbContext.SaveChangesAsync();
            return existDiffculties;
        }
    }
}
