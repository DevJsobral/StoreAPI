using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using TrainingAPI.DTOs.AuthDTOs;
using TrainingAPI.Models;
using TrainingAPI.Services;
using Microsoft.AspNetCore.Http;

namespace TrainingAPI.Controllers;

/// <summary>
/// Controller responsible for authentication operations, including login,
/// refresh token, and token revocation.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly ITokenService _tokenService;
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IConfiguration _configuration;

    /// <summary>
    /// Initializes a new instance of the <see cref="AuthController"/> class.
    /// </summary>
    public AuthController(ITokenService tokenService,
                          UserManager<User> userManager,
                          RoleManager<IdentityRole> roleManager,
                          IConfiguration configuration)
    {
        _tokenService = tokenService;
        _userManager = userManager;
        _roleManager = roleManager;
        _configuration = configuration;
    }

    /*
    [HttpPost]
    [Route("CreateRole")]
    public async Task<IActionResult> CreateRole(string roleName)
    {
         var roleExist = await _roleManager.RoleExistsAsync(roleName);

        if (!roleExist)
        {
            var roleResult = await _roleManager.CreateAsync(new IdentityRole(roleName));

            if (roleResult.Succeeded)
            {
                return StatusCode(StatusCodes.Status200OK,
                        new ResponseDTO { Status = "Success", Message = 
                        $"Role {roleName} added successfully" });
            }
            else
            {
                return StatusCode(StatusCodes.Status400BadRequest,
                   new ResponseDTO { Status = "Error", Message = 
                       $"Issue adding the new {roleName} role" });
            }
        }
        return StatusCode(StatusCodes.Status400BadRequest,
          new ResponseDTO { Status = "Error", Message = "Role already exists." });
    }

    [HttpPost]
    [Route("AddUserToRole")]
    public async Task<IActionResult> AddUserToRole(string email, string roleName)
    {
       var user = await _userManager.FindByEmailAsync(email);

        if (user != null)
        {
            var result = await _userManager.AddToRoleAsync(user, roleName);
            if(result.Succeeded)
            {
                return StatusCode(StatusCodes.Status200OK,
                       new ResponseDTO { Status = "Success", Message = 
                       $"User {user.Email} added to the {roleName} role" });
            }
            else
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseDTO
                {
                    Status = "Error",
                    Message =$"Error: Unable to add user {user.Email} to the {roleName} role"
                });
            }
        }
        return BadRequest(new { error = "Unable to find user" });
    }
    }
    */

    /// <summary>
    /// Authenticates a user and returns an access token and refresh token.
    /// </summary>
    /// <param name="model">Login credentials.</param>
    /// <returns>JWT access token and refresh token.</returns>
    [HttpPost]
    [Route("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> Login([FromBody] LoginDTO model)
    {
        var user = await _userManager.FindByNameAsync(model.Username!);

        if (user is not null && await _userManager.CheckPasswordAsync(user, model.Password!))
        {
            var userRoles = await _userManager.GetRolesAsync(user);

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName!),
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var token = _tokenService.GenerateAccessToken(authClaims, _configuration);
            var refreshToken = _tokenService.GenerateRefreshToken();

            _ = int.TryParse(_configuration["JWT:RefreshTokenValidityInMinutes"], out int refreshTokenValidityInMinutes);
            user.RefreshTokenExpiryTime = DateTime.Now.AddMinutes(refreshTokenValidityInMinutes);
            user.RefreshToken = refreshToken;

            await _userManager.UpdateAsync(user);

            return Ok(new
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                RefreshToken = refreshToken,
                Expiration = token.ValidTo
            });
        }

        return Unauthorized();
    }

    /*
    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDTO model)
    {
          var userExists = await _userManager.FindByNameAsync(model.Username!);

        if (userExists != null)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                   new ResponseDTO { Status = "Error", Message = "User already exists!" });
        }

        User user = new()
        {
            Email = model.Email,
            SecurityStamp = Guid.NewGuid().ToString(),
            UserName = model.Username
        };

        var result = await _userManager.CreateAsync(user, model.Password!);

        if (!result.Succeeded)
        {   
           var errors = string.Join("; ", result.Errors.Select(e => e.Description));
            return StatusCode(StatusCodes.Status500InternalServerError,
                new ResponseDTO { Status = "Error", Message = $"User creation failed: {errors}" });
        }

        return Ok(new ResponseDTO { Status = "Success", Message = "User created successfully!" });

    }
    }
    */

    /// <summary>
    /// Generates a new access token and refresh token using valid expired access token and current refresh token.
    /// </summary>
    /// <param name="tokenModel">The token model containing expired access token and refresh token.</param>
    /// <returns>New access and refresh tokens if valid; otherwise, bad request.</returns>
    [HttpPost]
    [Route("refresh-token")]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> RefreshToken(TokenDTO tokenModel)
    {
        if (tokenModel is null)
        {
            return BadRequest("Invalid client request");
        }

        string? accessToken = tokenModel.AccessToken ?? throw new ArgumentNullException(nameof(tokenModel));
        string? refreshToken = tokenModel.RefreshToken ?? throw new ArgumentException(nameof(tokenModel));

        var principal = _tokenService.GetPrincipalFromExpiredToken(accessToken!, _configuration);

        if (principal == null)
        {
            return BadRequest("Invalid access token/refresh token");
        }

        string username = principal.Identity?.Name!;
        var user = await _userManager.FindByNameAsync(username);

        if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
        {
            return BadRequest("Invalid access token/refresh token");
        }

        var newAccessToken = _tokenService.GenerateAccessToken(principal.Claims.ToList(), _configuration);
        var newRefreshToken = _tokenService.GenerateRefreshToken();

        user.RefreshToken = newRefreshToken;
        await _userManager.UpdateAsync(user);

        return new ObjectResult(new
        {
            accessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
            refreshToken = newRefreshToken
        });
    }

    /// <summary>
    /// Revokes the refresh token of a specific user, effectively logging them out.
    /// </summary>
    /// <param name="username">The username whose refresh token should be revoked.</param>
    /// <returns>No content if successful; otherwise, bad request.</returns>
    [Authorize]
    [HttpPost]
    [Route("revoke/{username}")]
    [ProducesDefaultResponseType]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Revoke(string username)
    {
        var user = await _userManager.FindByNameAsync(username);

        if (user == null)
            return BadRequest("Invalid user name");

        user.RefreshToken = null;
        await _userManager.UpdateAsync(user);

        return NoContent();
    }
}
