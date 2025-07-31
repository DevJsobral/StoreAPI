using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using TrainingAPI.Controllers;
using TrainingAPI.DTOs;

namespace TrainingAPIXunitTests.UnitTests;

public class PostProductsUnitTests : IClassFixture<ProductsUnitTestController>
{
    private readonly ProductsController _controller;

    public PostProductsUnitTests(ProductsUnitTestController controller)
    {
        _controller = new ProductsController(controller.repository, controller.mapper);
    }

    [Fact]
    public async Task PostProductsUnit_Return_CreatedStatusCode()
    {
        //Arrange
        var newProductDTO = new ProductDTORequest
        {
            Name = "Test product",
            Description = "Testing a product name",
            Price = 20,
            Stock = 100,
            ImageURL = "FakeUrl.svg",
            CategoryId = 1
        };
        //Act
        var data = await _controller.Post(newProductDTO);

        //Assert
        var createdResult = data.Result.Should().BeOfType<CreatedAtRouteResult>();
        createdResult.Subject.StatusCode.Should().Be(201);
    }

    [Fact]
    public async Task PostProductsUnit_Return_BadRequest()
    {
        //Arrange
        var newProductDTO = new ProductDTORequest
        {
            Name = "Test product",
            Description = "Testing a product name",
            Price = 10000,
            Stock = 100,
            ImageURL = "FakeUrl.svg",
            CategoryId = 1
        };

        //Simulationg violationError
        _controller.ModelState.AddModelError("Name", "The name field is required");

        //Act
        var data = await _controller.Post(newProductDTO);

        //Assert
        var badRequestResult = data.Result.Should().BeOfType<BadRequestObjectResult>();
        badRequestResult.Subject.StatusCode.Should().Be(400);
    }

    [Fact]
    public async Task PostProductsUnitValidatingAnnotations_Return_BadRequest()
    {
        //Arrange
        var newProductDTO = new ProductDTORequest
        {
            Name = null,
            Description = "Testing a product name",
            Price = 11000,
            Stock = -1,
            ImageURL = "FakeUrl.svg",
            CategoryId = 1
        };

        var validationContext = new ValidationContext(newProductDTO);
        var validationResults = new List<ValidationResult>();

        // Act
        var isValid = Validator.TryValidateObject(newProductDTO, validationContext, validationResults, true);

        // Assert
        isValid.Should().BeFalse();
        validationResults.Should().Contain(v => v.MemberNames.Contains("Name"));
        validationResults.Should().Contain(v => v.MemberNames.Contains("Price"));
        validationResults.Should().Contain(v => v.MemberNames.Contains("Stock"));
    }
}

