using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TrainingAPI.Models;

/// <summary>
/// Represents a product.
/// </summary>
public class Product
{
    public Product()
    {
        RegisterDate = DateTime.Now;
    }
    /// <summary>
    /// Unique identifier for the product.
    /// </summary>
    public int ProductId { get; set; }

    /// <summary>
    /// Name of the product.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Product description.
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Price of the product.
    /// </summary>
    [Column(TypeName = "decimal(10,2)")]
    public decimal Price { get; set; }

    /// <summary>
    /// URL of the product image.
    /// </summary>
    public string ImageURL { get; set; }

    /// <summary>
    /// Available stock quantity.
    /// </summary>
    public int Stock { get; set; }

    /// <summary>
    /// Date the product was registered.
    /// </summary>
    public DateTime RegisterDate { get; set; }

    /// <summary>
    /// Foreign key to category.
    /// </summary>
    public int CategoryId { get; set; }

    /// <summary>
    /// Category to which this product belongs.
    /// </summary>
    public Category Category { get; set; }
}


