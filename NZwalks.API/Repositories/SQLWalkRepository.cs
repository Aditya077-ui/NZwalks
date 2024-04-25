using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using NZwalks.API.Data;
using NZwalks.API.Models.Domain;

namespace NZwalks.API.Repositories
{
    public class SQLWalkRepository : IWalkRepository
    {
        private readonly NZwalksDbContext _nZwalksDbContext1;
        public SQLWalkRepository(NZwalksDbContext nZwalksDbContext)
        {
            _nZwalksDbContext1 = nZwalksDbContext;
        }

        public async Task<Walk> CreateAsync(Walk walk)
        {
            await _nZwalksDbContext1.Walks.AddAsync(walk);  
            await _nZwalksDbContext1.SaveChangesAsync();
            return walk;
        }

        public async Task<List<Walk>> GetAllAsync()
        {
            return await _nZwalksDbContext1.Walks.ToListAsync();
        }
    }
}
