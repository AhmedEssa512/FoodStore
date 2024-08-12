using FluentAssertions;
using FoodStore.Api.Controllers;
using FoodStore.Data.Entities;
using FoodStore.Service.Abstracts;
using FoodStore.Service.Authorization;
using FoodStore.Service.Context;
using FoodStore.Service.Implementations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Moq;

namespace FoodStore.XUnitTest;

public class CategoryControllerTests
{
    private readonly Mock<ICategoryService> _mockCategoryService;
    private readonly Mock<IStringLocalizer<CategoryController>> _mockLocalizer;
    private readonly CategoryController _controller;

    public CategoryControllerTests()
    {
        _mockCategoryService = new Mock<ICategoryService>();
        _mockLocalizer = new Mock<IStringLocalizer<CategoryController>>();
        _controller = new CategoryController(_mockCategoryService.Object, _mockLocalizer.Object);
    }


    [Fact]
    public async Task DeleteAsync_CategoryExists_ReturnsOkResult()
    {
        // Arrange
        var categoryId = 1;
        var category = new Category(); // Assuming a Category class exists

        _mockCategoryService.Setup(service => service.GetByIdAsync(categoryId))
                            .ReturnsAsync(category);
        _mockLocalizer.Setup(localizer => localizer["Deleted"])
                      .Returns(new LocalizedString("Deleted", "Category deleted successfully"));

        // Act
        var result = await _controller.DeleteAsync(categoryId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal("Category deleted successfully", okResult.Value);
    }


     [Fact]
    public async Task DeleteAsync_CategoryDoesNotExist_ReturnsNotFoundResult()
    {
        // Arrange
        var categoryId = 1;
        _mockCategoryService.Setup(service => service.GetByIdAsync(categoryId))
                            .ReturnsAsync((Category)null);
        _mockLocalizer.Setup(localizer => localizer["CategoryIsNotFound"])
                      .Returns(new LocalizedString("CategoryIsNotFound", "Category not found"));

        // Act
        var result = await _controller.DeleteAsync(categoryId);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("Category not found", notFoundResult.Value);
    }
}
