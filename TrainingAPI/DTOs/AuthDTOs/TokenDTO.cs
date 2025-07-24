using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TrainingAPI.DTOs.AuthDTOs
{
    /// <summary>
    /// Data Transfer Object for access and refresh tokens used in authentication.
    /// </summary>
    public class TokenDTO
    {
        /// <summary>
        /// Gets or sets the JWT access token.
        /// </summary>
        public string? AccessToken { get; set; }
        
        /// <summary>
        /// Gets or sets the refresh token used to obtain a new access token.
        /// </summary>
        public string? RefreshToken { get; set; }
    }
}
