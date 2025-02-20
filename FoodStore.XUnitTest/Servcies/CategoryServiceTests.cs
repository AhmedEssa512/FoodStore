using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodStore.Data.DTOS;
using FoodStore.Data.Entities;
using FoodStore.Service.Implementations;
using FoodStore.Service.IRepository;



namespace FoodStore.XUnitTest.Servcies
{
    public class CategoryServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<ICategoryRepository> _mockCategoryRepository;
        private readonly CategoryService _service;


        public CategoryServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _service = new CategoryService(_mockUnitOfWork.Object);
            _mockCategoryRepository = new Mock<ICategoryRepository>();
            _mockUnitOfWork.Setup(u => u.Category).Returns(_mockCategoryRepository.Object);
        }

    [Fact]
    public async Task AddCategoryAsync_ValidCategory_AddsCategory()
    {
        // Arrange
        var categoryDto = new CategoryDto
        {
            Name = "Electronics",
            Description = "Various electronic items"
        };

        // Act
        await _service.AddCategoryAsync(categoryDto);

        // Assert
        _mockCategoryRepository.Verify(repo => repo.AddAsync(It.Is<Category>(c => c.Name == categoryDto.Name && c.Description == categoryDto.Description)), Times.Once);
        _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Once);
    }
    
    }
}