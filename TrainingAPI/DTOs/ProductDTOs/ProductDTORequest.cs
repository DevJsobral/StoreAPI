using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TrainingAPI.DTOs
{
    /// <summary>
    /// DTO used for creating or updating a product.
    /// Contains necessary product details provided by the client.
    /// </summary>
    public class ProductDTORequest
    {
        /// <summary>
        /// Gets or sets the product name.
        /// Required, max length 80 characters.
        /// </summary>
        [Required(ErrorMessage = "Name can't be null")]
        [StringLength(80, ErrorMessage = "Name can't have more than 80 characters")]
        public string? Name { get; set; }

        /// <summary>
        /// Gets or sets the product description.
        /// Required, max length 300 characters.
        /// </summary>
        [Required(ErrorMessage = "Description can't be null")]
        [StringLength(300)]
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets the product price.
        /// Required, must be between 1 and 10,000.
        /// </summary>
        [Required(ErrorMessage = "Price can't be null")]
        [Range(1, 10000, ErrorMessage = "Price must be beetween 1 and 10.000.")]
        public decimal Price { get; set; }

        /// <summary>
        /// Gets or sets the URL of the product image.
        /// Required.
        /// </summary>
        [Required(ErrorMessage = "ImageURL can't be null")]
        public string? ImageURL { get; set; }

        /// <summary>
        /// Gets or sets the category ID associated with the product.
        /// Required, must be greater than 0.
        /// </summary>
        [Required(ErrorMessage = "CategoryId is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "You must provide a valid categoryId (greater than 0)")]
        public int CategoryId { get; set; }
    }
}
