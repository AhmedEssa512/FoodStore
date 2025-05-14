using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodStore.Data.DTOS;
using FoodStore.Data.Entities;
using FoodStore.Data.Context;
using FoodStore.Data.GenericRepository;
using FoodStore.Data.IRepository;
using Microsoft.EntityFrameworkCore;

namespace FoodStore.Data.Repository
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
        public async Task<IEnumerable<Food>> GetFoodsByIdsAsync(List<int> foodIds)
        {
            return await _context.foods
                .Where(f => foodIds.Contains(f.Id)) 
                .ToListAsync();
        }

        public async Task<(IReadOnlyList<Food>, int TotalCount)> GetPaginatedFoods(PaginationParams paginationParams, int? categoryId = null)
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
            .Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
            .Take(paginationParams.PageSize)
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

        public async Task<(IReadOnlyList<Food>, int TotalCount)> SearchFoodsInDatabaseAsync(string normalizedQuery, PaginationParams paginationParams)
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
                .Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
                .Take(paginationParams.PageSize)
                .ToListAsync();

            return (foods.AsReadOnly(), totalCount);
        }
    }
}