using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodStore.Data.Entities;
using FoodStore.Service.GenericRepository;

namespace FoodStore.Service.IRepository
{
    public interface ICategoryRepository : IGenericBase<Category>
    {
        Task<bool> AnyCategoryAsync(int CategoryId);
        Task<List<Category>> GetCategoriesAsync();
    }
}