using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TrainingAPI.Models;

/// <summary>
/// Represents an item within an order, including product, quantity, and unit price.
/// </summary>
public class OrderItem
{
    /// <summary>
    /// Unique identifier for the order item.
    /// </summary>
    public int OrderItemId { get; set; }

    /// <summary>
    /// Foreign key referencing the associated product.
    /// </summary>
    public int ProductId { get; set; }

    /// <summary>
    /// Navigation property for the associated product.
    /// </summary>
    public Product Product { get; set; }

    /// <summary>
    /// Quantity of the product ordered.
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// Price per unit of the product at the time of the order.
    /// </summary>
    [Column(TypeName = "decimal(10,2)")]
    public decimal UnitPrice { get; set; }

    /// <summary>
    /// Foreign key referencing the associated order.
    /// </summary>
    public int OrderId { get; set; }

    /// <summary>
    /// Navigation property for the associated order.
    /// </summary>
    public Order Order { get; set; }
}
