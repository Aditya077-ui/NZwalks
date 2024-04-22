using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using NZwalks.API.Data;
using NZwalks.API.Models.Domain;

namespace NZwalks.API.Repositories
{
    public class SQLRegionRepository : IRegionRepository
    {
        private readonly NZwalksDbContext _nZwalksDbContext;
        public SQLRegionRepository(NZwalksDbContext nZwalksDbContext)
        {
            _nZwalksDbContext = nZwalksDbContext;
        }

        public async Task<Region> CreateAsync(Region region)
        {
            await _nZwalksDbContext.Regions.AddAsync(region);
            await _nZwalksDbContext.SaveChangesAsync();
            return region;
        }

        public async Task<Region?> DeleteAsync(Guid id)
        {
            var existingRegion =  await _nZwalksDbContext.Regions.FirstOrDefaultAsync(x=> x.Id == id);
            if (existingRegion == null) 
            {
                return null;
            }
            _nZwalksDbContext.Regions.Remove(existingRegion);
            await _nZwalksDbContext.SaveChangesAsync();
            return existingRegion;
        }


        public async Task<List<Region>> GetAllAsync()
        {
            return await _nZwalksDbContext.Regions.ToListAsync(); 
        }

        public async Task<Region?> GetByIdAsync(Guid id)
        {
            return await _nZwalksDbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);    
        }

        public async Task<Region?> UpdateAsync(Guid id ,Region region)
        {
            var existingfield = await _nZwalksDbContext.Regions.FirstOrDefaultAsync(x=>x.Id == id);
            if (existingfield == null)
            {
                return null;
            }
            
            existingfield.Code = region.Code;
            existingfield.Name = region.Name;
            existingfield.RegionImageURl = region.RegionImageURl;

            await _nZwalksDbContext.SaveChangesAsync();
            return existingfield;
        }
    }
}
