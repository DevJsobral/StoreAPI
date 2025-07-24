using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TrainingAPI.DTOs
{
    /// <summary>
    /// Data Transfer Object for updating a category.
    /// </summary>
    public class PutCategoryDTORequest
    {
        /// <summary>
        /// Gets or sets the unique identifier of the category.
        /// </summary>
        [Required]
        public int CategoryId { get; set; }

        /// <summary>
        /// Gets or sets the name of the category.
        /// </summary>
        [Required(ErrorMessage = "Name can't be null")]
        [StringLength(80, ErrorMessage = "Name can't have more than 80 characters")]
        public string? Name { get; set; }

        /// <summary>
        /// Gets or sets the URL of the category image.
        /// </summary>
        [Required(ErrorMessage = "ImageURL can't be null")]
        public string? ImageURL { get; set; }
    }
}
