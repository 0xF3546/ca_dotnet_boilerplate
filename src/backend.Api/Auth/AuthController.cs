using backend.Core.Accounts;
using backend.Core.Auth;
using backend.DataAccess.Accounts;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace backend.Api.Auth
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController(SignInManager<AppUser> signInManager, RoleManager<AppRole> roleManager, UserManager<AppUser> userManager, IExternalAuthService externalAuthService) : ControllerBase
    {
        [HttpPost("login")]
        [ProducesResponseType<UserDto>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var user = await userManager.FindByEmailAsync(loginDto.Email);
            if (user == null)
            {
                return BadRequest("User not found.");
            }
            
            var loginResult = await signInManager.PasswordSignInAsync(user, loginDto.Password, true, false);
            if (!loginResult.Succeeded)
            {
                return BadRequest("Invalid login attempt.");
            }

            return Ok(user.GetDto());
        }

        [HttpPost("register")]
        [ProducesResponseType<UserDto>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            if (registerDto == null)
            {
                return BadRequest("Registration data is required.");
            }

            var user = new AppUser
            {
                UserName = registerDto.Email,
                Email = registerDto.Email
            };

            var result = await userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors.Select(e => e.Description));
            }

            await signInManager.SignInAsync(user, isPersistent: false);
            return Ok(user.GetDto());
        }

        [HttpGet("google-login")]
        public IActionResult GoogleLogin(string? returnUrl = null)
        {
            var redirectUrl = Url.Action(nameof(GoogleCallback), "Auth", new { returnUrl });
            var properties = signInManager.ConfigureExternalAuthenticationProperties(GoogleDefaults.AuthenticationScheme, redirectUrl);
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        [HttpGet("google-callback")]
        public async Task<IActionResult> GoogleCallback(string? returnUrl = null)
        {
            var info = await signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return BadRequest("Error loading external login information.");
            }

            var email = info.Principal.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrEmpty(email))
            {
                return BadRequest("Email not provided by Google.");
            }

            var externalAuthDto = new ExternalAuthDto
            {
                Provider = info.LoginProvider,
                ProviderKey = info.ProviderKey,
                Email = email,
                Name = info.Principal.FindFirstValue(ClaimTypes.Name)
            };

            var userDto = await externalAuthService.HandleExternalLoginAsync(externalAuthDto);
            if (userDto == null)
            {
                return BadRequest("Failed to process external login.");
            }

            if (!string.IsNullOrEmpty(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return Ok(userDto);
        }

        [Authorize]
        [HttpPost("logout")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return NoContent();
        }

        [Authorize]
        [HttpGet("me")]
        [ProducesResponseType<AuthUserDto>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetCurrentUser()
        {
            var user = await userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            var roleNames = await userManager.GetRolesAsync(user);

            var roles = await roleManager.Roles
                .Where(r => r.Name != null && roleNames.Contains(r.Name))
                .Select(r => r.GetDto())
                .ToListAsync();

            var dto = new AuthUserDto
            {
                User = user.GetDto(),
                Roles = roles
            };

            return Ok(dto);
        }
    }
}
