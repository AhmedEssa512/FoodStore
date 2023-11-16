using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodStore.Data.Entities;
using FoodStore.Service.GenericRepository;

namespace FoodStore.Service.Abstracts
{
    public interface ICategoryService : IGenericBase<Category>
    {
        public Task<List<Category>> GetCategoriesAsync();
        public Task<bool> IsFoundCategory(int CategoryId);
    }
}