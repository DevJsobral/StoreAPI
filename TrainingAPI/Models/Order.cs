using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using TrainingAPI.DTOs;

namespace TrainingAPI.Models;

/// <summary>
/// Represents an order in the system.
/// </summary>
public class Order
{
    public Order()
    {
        CreatedAt = DateTime.Now;
    }
    /// <summary>
    /// Unique identifier for the order.
    /// </summary>
    public int OrderId { get; set; }

    /// <summary>
    /// Date/time when the order was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Total price of the order.
    /// </summary>
    [Column(TypeName = "decimal(10,2)")]
    public decimal Total { get; set; }

    /// <summary>
    /// List of items belonging to this order.
    /// </summary>
    public List<OrderItem> Items { get; set; }
}
