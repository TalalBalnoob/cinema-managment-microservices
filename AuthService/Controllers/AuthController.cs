using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using AuthService.data;
using AuthService.DTOs;
using AuthService.Models;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace AuthService.Controllers {
	[Route("/")]
	[ApiController]
	public class AuthController(IConfiguration config, AppDbContext db) : ControllerBase {
		[HttpGet("ping")]
		public IActionResult Ping() {
			return Ok("Auth Service is alive!");
		}

		[HttpPost("register")]
		public IActionResult SingUp(NewUserDto userDto) {
			User? isExist = db.Users.FirstOrDefault(u => userDto.Email == userDto.Email || u.Username == userDto.Username);
			if (isExist != null) return this.BadRequest("User with that email/username already exists");

			var hasher = new PasswordHasher<string>();

			var newUser = new User {
				Email = userDto.Email,
				Username = userDto.Username,
				Password = hasher.HashPassword(userDto.Username, userDto.Password),
				Id = 0,
				CreatedAt = DateTime.UtcNow,
			};

			db.Users.Add(newUser);
			db.SaveChanges();

			return Ok(newUser);
		}


		[HttpPost("login")]
		public IActionResult Login([FromBody] LoginDto login) {
			var user = db.Users.FirstOrDefault(u => u.Username == login.username);
			if (user == null) return NotFound();
			var hasher = new PasswordHasher<string>();

			var isValidPass = hasher.VerifyHashedPassword(user.Username, user.Password, login.password);
			if (isValidPass != PasswordVerificationResult.Success) return Unauthorized();

			var token = this.GenerateJwtToken(user);
			return Ok(token);

		}

		private string GenerateJwtToken(User user) {
			var claims = new[]
			{
			new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
			new Claim(ClaimTypes.Name, user.Username),
			new Claim(ClaimTypes.Email, user.Email),
		};

			var key = new SymmetricSecurityKey(
				Encoding.UTF8.GetBytes(config["Jwt:Key"]!)
			);
			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

			var token = new JwtSecurityToken(
				issuer: config["Jwt:Issuer"],
				audience: config["Jwt:Audience"],
				claims: claims,
				expires: DateTime.UtcNow.AddDays(100),
				signingCredentials: creds
			);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}
	}

}
