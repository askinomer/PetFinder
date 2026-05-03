using Microsoft.EntityFrameworkCore;
using PetFinder.DataAccessLayer;
using PetFinder.Models;

namespace PetFinder.Services;

public class UserService : IUserService
{
    private readonly AppDbContext _db;

    public UserService(AppDbContext db) => _db = db;

    public async Task<User?> ValidateAsync(string username, string password)
    {
        // For a final term project: simple lookup. In production: hash + compare.
        return await _db.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Username == username && u.Password == password);
    }

    public async Task<User?> GetByIdAsync(int id) => await _db.Users.FindAsync(id);

    public async Task<User?> GetByUsernameAsync(string username) =>
        await _db.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Username == username);

    public async Task<User> CreateAsync(User user)
    {
        _db.Users.Add(user);
        await _db.SaveChangesAsync();
        return user;
    }
}
