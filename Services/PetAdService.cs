using Microsoft.EntityFrameworkCore;
using PetFinder.DataAccessLayer;
using PetFinder.Models;

namespace PetFinder.Services;

public class PetAdService : IPetAdService
{
    private readonly AppDbContext _db;

    public PetAdService(AppDbContext db) => _db = db;

    public async Task<List<PetAd>> GetAllAsync(string? species = null, string? city = null)
    {
        // LINQ filtering by species and/or city
        var query = _db.PetAds.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(species))
            query = query.Where(p => p.Species == species);

        if (!string.IsNullOrWhiteSpace(city))
            query = query.Where(p => p.City.Contains(city));

        return await query.OrderByDescending(p => p.Id).ToListAsync();
    }

    public async Task<PetAd?> GetByIdAsync(int id) =>
        await _db.PetAds.FirstOrDefaultAsync(p => p.Id == id);

    public async Task<PetAd> CreateAsync(PetAd petAd)
    {
        _db.PetAds.Add(petAd);
        await _db.SaveChangesAsync();
        return petAd;
    }

    public async Task<PetAd?> UpdateAsync(PetAd petAd)
    {
        var existing = await _db.PetAds.FirstOrDefaultAsync(p => p.Id == petAd.Id);
        if (existing == null) return null;

        existing.Name = petAd.Name;
        existing.Species = petAd.Species;
        existing.City = petAd.City;
        existing.Description = petAd.Description;

        // Only replace the image if a new one was uploaded
        if (petAd.ImageBytes != null && petAd.ImageBytes.Length > 0)
            existing.ImageBytes = petAd.ImageBytes;

        await _db.SaveChangesAsync();
        return existing;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _db.PetAds.FindAsync(id);
        if (entity == null) return false;
        _db.PetAds.Remove(entity);
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<List<string>> GetDistinctCitiesAsync() =>
        await _db.PetAds.AsNoTracking()
            .Select(p => p.City)
            .Distinct()
            .OrderBy(c => c)
            .ToListAsync();
}
