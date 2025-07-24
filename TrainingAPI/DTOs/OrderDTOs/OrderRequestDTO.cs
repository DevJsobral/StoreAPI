using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrainingAPI.DTOs.OrderDTOs
{
    /// <summary>
    /// Data Transfer Object for creating a new order.
    /// Contains a list of items included in the order.
    /// </summary>
    public class OrderRequestDTO
    {
        /// <summary>
        /// Gets or sets the list of order items.
        /// </summary>
        public List<OrderItemDTO> Items { get; set; } = new List<OrderItemDTO>();
    }
}
