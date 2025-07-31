using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using TrainingAPI.Controllers;
using TrainingAPI.DTOs;

namespace TrainingAPIXunitTests.UnitTests;

public class GetProductUnitTests : IClassFixture<ProductsUnitTestController>
{
    private readonly ProductsController _controller;

    public GetProductUnitTests(ProductsUnitTestController controller)
    {
        _controller = new ProductsController(controller.repository, controller.mapper);
    }

    [Fact]
    public async Task GetProductById_OkResult()
    {
        //Arrange 
        var prodId = 3;

        //Act
        var data = await _controller.Get(prodId);

        //Assert
        //var okResult = Assert.IsType<OkObjectResult>(data.Result);
        //Assert.Equal(200, okResult.StatusCode);

        //Assert (Fluent Assertions!!)
        data.Result.Should().BeOfType<OkObjectResult>()
            .Which.StatusCode.Should().Be(200);
    }

    [Fact]
    public async Task GetProductById_NotFoundResult()
    {
        //Arrange 
        var prodId = 2;

        //Act
        var data = await _controller.Get(prodId);

        //Assert (Fluent Assertions!!)
        data.Result.Should().BeOfType<NotFoundObjectResult>()
            .Which.StatusCode.Should().Be(404);
    }

    [Fact]
    public async Task GetProductById_BadRequestResult()
    {
        //Arrange 
        var prodId = -2;

        //Act
        var data = await _controller.Get(prodId);

        //Assert (Fluent Assertions!!)
        data.Result.Should().BeOfType<BadRequestObjectResult>()
            .Which.StatusCode.Should().Be(400);
    }

    [Fact]
    public async Task GetAllProducts_ReturnOKAndListOfProductDTO()
    {
        //Act
        var data = await _controller.GetAll(null, null);

        //Assert (Fluent Assertions!!)
        data.Result.Should().BeOfType<OkObjectResult>()
            .Which.Value.Should().BeAssignableTo<IEnumerable<ProductDTOResponse>>()
            .And.NotBeNull();
    }
}
