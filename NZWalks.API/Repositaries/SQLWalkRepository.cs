using Microsoft.EntityFrameworkCore;
using NZWalks.API.Models.Domain;
using NZWalks.API.Data;

namespace NZWalks.API.Repositories;

public class SQLWalkRepository : IWalkRepository
{
    private readonly NZWalksDbContext dbContext;

    public SQLWalkRepository(NZWalksDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<List<Walk>> GetAllAsync(string? filterBy, string? filterQuery,
    string? orderBy, bool isAscending, int pageNum, int pageSize)
    {
        var walks = dbContext.Walks.Include("Difficulty").Include("Region").AsQueryable<Walk>();

        // Filter
        if (!string.IsNullOrWhiteSpace(filterBy) && !string.IsNullOrWhiteSpace(filterQuery))
        {
            if (filterBy.Equals("name", StringComparison.OrdinalIgnoreCase))
            {
                walks = walks.Where(x => x.Name.Contains(filterQuery));
            }

        }

        // Order
        if (!string.IsNullOrWhiteSpace(orderBy))
        {
            if (orderBy.Equals("name", StringComparison.OrdinalIgnoreCase))
            {
                walks = isAscending ? walks.OrderBy(x => x.Name) : walks.OrderByDescending(x => x.Name);
            }
            else if (orderBy.Equals("length", StringComparison.OrdinalIgnoreCase))
            {
                walks = isAscending ? walks.OrderBy(x => x.LengthInKm) : walks.OrderByDescending(x => x.LengthInKm);
            }
        }

        // Pagenation
        var skipNum = (pageNum - 1) * pageSize;


        return await walks.Skip(skipNum).Take(pageSize).ToListAsync();
    }

    public async Task<Walk> CreateAsync(Walk walk)
    {
        await dbContext.Walks.AddAsync(walk);
        await dbContext.SaveChangesAsync();

        return walk;
    }

    public async Task<Walk?> GetByIdAsync(Guid id)
    {
        return await dbContext.Walks.Include("Difficulty").Include("Region").FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Walk?> UpdateAsync(Guid id, Walk walk)
    {
        var existingWalk = await dbContext.Walks.FindAsync(id);
        if (existingWalk == null)
        {
            return null;
        }
        existingWalk.Name = walk.Name;
        existingWalk.Description = walk.Description;
        existingWalk.LengthInKm = walk.LengthInKm;
        existingWalk.WalkImageUrl = walk.WalkImageUrl;
        existingWalk.DifficultyId = walk.DifficultyId;
        existingWalk.RegionId = walk.RegionId;

        await dbContext.SaveChangesAsync();

        Console.WriteLine("existingWalk", existingWalk);

        return existingWalk;
    }

    public async Task<Walk?> DeleteAsync(Guid id)
    {
        var existingWalk = await dbContext.Walks.FirstOrDefaultAsync(x => x.Id == id);

        if (existingWalk == null)
        {
            return null;
        }

        dbContext.Walks.Remove(existingWalk);
        await dbContext.SaveChangesAsync();

        return existingWalk;
    }
}
