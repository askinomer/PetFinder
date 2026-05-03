using PetFinder.Models;

namespace PetFinder.Services;

public interface IUserService
{
    Task<User?> ValidateAsync(string username, string password);
    Task<User?> GetByIdAsync(int id);
    Task<User?> GetByUsernameAsync(string username);
    Task<User> CreateAsync(User user);
}
