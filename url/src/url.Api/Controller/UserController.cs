using Microsoft.AspNetCore.Mvc;
using url.Application.Dtos;
using url.Application.Interface;

namespace url.Api.Controller;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService=userService;
    }

    [HttpPost("register/user")]
    public async Task<IActionResult> RegisterAsync(CreateUserDto dto)
    {
        var user=await _userService.RegisterUserAsync(dto);

        return Ok(user);
    }

    [HttpPost("login/user")]
    public async Task<IActionResult> LoginAsync(LoginUserDto dto)
    {
        var loggedIn=await _userService.LoginUserAsync(dto);

        return Ok(loggedIn);
    }
}