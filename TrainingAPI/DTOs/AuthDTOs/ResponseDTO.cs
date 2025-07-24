using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrainingAPI.DTOs.AuthDTOs
{
    /// <summary>
    /// Represents a standard response object with status and message.
    /// Used to return status information from API endpoints.
    /// </summary>
    public class ResponseDTO
    {
        /// <summary>
        /// Gets or sets the status of the response (e.g., "Success", "Error").
        /// </summary>
        public string? Status { get; set; }

        /// <summary>
        /// Gets or sets the message that provides additional details about the response.
        /// </summary>
        public string? Message { get; set; }
    }
}
