﻿using ITBGameJam2025Api.DTOs;
using ITBGameJam2025Api.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Migrations.Operations.Builders;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ITBGameJam2025Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;

        public AuthController(UserManager<User> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO userDTO)
        {
            var user = new User { UserName = userDTO.Email, Email = userDTO.Email, Name = userDTO.Name, Surname = userDTO.Surname };
            var result = await _userManager.CreateAsync(user, userDTO.Password);
            if (result.Succeeded)
            {
                return Ok("User registered");
            }
            return BadRequest(result.Errors);
        }
        [HttpPost("admin/register")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterDTO userDTO)
        {
            var user = new User { UserName = userDTO.Email, Email = userDTO.Email, Name = userDTO.Name, Surname = userDTO.Surname };
            var result = await _userManager.CreateAsync(user, userDTO.Password);
            var roleResult = new IdentityResult();
            if (result.Succeeded)
            {
                roleResult = await _userManager.AddToRoleAsync(user, "Admin");
            }
            if(result.Succeeded && roleResult.Succeeded)
            {
                return Ok("Admin registered");
            }
            return BadRequest(result.Errors);
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO userDTO)
        {
            var user = await _userManager.FindByEmailAsync(userDTO.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, userDTO.Password))
            {
                return Unauthorized("Invalid email or password");
            }
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };
            var roles = await _userManager.GetRolesAsync(user);
            if(roles != null && roles.Count > 0)
            {
                foreach(var rol in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, rol));
                }
            }
            var token = CreateToken(claims.ToArray());
            return Ok(token);
        }
        private string CreateToken(Claim[] claims)
        {
            var jwtConfig = _configuration.GetSection("JwtSettings");
            var secretKey = jwtConfig["Key"];
            var issuer = jwtConfig["Issuer"];
            var audience = jwtConfig["Audience"];
            var expirationMinutes = int.Parse(jwtConfig["ExpirationMinutes"]);
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(expirationMinutes),
                signingCredentials: creds
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
