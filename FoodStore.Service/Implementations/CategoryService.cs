using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodStore.Data.Entities;
using FoodStore.Data.Context;
using FoodStore.Service.Abstracts;
using Microsoft.EntityFrameworkCore;
using FoodStore.Data.IRepository;
using FoodStore.Data.DTOS;
using Microsoft.Extensions.Localization;
using FoodStore.Service.Exceptions;

namespace FoodStore.Service.Implementations
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

            await _unitOfWork.Category.DeleteAsync(category);

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<List<Category>> GetCategoriesAsync()
        {
           return await _unitOfWork.Category.GetCategoriesAsync();
        }

        public async Task<Category> GetCategoryAsync(int categoryId)
        {
            var category = await _unitOfWork.Category.GetByIdAsync(categoryId);

            if(category is null) throw new NotFoundException("Category is not found");

            return category;
        }

        public async Task UpdateCategoryAsync(int categoryId, CategoryDto categoryDto)
        {
                
            var category = await _unitOfWork.Category.GetByIdAsync(categoryId) ??
                throw new NotFoundException("Category is not found");

            category.Name = categoryDto.Name;

            await _unitOfWork.Category.UpdateAsync(category);

            await _unitOfWork.SaveChangesAsync();

        }
    }
}