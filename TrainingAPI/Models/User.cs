using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace TrainingAPI.Models;

/// <summary>
/// Represents an application user with authentication details and refresh token information.
/// Inherits from ASP.NET Core IdentityUser.
/// </summary>
public class User : IdentityUser
{
    /// <summary>
    /// The refresh token assigned to the user for renewing JWT access tokens.
    /// </summary>
    public string? RefreshToken { get; set; }

    /// <summary>
    /// The expiration time of the current refresh token.
    /// </summary>
    public DateTime RefreshTokenExpiryTime { get; set; }
}
