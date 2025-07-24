using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TrainingAPI.DTOs.AuthDTOs
{
    /// <summary>
    /// Data Transfer Object for user login.
    /// Contains the username and password required for authentication.
    /// </summary>
    public class LoginDTO
    {
        /// <summary>
        /// Gets or sets the username of the user attempting to login.
        /// </summary>
        [Required(ErrorMessage = "User name is Required!")]
        public string? Username { get; set; }

        /// <summary>
        /// Gets or sets the password of the user attempting to login.
        /// </summary>
        [Required(ErrorMessage = "Password is Required!")]
        public string? Password { get; set; }
    }
}
