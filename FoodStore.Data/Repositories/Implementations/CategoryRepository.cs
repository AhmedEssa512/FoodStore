using FoodStore.Data.Entities;
using FoodStore.Data.Context;
using Microsoft.EntityFrameworkCore;
using FoodStore.Data.Repositories.Interfaces;

namespace FoodStore.Data.Repositories.Implementations
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