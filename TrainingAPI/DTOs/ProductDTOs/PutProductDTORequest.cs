using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrainingAPI.DTOs
{
    /// <summary>
    /// DTO for updating an existing product.
    /// Contains all properties required for a full update via PUT.
    /// </summary>
    public class PutProductDTORequest
    {
        /// <summary>
        /// Gets or sets the product identifier.
        /// </summary>
        [Required]
        public int ProductId { get; set; }

        /// <summary>
        /// Gets or sets the product name.
        /// Cannot be null and must have a maximum length of 80 characters.
        /// </summary>
        [Required(ErrorMessage = "Name can't be null")]
        [StringLength(80, ErrorMessage = "Name can't have more than 80 characters")]
        public string? Name { get; set; }

        /// <summary>
        /// Gets or sets the product description.
        /// Cannot be null and has a maximum length of 300 characters.
        /// </summary>
        [Required(ErrorMessage = "Description can't be null")]
        [StringLength(300)]
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets the product price.
        /// Must be between 1 and 10,000.
        /// </summary>
        [Required(ErrorMessage = "Price can't be null")]
        [Range(1, 10000, ErrorMessage = "Price must be between 1 and 10,000.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Stock can't be null")]
        [Range(1, 50000, ErrorMessage = "Stock must be beetween 1 and 50.000.")]
        public int Stock { get; set; }

        /// <summary>
        /// Gets or sets the image URL for the product.
        /// Cannot be null.
        /// </summary>
        [Required(ErrorMessage = "ImageURL can't be null")]
        public string? ImageURL { get; set; }

        /// <summary>
        /// Gets or sets the category identifier to which the product belongs.
        /// Must be a valid positive integer.
        /// </summary>
        [Required(ErrorMessage = "CategoryId is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "You must provide a valid categoryId (greater than 0)")]
        public int CategoryId { get; set; }
    }
}
