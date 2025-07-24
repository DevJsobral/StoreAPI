using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrainingAPI.DTOs.OrderDTOs;

/// <summary>
/// Data Transfer Object for order item responses.
/// </summary>
public class OrderItemResponseDTO
{
    /// <summary>
    /// Name of the product.
    /// </summary>
    public string ProductName { get; set; }

    /// <summary>
    /// Price per unit of the product.
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// Quantity ordered.
    /// </summary>
    public int Quantity { get; set; }
}
