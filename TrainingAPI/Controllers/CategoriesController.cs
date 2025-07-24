using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrainingAPI.DTOs;
using TrainingAPI.DTOs.Mapping;
using TrainingAPI.Models;
using TrainingAPI.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;

namespace TrainingAPI.Controllers;

/// <summary>
/// Controller responsible for managing product categories.
/// Provides endpoints for creating, retrieving, updating, and deleting categories.
/// </summary>
[Produces("application/json")]
[Route("api/[Controller]")]
[ApiController]
public class CategoriesController : ControllerBase
{
    private readonly IUnitOfWork _uof;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="CategoriesController"/> class.
    /// </summary>
    /// <param name="uof">The injected Unit of Work.</param>
    /// <param name="mapper">The injected AutoMapper instance.</param>
    public CategoriesController(IUnitOfWork uof, IMapper mapper)
    {
        _uof = uof;
        _mapper = mapper;
    }

    /// <summary>
    /// Retrieves all categories.
    /// </summary>
    /// <returns>A list of category DTOs.</returns>
    /// <response code="200">Returns the list of categories.</response>
    /// <response code="404">If no categories are found.</response>
    [HttpGet("GetAll")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<IEnumerable<Category>>> GetAll()
    {
        var categories = await _uof.CategoriesRepository.GetAllAsync();
        if (categories is null)
            return NotFound("There are no categories registered in the database.");

        var categoriesDTO = _mapper.Map<IEnumerable<CategoryDTOResponse>>(categories);
        return Ok(categoriesDTO);
    }

    /// <summary>
    /// Retrieves a specific category by its ID.
    /// </summary>
    /// <param name="id">The category ID.</param>
    /// <returns>The corresponding category DTO.</returns>
    /// <response code="200">Returns the category.</response>
    /// <response code="404">If the category is not found.</response>
    [HttpGet("Get", Name = "GetCategory")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<Category>> Get(int id)
    {
        var category = await _uof.CategoriesRepository.GetAsync(c => c.CategoryId == id);

        if (category is null)
            return NotFound($"Category with ID = {id} was not found.");

        var categoryDTO = _mapper.Map<CategoryDTOResponse>(category);
        return Ok(categoryDTO);
    }

    /// <summary>
    /// Creates a new category.
    /// </summary>
    /// <param name="categoryDTO">The category data to create.</param>
    /// <remarks>
    /// This endpoint requires the user to be in the "ADMIN" role.
    /// Only users authorized by the "AdminOnly" policy can access this resource.
    /// </remarks>
    /// <returns>The created category.</returns>
    /// <response code="201">Returns the created category.</response>
    /// <response code="400">If the request data is invalid.</response>
    [Authorize(Policy = "AdminOnly")]
    [HttpPost("Post")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult> Post(CategoryDTO categoryDTO)
    {
        if (categoryDTO is null)
            return BadRequest("Invalid data.");

        if (!ModelState.IsValid || !TryValidateModel(categoryDTO))
            return BadRequest(ModelState);

        var categoryToCreate = _mapper.Map<Category>(categoryDTO);
        var newCategory = _uof.CategoriesRepository.Create(categoryToCreate);
        await _uof.Commit();

        var createdCategory = _mapper.Map<CategoryDTOResponse>(newCategory);

        return new CreatedAtRouteResult("GetCategory",
            new { id = newCategory.CategoryId }, createdCategory);
    }

    /// <summary>
    /// Updates an existing category.
    /// </summary>
    /// <param name="id">The ID of the category to update.</param>
    /// <param name="categoryDTO">The updated category data.</param>
    /// <remarks>
    /// This endpoint requires the user to be in the "ADMIN" role.
    /// Only users authorized by the "AdminOnly" policy can access this resource.
    /// </remarks>
    /// <returns>The updated category.</returns>
    /// <response code="200">Returns the updated category.</response>
    /// <response code="400">If the request data is invalid.</response>
    [Authorize(Policy = "AdminOnly")]
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult> Put(int id, PutCategoryDTORequest categoryDTO)
    {
        if (categoryDTO == null)
            return BadRequest("Invalid data.");

        if (id != categoryDTO.CategoryId)
            return BadRequest("Mismatched category ID.");

        if (!ModelState.IsValid || !TryValidateModel(categoryDTO))
            return BadRequest(ModelState);

        var categoryToUpdate = _mapper.Map<Category>(categoryDTO);
        _uof.CategoriesRepository.Update(categoryToUpdate);
        await _uof.Commit();

        var updatedCategory = _mapper.Map<CategoryDTOResponse>(categoryToUpdate);
        return Ok(updatedCategory);
    }

    /// <summary>
    /// Deletes a category by ID.
    /// </summary>
    /// <param name="id">The ID of the category to delete.</param>
    /// <remarks>
    /// This endpoint requires the user to be in the "ADMIN" role.
    /// Only users authorized by the "AdminOnly" policy can access this resource.
    /// </remarks>
    /// <returns>The deleted category.</returns>
    /// <response code="200">Returns the deleted category.</response>
    /// <response code="404">If the category is not found.</response>
    [HttpDelete("{id:int}")]
    [Authorize(Policy = "AdminOnly")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult> Delete(int id)
    {
        var category = await _uof.CategoriesRepository.GetAsync(c => c.CategoryId == id);

        if (category is null)
            return NotFound($"Category with ID = {id} was not found.");

        _uof.CategoriesRepository.Delete(category);
        await _uof.Commit();

        var deletedCategory = _mapper.Map<CategoryDTOResponse>(category);
        return Ok(deletedCategory);
    }
}
