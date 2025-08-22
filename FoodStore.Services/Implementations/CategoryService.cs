using FoodStore.Data.Entities;
using FoodStore.Contracts.DTOs.Category;
using FoodStore.Services.Exceptions;
using FoodStore.Data.Repositories.Interfaces;
using FoodStore.Contracts.Interfaces;
using AutoMapper;

namespace FoodStore.Services.Implementations
{
    public class CategoryService : ICategoryService
    {


        private readonly IUnitOfWork _unitOfWork;

        public CategoryService(IUnitOfWork unitOfWork) 
        {
            _unitOfWork = unitOfWork;
        }

        public async Task AddCategoryAsync(CategoryDto categoryDto)
        {

            var category = new Category{ Name = categoryDto.Name };

            await _unitOfWork.Category.AddAsync(category);

            await _unitOfWork.SaveChangesAsync();

        }

        public async Task DeleteCategoryAsync(int categoryId)
        {
            var category = await _unitOfWork.Category.GetByIdAsync(categoryId);

            if(category is null) throw new NotFoundException("Category is not found");

            _unitOfWork.Category.Delete(category);

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IReadOnlyList<CategoryResponseDto>> GetCategoriesAsync()
        {
           var categories = await _unitOfWork.Category.GetCategoriesAsync();

            var categoryResponses = categories
            .Select(c => new CategoryResponseDto
            {
                Id = c.Id,
                Name = c.Name
            })
            .ToList();

           return categoryResponses;
        }

        public async Task<CategoryResponseDto> GetCategoryByIdAsync(int categoryId)
        {
            var category = await _unitOfWork.Category.GetByIdAsync(categoryId) ??
                throw new NotFoundException("Category is not found");

            var CategoryResponse = new CategoryResponseDto{
                Id = category.Id,
                Name = category.Name,
            };
            return CategoryResponse;
        }

        public async Task UpdateCategoryAsync(int categoryId, CategoryDto categoryDto)
        {
                
            var category = await _unitOfWork.Category.GetByIdAsync(categoryId) ??
                throw new NotFoundException("Category is not found");

            category.Name = categoryDto.Name;

            _unitOfWork.Category.Update(category);

            await _unitOfWork.SaveChangesAsync();

        }
    }
}