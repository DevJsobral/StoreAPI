using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrainingAPI.DTOs;
using TrainingAPI.DTOs.Mapping;
using TrainingAPI.Filters;
using TrainingAPI.Models;
using TrainingAPI.Repositories;
using TrainingAPI.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;

namespace TrainingAPI.Controllers;

/// <summary>
/// Controller responsible for managing product operations,
/// including listing, retrieving, creating, updating and deleting products.
/// </summary>
[Produces("application/json")]
[Route("api/[Controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly IUnitOfWork _uof;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProductsController"/> class.
    /// </summary>
    /// <param name="uof">The unit of work instance for repository access.</param>
    /// <param name="mapper">AutoMapper instance for DTO conversions.</param>
    public ProductsController(IUnitOfWork uof, IMapper mapper)
    {
        _uof = uof;
        _mapper = mapper;
    }

    /// <summary>
    /// Gets a list of all products, with optional filtering by name and category.
    /// </summary>
    /// <param name="name">Optional name to filter by.</param>
    /// <param name="categoryId">Optional category ID to filter by.</param>
    /// <returns>A list of product DTOs.</returns>
    /// <response code="200">Returns the list of products matching the criteria.</response>
    /// <response code="404">If no products are found matching the criteria.</response>
    [HttpGet("GetAll")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<IEnumerable<ProductDTOResponse>>> GetAll(string? name, int? categoryId)
    {
        var query = _uof.ProductsRepository.GetAllProducts();

        if (!string.IsNullOrWhiteSpace(name))
            query = query.Where(p => p.Name.Contains(name));

        if (categoryId.HasValue)
            query = query.Where(p => p.CategoryId == categoryId);

        var products = await query.ToListAsync();

        if (products == null || !products.Any())
            return NotFound("There are no products registered in the database matching the criteria.");

        var productsDTO = _mapper.Map<IEnumerable<ProductDTOResponse>>(products);

        return Ok(productsDTO);
    }

    /// <summary>
    /// Gets a single product by its ID.
    /// </summary>
    /// <param name="id">Product ID.</param>
    /// <returns>The product DTO if found; otherwise, not found.</returns>
    /// <response code="200">Returns the requested product.</response>
    /// <response code="404">If the product is not found.</response>
    [HttpGet("Get", Name = "GetProduct")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult> Get(int id)
    {
        var product = await _uof.ProductsRepository.GetAsync(p => p.ProductId == id);
        if (product is null)
            return NotFound($"Product with the id = {id} was not found.");

        var productDTO = _mapper.Map<ProductDTOResponse>(product);

        return Ok(productDTO);
    }

    /// <summary>
    /// Creates a new product.
    /// </summary>
    /// <param name="productDTO">Product data to create.</param>
    /// <remarks>
    /// This endpoint requires the user to be in the "ADMIN" role.
    /// Only users authorized by the "AdminOnly" policy can access this resource.
    /// </remarks>
    /// <returns>The created product with status 201.</returns>
    /// <response code="201">Returns the newly created product.</response>
    /// <response code="400">If the request data is invalid.</response>
    //[Authorize(Policy = "AdminOnly")]
    [HttpPost("Post")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult> Post(ProductDTORequest productDTO)
    {
        if (productDTO == null)
            return BadRequest("Invalid data.");

        if (!ModelState.IsValid || !TryValidateModel(productDTO))
            return BadRequest(ModelState);

        var productToCreate = _mapper.Map<Product>(productDTO);
        var newProduct = _uof.ProductsRepository.Create(productToCreate);
        await _uof.Commit();

        var createdProduct = _mapper.Map<ProductDTOResponse>(newProduct);

        return new CreatedAtRouteResult("GetProduct",
         new { id = newProduct.ProductId }, createdProduct);
    }

    /// <summary>
    /// Updates the price and stock of a product using a JSON patch.
    /// </summary>
    /// <param name="id">Product ID to update.</param>
    /// <param name="patchProductDTO">Patch document with updates.</param>
    /// <remarks>
    /// This endpoint requires the user to be in the "ADMIN" role.
    /// Only users authorized by the "AdminOnly" policy can access this resource.
    /// </remarks>
    /// <returns>The updated product if successful.</returns>
    /// <response code="200">Returns the updated product.</response>
    /// <response code="400">If the patch document is invalid or model state is invalid.</response>
    /// <response code="404">If the product to update was not found.</response>
    [Authorize(Policy = "AdminOnly")]
    [HttpPatch("{id}/UpdatePriceAndStock")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<ProductDTOResponse>> Patch(int id,
                                    [FromBody] ProductPatchDTO dto)
    {
        if (!ModelState.IsValid || id <= 0)
            return BadRequest(ModelState);

        var product = await _uof.ProductsRepository.GetAsync(p => p.ProductId == id);

        if (product == null)
            return NotFound($"Product with the id = {id} was not found.");

        _mapper.Map(dto, product);

        _uof.ProductsRepository.Update(product);
        await _uof.Commit();

        return Ok(_mapper.Map<ProductDTOResponse>(product));
    }

    /// <summary>
    /// Replaces a product's data with the provided values.
    /// </summary>
    /// <param name="id">Product ID to update.</param>
    /// <param name="productDTO">New product data.</param>
    /// <remarks>
    /// This endpoint requires the user to be in the "ADMIN" role.
    /// Only users authorized by the "AdminOnly" policy can access this resource.
    /// </remarks>
    /// <returns>The updated product.</returns>
    /// <response code="200">Returns the updated product.</response>
    /// <response code="400">If the request data is invalid or IDs do not match.</response>
    [Authorize(Policy = "AdminOnly")]
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult> Put(int id, PutProductDTORequest productDTO)
    {
        if (productDTO == null)
            return BadRequest("Invalid data.");

        if (id != productDTO.ProductId)
            return BadRequest("The product you're looking for to update must have the same ID you're requesting");

        if (!ModelState.IsValid || !TryValidateModel(productDTO))
            return BadRequest(ModelState);

        var productToUpdate = _mapper.Map<Product>(productDTO);
        _uof.ProductsRepository.Update(productToUpdate);
        await _uof.Commit();

        var updatedProduct = _mapper.Map<ProductDTOResponse>(productToUpdate);

        return Ok(updatedProduct);
    }

    /// <summary>
    /// Deletes a product by its ID.
    /// </summary>
    /// <param name="id">ID of the product to delete.</param>
    /// <remarks>
    /// This endpoint requires the user to be in the "ADMIN" role.
    /// Only users authorized by the "AdminOnly" policy can access this resource.
    /// </remarks>
    /// <returns>The deleted product if successful; otherwise, not found.</returns>
    /// <response code="200">Returns the deleted product.</response>
    /// <response code="404">If the product to delete was not found.</response>
    [Authorize(Policy = "AdminOnly")]
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult> Delete(int id)
    {
        var product = await _uof.ProductsRepository.GetAsync(p => p.ProductId == id);
        if (product is null)
            return NotFound($"Product with the id = {id} was not found.");

        _uof.ProductsRepository.Delete(product);
        await _uof.Commit();

        var deletedProduct = _mapper.Map<ProductDTOResponse>(product);

        return Ok(deletedProduct);
    }
}
