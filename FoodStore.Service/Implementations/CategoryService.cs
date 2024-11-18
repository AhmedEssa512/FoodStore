using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodStore.Data.Entities;
using FoodStore.Service.Context;
using FoodStore.Service.GenericRepository;
using FoodStore.Service.Abstracts;
using Microsoft.EntityFrameworkCore;
using FoodStore.Service.IRepository;
using FoodStore.Data.DTOS;
using Microsoft.Extensions.Localization;
using FoodStore.Service.Exceptions;

namespace FoodStore.Service.Implementations
{
    public class CategoryService : ICategoryService
    {


        private readonly ICategoryRepository _categoryRepo;
        private readonly IStringLocalizer<CategoryService> _localizer;

        public CategoryService(ICategoryRepository categoryRepo, IStringLocalizer<CategoryService> localizer) 
        {
            _categoryRepo = categoryRepo;
            _localizer = localizer;
        }

        public async Task AddCategoryAsync(CategoryDto categoryDto)
        {
            if (string.IsNullOrWhiteSpace(categoryDto.Name))
                throw new ValidationException("Name cannot be empty.");

            if (string.IsNullOrWhiteSpace(categoryDto.Description))
                throw new ValidationException("Description cannot be empty.");

            var category = new Category{Name = categoryDto.Name, Description = categoryDto.Description};

            await _categoryRepo.AddAsync(category);

        }

        public async Task DeleteCategoryAsync(int categoryId)
        {
            var category = await _categoryRepo.GetByIdAsync(categoryId);

            if(category is null) throw new NotFoundException("Category is not found");

            await _categoryRepo.DeleteAsync(category);
        }

        public async Task<List<Category>> GetCategoriesAsync()
        {
           return await _categoryRepo.GetCategoriesAsync();
        }

        public async Task<Category> GetCategoryAsync(int categoryId)
        {
            var category = await _categoryRepo.GetByIdAsync(categoryId);

            if(category is null) throw new NotFoundException("Category is not found");

            return category;
        }

        public async Task UpdateCategoryAsync(int categoryId, CategoryDto categoryDto)
        {
            if (string.IsNullOrWhiteSpace(categoryDto.Name))
                throw new ValidationException("Name cannot be empty.");

            if (string.IsNullOrWhiteSpace(categoryDto.Description))
                throw new ValidationException("Description cannot be empty.");
                
            var category = await _categoryRepo.GetByIdAsync(categoryId);

            if(category is null) throw new NotFoundException("Category is not found");

            category.Name = categoryDto.Name;
            category.Description = categoryDto.Description;

            await _categoryRepo.UpdateAsync(category);

        }
    }
}