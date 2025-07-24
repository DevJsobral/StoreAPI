using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System.Collections.ObjectModel;

namespace TrainingAPI.Models;

/// <summary>
/// Represents a product category.
/// </summary>
public class Category
{
    public Category()
    {
        Products = new Collection<Product>();
    }

    /// <summary>
    /// Unique identifier for the category.
    /// </summary>
    public int CategoryId { get; set; }

    /// <summary>
    /// Name of the category.
    /// </summary>
    public string? Name { get; set; }

    public string? ImageURL { get; set; }

    [JsonIgnore]
    public ICollection<Product>? Products { get; set; }
}
