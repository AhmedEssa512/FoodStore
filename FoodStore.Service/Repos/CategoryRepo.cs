using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodStore.Data.Entities;
using FoodStore.Service.Context;
using FoodStore.Service.GenericRepository;
using FoodStore.Service.IRepos;
using Microsoft.EntityFrameworkCore;

namespace FoodStore.Service.Repos
{
    public class CategoryRepo : GenericBase<Category>, ICategoryRepo
    {


        private DbSet<Category> _category;

        public CategoryRepo(ApplicationDbContext context) : base(context)
        {
            _category = context.Set<Category>();
        }

        public async Task<List<Category>> GetCategoriesAsync()
        {
            return await _category.Include(f => f.Foods).ToListAsync();
        }

        public async Task<bool> IsFoundCategory(int CategoryId)
        {
            return await _category.AnyAsync(c =>c.Id == CategoryId);
        }
    }
}