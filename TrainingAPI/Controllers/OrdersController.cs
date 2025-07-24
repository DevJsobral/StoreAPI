using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrainingAPI.DTOs.OrderDTOs;
using TrainingAPI.Models;
using TrainingAPI.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;

namespace TrainingAPI.Controllers;

/// <summary>
/// Controller responsible for managing orders.
/// Provides endpoints for creating and retrieving orders.
/// </summary>
[Produces("application/json")]
[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IUnitOfWork _uof;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="OrdersController"/> class.
    /// </summary>
    /// <param name="uof">Injected Unit of Work for data operations.</param>
    /// <param name="mapper">Injected AutoMapper instance.</param>
    public OrdersController(IUnitOfWork uof, IMapper mapper)
    {
        _uof = uof;
        _mapper = mapper;
    }

    /// <summary>
    /// Retrieves all orders from the system with their items.
    /// </summary>
    ///  <remarks>
    /// This endpoint requires the user to be in the "ADMIN" role.
    /// Only users authorized by the "AdminOnly" policy can access this resource.
    /// </remarks>
    /// <returns>A list of order DTOs.</returns>
    /// <response code="200">Returns the list of orders.</response>
    /// <response code="404">If no orders are found.</response>
    [HttpGet("GetAllOrders")]
    [Authorize(Policy = "AdminOnly")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<IEnumerable<Order>>> GetAll()
    {
        var orders = await _uof.OrdersRepository.GetAllOrders();

        if (orders is null)
            return NotFound("There's no orders registered in our database");

        return Ok(_mapper.Map<IEnumerable<OrderResponseDTO>>(orders));
    }

    /// <summary>
    /// Creates a new order based on the provided data.
    /// </summary>
    /// <param name="orderDTO">The order request DTO containing items and quantities.</param>
    /// <returns>A newly created order with its details.</returns>
    /// <response code="201">Returns the created order.</response>
    /// <response code="400">If the request data is invalid.</response>
    /// <response code="404">If any of the provided products are not found.</response>
    [HttpPost("CreateOrder")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult> Post(OrderRequestDTO orderDTO)
    {
        if (orderDTO is null)
            return BadRequest("Invalid Data...");

        if (!ModelState.IsValid || !TryValidateModel(orderDTO))
            return BadRequest(ModelState);

        var orderItems = new List<OrderItem>();
        decimal total = 0;

        foreach (var itemDTO in orderDTO.Items)
        {
            var product = await _uof.ProductsRepository.GetAsync(p => p.ProductId == itemDTO.ProductId);
            if (product == null)
                return NotFound($"Product ID {itemDTO.ProductId} not found.");

            orderItems.Add(new OrderItem
            {
                ProductId = product.ProductId,
                Quantity = itemDTO.Quantity,
                UnitPrice = product.Price
            });

            total += product.Price * itemDTO.Quantity;
        }

        var newOrder = new Order
        {
            Items = orderItems,
            Total = total
        };

        _uof.OrdersRepository.Create(newOrder);
        await _uof.Commit();

        return new CreatedAtRouteResult("GetOrder",
            new { id = newOrder.OrderId }, _mapper.Map<OrderResponseDTO>(newOrder));
    }

    /// <summary>
    /// Retrieves a specific order by its ID.
    /// </summary>
    /// <param name="id">The ID of the order.</param>
    /// <remarks>
    /// This endpoint requires the user to be in the "ADMIN" role.
    /// Only users authorized by the "AdminOnly" policy can access this resource.
    /// </remarks>
    /// <returns>The order DTO if found.</returns>
    /// <response code="200">Returns the requested order.</response>
    /// <response code="404">If the order is not found.</response>
    [HttpGet("{id}", Name = "GetOrder")]
    [Authorize(Policy = "AdminOnly")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<OrderResponseDTO>> GetById(int id)
    {
        var order = await _uof.OrdersRepository.GetAsync(o => o.OrderId == id);
        if (order == null)
            return NotFound("Order not found.");

        return Ok(_mapper.Map<OrderResponseDTO>(order));
    }

    /// <summary>
    /// Deletes an order by its ID.
    /// </summary>
    /// <param name="id">The ID of the order to delete.</param>
    /// <returns>No content if deleted successfully, or NotFound if the order does not exist.</returns>
    [HttpDelete("{id}")]
    [Authorize(Policy = "AdminOnly")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> DeleteOrder(int id)
    {
        var order = await _uof.OrdersRepository.GetAsync(o => o.OrderId == id);
        if (order == null)
        {
            return NotFound(new { message = $"Order with ID {id} not found." });
        }

        _uof.OrdersRepository.Delete(order);
        await _uof.Commit();

        return NoContent();
    }

}
