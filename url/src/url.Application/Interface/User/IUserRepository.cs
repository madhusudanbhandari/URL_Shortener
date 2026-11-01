using url.Domain.Entities;

namespace url.Application.Interface;

public interface IUserRepository
{
    Task AddUserAsync(User user);
    Task<User?> GetUserById(int id);
    Task<User?> GetUserByEmail(string email);
    public Task SaveChangesAsync();
}