using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TrainingAPI.DTOs.Mapping
{
    /// <summary>
    /// Data Transfer Object for category creation and update.
    /// </summary>
    public class CategoryDTO
    {
        /// <summary>
        /// Gets or sets the category name.
        /// Required and maximum length of 80 characters.
        /// </summary>
        [Required(ErrorMessage = "Name can't be null")]
        [StringLength(80, ErrorMessage = "Name can't have more than 80 characters")]
        public string? Name { get; set; }

        /// <summary>
        /// Gets or sets the URL of the category image.
        /// This field is required.
        /// </summary>
        [Required(ErrorMessage = "ImageURL can't be null")]
        public string? ImageURL { get; set; }
    }
}
