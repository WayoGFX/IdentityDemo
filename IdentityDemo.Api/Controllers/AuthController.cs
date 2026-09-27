using IdentityDemo.Api.Data;
using IdentityDemo.Api.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityDemo.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager;

    public record RegisterRequest(string Email, string Password);

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var user = new IdentityUser
        {
            UserName = request.Email,
            Email = request.Email,
            
        };
        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        return Ok("User created correctly");
    }

    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly TokenService _tokenService;

    public AuthController(
        UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager,
        TokenService tokenService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
    }

    // request
    public record LoginRequest(string Email, string Password);

    // login
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {   
        //find user with email
        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user is null)
        {
            return Unauthorized("Invalid Credential");
        }

        // check password
        var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, lockoutOnFailure: true);

        if (!result.Succeeded)
        {
            // if is not correct
            if (!result.IsLockedOut)
            {
                // if there many tries 
                return Unauthorized("Your count have been blocked temporally for many falling tries");
            }
            return Unauthorized("Invalid Credentials");
        }
        // get list roles
        var roles = await _userManager.GetRolesAsync(user);
        var token = _tokenService.GenerateToken(user,roles); // made token with roles and user data

        return Ok(new{token});
    }
}