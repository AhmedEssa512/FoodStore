using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodStore.Data.DTOS;
using FoodStore.Data.Entities;
using FoodStore.Service.Context;
using FoodStore.Service.GenericRepository;
using FoodStore.Service.IRepository;
using Microsoft.EntityFrameworkCore;

namespace FoodStore.Service.Repository
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

        public async Task<IEnumerable<Food>> GetPaginatedFoods(PaginationParams paginationParams)
        {
            return await _context.foods
            .Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
            .Take(paginationParams.PageSize)
            .ToListAsync();
        }

        public async Task<double> GetPriceAsync(int foodId)
        {
            return await _context.foods
            .Where(f => f.Id == foodId)
            .Select(f => f.Price)
            .FirstOrDefaultAsync();
        }

        public async Task<IReadOnlyList<Food>> SearchFoodsInDatabaseAsync(string searchQuery, PaginationParams paginationParams)
        {
            // Normalize the search query to handle case-insensitivity and trimming
            string normalizedQuery = searchQuery.Trim().ToLower();

            var query = _context.foods.AsQueryable();


            if (!string.IsNullOrWhiteSpace(normalizedQuery))
            {
                query = query.Where(f => f.Name.Contains(normalizedQuery) || f.Description.Contains(normalizedQuery));
            }
        
            var paginatedFoods = await query
                .Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
                .Take(paginationParams.PageSize)
                .ToListAsync();

            return paginatedFoods;
        }
    }
}