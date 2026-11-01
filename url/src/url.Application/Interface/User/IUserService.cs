using System.ComponentModel.Design;
using url.Application.Dtos;

namespace url.Application.Interface;


public interface IUserService
{
    public Task<ViewUserDto> RegisterUserAsync(CreateUserDto registerUserDto);
    public Task<ViewUserDto> LoginUserAsync(LoginUserDto loginUserDto);
}