using AuthenticationLesson.Authentication;
using AuthenticationLesson.Authentication.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationLesson.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthenticationController : ControllerBase
	{
		private readonly ILogger _logger;
		private readonly IConfiguration _configuration;
		private readonly UserManager<ApplicationUser> _userManager;

		public AuthenticationController(ILoggerFactory loggerFactory, IConfiguration configuration, UserManager<ApplicationUser> userManager)
		{
			_logger = loggerFactory.CreateLogger<AuthenticationController>();
			_configuration = configuration;
			_userManager = userManager;
		}

		[Route("login")]
		[HttpPost]
		[SwaggerRequestExample(typeof(LoginModel), typeof(LoginModelExample))]
		public async Task<IActionResult> Login([FromBody] LoginModel model)
		{
			var user = await _userManager.FindByNameAsync(model.Username);
			if((user != null) && await _userManager.CheckPasswordAsync(user, model.Password))
			{
				var authClaims = new List<Claim>
				{
					new Claim(ClaimTypes.Name, model.Username),
					new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
				};
				foreach (var userRole in await _userManager.GetRolesAsync(user))
				{
					authClaims.Add(new Claim(ClaimTypes.Role, userRole));
				}
				var token = new JwtSecurityToken
				(
					_configuration["JWT:ValidIssuer"],
					_configuration["JWT:ValidAudience"],
					authClaims,
					DateTime.Now,
					DateTime.Now.AddMinutes(20),
					new SigningCredentials
					(
						new SymmetricSecurityKey
						(
							Encoding.UTF8.GetBytes(_configuration["JWT:Secret"])
						),
						SecurityAlgorithms.HmacSha256
					)
				);
				return Ok
				(
					new
					{
						token = new JwtSecurityTokenHandler().WriteToken(token),
						expiration = token.ValidTo
					}
				);
			}
			return Unauthorized();
		}

		[Route("register")]
		[HttpPost]
		[SwaggerRequestExample(typeof(RegisterModel), typeof(RegisterModelExample))]
		public async Task<IActionResult> Register([FromBody] RegisterModel model)
		{
			if(await _userManager.FindByNameAsync(model.Username) != null)
			{
				return StatusCode
				(
					StatusCodes.Status409Conflict,
					new AuthenticationResponse
					{
						Status = AuthenticationStatus.UserExists,
						Message = "Username already exists"
					}
				);
			}
			ApplicationUser user = new ApplicationUser
			{
				Email = model.Email,
				SecurityStamp = Guid.NewGuid().ToString(),
				UserName = model.Username
			};
			if(!(await _userManager.CreateAsync(user, model.Password)).Succeeded)
			{
				return StatusCode
				(
					StatusCodes.Status500InternalServerError,
					"Failed to create user"
				);
			}
			return Ok
			(
				new AuthenticationResponse
				{
					Status = AuthenticationStatus.UserCreated,
					Message = "User created"
				}
			);
		}
	}
}