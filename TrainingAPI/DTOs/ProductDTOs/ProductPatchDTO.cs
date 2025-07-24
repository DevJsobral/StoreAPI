using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TrainingAPI.DTOs
{
    /// <summary>
    /// DTO for partial update of a product.
    /// Used to update the product's price and stock via PATCH requests.
    /// </summary>
    public class ProductPatchDTO
    {
        /// <summary>
        /// Gets or sets the price of the product.
        /// Must be between 1 and 10,000.
        /// </summary>
        [Range(1, 10000, ErrorMessage = "Price must be between 1 and 10,000.")]
        public decimal Price { get; set; }

        /// <summary>
        /// Gets or sets the stock quantity of the product.
        /// Must be between 1 and 10,000.
        /// </summary>
        [Range(1, 10000, ErrorMessage = "Stock must be between 1 and 10,000.")]
        public float Stock { get; set; }
    }
}
