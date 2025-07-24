using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrainingAPI.DTOs.OrderDTOs;

/// <summary>
/// Data Transfer Object for order responses.
/// </summary>
public class OrderResponseDTO
{
    /// <summary>
    /// Order identifier.
    /// </summary>
    public int OrderId { get; set; }

    /// <summary>
    /// Date/time the order was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// List of items included in the order.
    /// </summary>
    public List<OrderItemResponseDTO> Items { get; set; }

    /// <summary>
    /// Total price of the order.
    /// </summary>
    public decimal Total { get; set; }
}
