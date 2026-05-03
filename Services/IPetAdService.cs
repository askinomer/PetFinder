using PetFinder.Models;

namespace PetFinder.Services;

public interface IPetAdService
{
    Task<List<PetAd>> GetAllAsync(string? species = null, string? city = null);
    Task<PetAd?> GetByIdAsync(int id);
    Task<PetAd> CreateAsync(PetAd petAd);
    Task<PetAd?> UpdateAsync(PetAd petAd);
    Task<bool> DeleteAsync(int id);
    Task<List<string>> GetDistinctCitiesAsync();
}
