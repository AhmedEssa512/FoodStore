using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodStore.Data.Entities;
using FoodStore.Service.Context;
using FoodStore.Service.GenericRepository;
using FoodStore.Service.Abstracts;
using Microsoft.EntityFrameworkCore;

namespace FoodStore.Service.Implementations
{
    public class FoodService : GenericBase<Food> , IFoodService
    {
          private readonly DbSet<Food> _Food;

        public FoodService(ApplicationDbContext context) : base(context)
        {
            _Food = context.Set<Food>();
        }

        public async Task<List<Food>> GetFoodsAsync()
        {
          
          return await _Food.Include(c => c.category).ToListAsync();
            
        }

        public async Task<bool> IsFoundFoodId(int foodId)
        {
            return await _Food.AnyAsync(f => f.Id == foodId);
        }
    }
}