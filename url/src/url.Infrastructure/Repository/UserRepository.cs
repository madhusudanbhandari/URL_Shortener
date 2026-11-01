using Microsoft.EntityFrameworkCore;
using url.Application.Interface;
using url.Domain.Entities;
using url.Infrastructure.Persistence.Configurations;

namespace url.Infrastructure.Repository;

public class UserRepository : IUserRepository
{
    private readonly urlDbContext _context;

    public UserRepository(urlDbContext context)
    {
        _context=context;
    }
    public async Task<User?> GetUserById(int id)
    {
        return await _context.users.FirstOrDefaultAsync(u=>u.Id==id);
    }

    public async Task<User?> GetUserByEmail(string email)
    {
        return await _context.users.FirstOrDefaultAsync(u=>u.Email==email);

    }

    public async Task AddUserAsync(User user)
    {
         await _context.AddAsync(user);
    }

    public async Task SaveChangesAsync()
    {
         await _context.SaveChangesAsync();
    }
}