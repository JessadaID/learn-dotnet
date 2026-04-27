using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using learn_dotnet.models;
using learn_dotnet.data;
using learn_dotnet.services;

namespace learn_dotnet.controllers;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly ITokenService _tokenService;

    public AuthController(AppDbContext context, ITokenService tokenService)
    {
        _context = context;
        _tokenService = tokenService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<User>> Register(User user)
    {
        var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == user.Username);
        if (existingUser != null)
        {
            return BadRequest(new { message = "User already exists" });
        }
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return Ok(new { message = "Register success" });
    }

    [HttpPost("login")]
    public async Task<ActionResult<User>> Login(User user)
    {
        var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == user.Username && u.Password == user.Password);
        if (existingUser == null)
        {
            return Unauthorized();
        }
        return Ok(new { Token = _tokenService.GenerateToken(existingUser) });
    }
}