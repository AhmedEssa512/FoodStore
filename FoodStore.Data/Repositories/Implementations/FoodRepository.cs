using FoodStore.Data.Entities;
using FoodStore.Data.Context;
using Microsoft.EntityFrameworkCore;
using FoodStore.Data.Repositories.Interfaces;

namespace FoodStore.Data.Repositories.Implementations
{
    public class FoodRepository : GenericBase<Food>, IFoodRepository
    {
        private readonly ApplicationDbContext _context;
        public FoodRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<bool> AnyFoodAsync(int foodId)
        {
            return await _context.foods.AnyAsync(f => f.Id == foodId);
        }
        public async Task<IReadOnlyList<Food>> GetFoodsByIdsAsync(List<int> foodIds)
        {
            var idSet = new HashSet<int>(foodIds); 

             return await _context.foods
                .Where(f => idSet.Contains(f.Id))
                .ToListAsync();
        }

        public async Task<(IReadOnlyList<Food>, int TotalCount)> GetPaginatedFoods(int pageNumber, int pageSize, int? categoryId = null)
        {
            IQueryable<Food> query = _context.foods.AsQueryable();

            if (categoryId.HasValue)
            {
                query = query.Where(f => f.CategoryId == categoryId.Value);
            }

            int totalCount = await query.CountAsync();

            var foods = await query
            .AsNoTracking()
            .OrderBy(f => f.Name)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

            return (foods, totalCount);
        }

        public async Task<decimal> GetPriceAsync(int foodId)
        {
            return await _context.foods
            .Where(f => f.Id == foodId)
            .Select(f => f.Price)
            .FirstOrDefaultAsync();
        }

        public async Task<(IReadOnlyList<Food>, int TotalCount)> SearchFoodsInDatabaseAsync(string normalizedQuery, int pageNumber, int pageSize)
        {

            var query = _context.foods.AsQueryable();

            if (!string.IsNullOrWhiteSpace(normalizedQuery))
            {
                query = query.Where(f =>
                 f.Name.ToLower().Contains(normalizedQuery) ||
                 f.Description.ToLower().Contains(normalizedQuery));
            }

            var totalCount = await query.CountAsync();
        
            var foods = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (foods.AsReadOnly(), totalCount);
        }
    }
}