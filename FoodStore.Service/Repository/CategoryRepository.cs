using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodStore.Data.Entities;
using FoodStore.Service.Context;
using FoodStore.Service.GenericRepository;
using FoodStore.Service.IRepository;
using Microsoft.EntityFrameworkCore;

namespace FoodStore.Service.Repository
{
    public class CategoryRepository : GenericBase<Category>, ICategoryRepository
    {
        private readonly ApplicationDbContext _context;
        public CategoryRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<bool> AnyCategoryAsync(int CategoryId)
        {
            return await _context.categories.AnyAsync(c =>c.Id == CategoryId);    
        }

        public async Task<List<Category>> GetCategoriesAsync()
        {
            return await _context.categories.AsNoTracking().ToListAsync();
        }
    }
}