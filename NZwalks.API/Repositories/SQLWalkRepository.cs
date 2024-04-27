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

        public async Task<Walk?> DeleteAsync(Guid id)
        {
            var existingwalk = await _nZwalksDbContext1.Walks.FirstOrDefaultAsync(x => x.Id == id);
            if (existingwalk == null) {
                return null;
            }
            _nZwalksDbContext1.Walks.Remove(existingwalk);
            await _nZwalksDbContext1.SaveChangesAsync();
            return existingwalk;
        }

        public async Task<List<Walk>> GetAllAsync(string? filterOn = null, string? filterQuery = null,
            string? sortby = null,bool IsAscending = true)
        {
            var walks =  _nZwalksDbContext1.Walks.Include("Difficulty").Include("Region").AsQueryable();

            //filtering according to column name as filterOn 
            if (string.IsNullOrWhiteSpace(filterOn) == false && string.IsNullOrWhiteSpace(filterQuery) == false) 
            {
                if(filterOn.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    walks = walks.Where(x => x.Name.Contains(filterQuery));
                }
            }

            //sorting (order by)
            if(string.IsNullOrWhiteSpace(sortby) == false)
            {
                if (sortby.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    walks = IsAscending ? walks.OrderBy(x=>x.Name) : walks.OrderByDescending(x=>x.Name);
                }
                else if (sortby.Equals("Length", StringComparison.OrdinalIgnoreCase))
                {
                    walks = IsAscending ? walks.OrderBy(x=>x.LengthInKM) : walks.OrderByDescending(x=>x.LengthInKM);
                }
            }


            return await walks.ToListAsync();
        }

        public async Task<Walk?> GetByIdAsync(Guid id)
        {
           return await _nZwalksDbContext1.Walks.Include("Difficulty").Include("Region").FirstOrDefaultAsync(x=>x.Id == id);

        }

        public async Task<Walk?> UpdateAsync(Guid id, Walk walk)
        {
            var existingwalkmodel = await _nZwalksDbContext1.Walks.FirstOrDefaultAsync(x=> x.Id == id);   
            if (existingwalkmodel == null) {
                return null;
            }
             
            existingwalkmodel.Name = walk.Name;
            existingwalkmodel.Description = walk.Description;
            existingwalkmodel.LengthInKM = walk.LengthInKM;
            existingwalkmodel.WalkImageURl = walk.WalkImageURl;
            existingwalkmodel.DifficultyId = walk.DifficultyId;
            existingwalkmodel.RegionId = walk.RegionId;

            await _nZwalksDbContext1.SaveChangesAsync();
            return existingwalkmodel;
        }
    }
}
