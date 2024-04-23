using Microsoft.EntityFrameworkCore;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories;

public interface IWalkRepository
{
    Task<List<Walk>> GetAllAsync(string? filterBy, string? filterQuery,
    string? orderBy, bool isAscending,
    int pageNum, int pageSize);

    Task<Walk?> GetByIdAsync(Guid id);

    Task<Walk?> UpdateAsync(Guid id, Walk walk);
    Task<Walk> CreateAsync(Walk walk);

    Task<Walk?> DeleteAsync(Guid id);

}