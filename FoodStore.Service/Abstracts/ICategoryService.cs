using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodStore.Data.DTOS;
using FoodStore.Data.Entities;


namespace FoodStore.Service.Abstracts
{
    public interface ICategoryService
    {
        Task AddCategoryAsync(CategoryDto categoryDto);
        Task DeleteCategoryAsync(int categoryId);
        Task UpdateCategoryAsync(int categoryId,CategoryDto categoryDto);
        Task<List<Category>> GetCategoriesAsync();
        Task<Category> GetCategoryAsync(int categoryId);

    }
}