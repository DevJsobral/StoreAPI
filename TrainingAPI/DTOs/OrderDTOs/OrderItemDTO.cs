using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrainingAPI.DTOs.OrderDTOs
{
    /// <summary>
    /// Data Transfer Object representing an item in an order.
    /// </summary>
    public class OrderItemDTO
    {
        /// <summary>
        /// Gets or sets the ID of the product in the order item.
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// Gets or sets the quantity of the product in the order item.
        /// </summary>
        public int Quantity { get; set; }
    }
}
