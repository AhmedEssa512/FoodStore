using FoodStore.Contracts.DTOs.Category;
using FoodStore.Data.Entities;
using FoodStore.Data.Repositories.Interfaces;
using FoodStore.Services.Exceptions;
using FoodStore.Services.Implementations;



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
            _mockCategoryRepository = new Mock<ICategoryRepository>();

            _mockUnitOfWork.Setup(u => u.Category).Returns(_mockCategoryRepository.Object);

            _service = new CategoryService(_mockUnitOfWork.Object);
        }

        [Fact]
        public async Task AddCategoryAsync_CategoryIsValid_AddsCategory()
        {
            // Arrange
            var newCategoryDto  = new CategoryDto {  Name = "Electronics" };

            // Act
            await _service.AddCategoryAsync(newCategoryDto );

            // Assert
            _mockCategoryRepository.Verify(repo => repo.AddAsync(It.Is<Category>(c => c.Name == newCategoryDto .Name)), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Once);
        }


        [Fact]
        public async Task DeleteCategoryAsync_CategoryExists_DeletesCategoryAndSaves()
        {
            // Arrange
            var existingCategory = new Category { Id = 1, Name = "Pizza" };

            _mockCategoryRepository
                .Setup(r => r.GetByIdAsync(existingCategory.Id))
                .ReturnsAsync(existingCategory);

            // Act
            await _service.DeleteCategoryAsync(existingCategory.Id);

            // Assert
            _mockCategoryRepository.Verify(r => r.DeleteAsync(existingCategory), Times.Once);
            _mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteCategoryAsync_CategoryNotFound_ThrowsNotFoundException()
        {
            // Arrange
            var nonExistentCategoryId = 99;

            _mockCategoryRepository
                .Setup(r => r.GetByIdAsync(nonExistentCategoryId))
                .ReturnsAsync((Category?)null);

            // Act
            Func<Task> act = async () => await _service.DeleteCategoryAsync(nonExistentCategoryId);

            // Assert
            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage("Category is not found");

            _mockCategoryRepository.Verify(r => r.DeleteAsync(It.IsAny<Category>()), Times.Never);
            _mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task UpdateCategoryAsync_CategoryExists_UpdatesCategoryAndSaves()
        {
            // Arrange
            var existingCategory = new Category { Id = 1, Name = "OldName" };
            var updateDto = new CategoryDto { Name = "NewName" };

            _mockCategoryRepository
                .Setup(r => r.GetByIdAsync(existingCategory.Id))
                .ReturnsAsync(existingCategory);

            // Act
            await _service.UpdateCategoryAsync(existingCategory.Id, updateDto);

            // Assert
            existingCategory.Name.Should().Be(updateDto.Name);

            _mockCategoryRepository.Verify(r => r.UpdateAsync(existingCategory), Times.Once);
            _mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateCategoryAsync_CategoryNotFound_ThrowsNotFoundException()
        {
            // Arrange
            var nonExistentCategoryId = 1;
            var updateDto = new CategoryDto { Name = "NewName" };

            _mockCategoryRepository
                .Setup(r => r.GetByIdAsync(nonExistentCategoryId))
                .ReturnsAsync((Category?)null);

            // Act
            Func<Task> act = async () => await _service.UpdateCategoryAsync(nonExistentCategoryId, updateDto);

            // Assert
            await act.Should()
                .ThrowAsync<NotFoundException>();

            _mockCategoryRepository.Verify(r => r.UpdateAsync(It.IsAny<Category>()), Times.Never);
            _mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task GetCategoryByIdAsync_CategoryExists_ReturnsCategoryResponseDto()
        {
            // Arrange
            var existingCategory = new Category { Id = 1, Name = "Cheese" };

            _mockCategoryRepository
                .Setup(r => r.GetByIdAsync(existingCategory.Id))
                .ReturnsAsync(existingCategory);

            // Act
            var result = await _service.GetCategoryByIdAsync(existingCategory.Id);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(existingCategory.Id);
            result.Name.Should().Be(existingCategory.Name);
        }

        [Fact]
        public async Task GetCategoryByIdAsync_NotFound_ThrowsNotFoundException()
        {
            // Arrange
            var nonExistentCategoryId = 1;

            _mockCategoryRepository
                .Setup(r => r.GetByIdAsync(nonExistentCategoryId))
                .ReturnsAsync((Category?)null);

            // Act
            Func<Task> act = async () => await _service.GetCategoryByIdAsync(nonExistentCategoryId);

            // Assert
            await act.Should()
                .ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task GetCategoriesAsync_WhenCategoriesExist_ReturnsMappedDtos()
        {
            // Arrange
            var categories = new List<Category>
            {
                new Category { Id = 1, Name = "Pizza" },
                new Category { Id = 2, Name = "Cheese" }
            };

            _mockCategoryRepository
                .Setup(r => r.GetCategoriesAsync())
                .ReturnsAsync(categories);

            // Act
            var result = await _service.GetCategoriesAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result.Should().ContainSingle(c => c.Id == 1 && c.Name == "Pizza");
            result.Should().ContainSingle(c => c.Id == 2 && c.Name == "Cheese");
        }

        [Fact]
        public async Task GetCategoriesAsync_WhenNoCategories_ReturnsEmptyList()
        {
            // Arrange
            var emptyCategories = new List<Category>();

            _mockCategoryRepository
                .Setup(r => r.GetCategoriesAsync())
                .ReturnsAsync(emptyCategories);

            // Act
            var result = await _service.GetCategoriesAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }
    
    }
}