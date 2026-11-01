using url.Application.Dtos;
using url.Application.Interface;
using url.Domain.Entities;

namespace url.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository=userRepository;
    }
    public async Task<ViewUserDto> RegisterUserAsync(CreateUserDto dto)
    {
        var existingUser=await _userRepository.GetUserByEmail(dto.Email);

        if (existingUser != null)
        {
            throw new Exception("User with that email already exists");
        }

        var user=new User
        {
            Name=dto.Name,
            Email=dto.Email,
            Password=BCrypt.Net.BCrypt.HashPassword(dto.Password)
        };

        await _userRepository.AddUserAsync(user);
        await _userRepository.SaveChangesAsync();

        return new ViewUserDto
        {
            Id=user.Id,
            Name=user.Name,
            Email=user.Email
        };
    }

    public async Task<ViewUserDto> LoginUserAsync(LoginUserDto dto1)
    {
        var existingUser=await _userRepository.GetUserByEmail(dto1.Email);

        if (existingUser == null)
        {
            throw new Exception("Cannot find the user");
        }

        if (!BCrypt.Net.BCrypt.Verify(dto1.Password, existingUser.Password))
        {
           throw new Exception("Passwords did not match");
        }

        return new ViewUserDto
        {
            Id=existingUser.Id,
            Name=existingUser.Name,
            Email=existingUser.Email

        };
    }
}