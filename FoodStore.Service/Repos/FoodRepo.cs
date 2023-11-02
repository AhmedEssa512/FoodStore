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
    public class FoodRepo : GenericBase<Food> , IFoodRepo
    {
          private readonly DbSet<Food> _Food;

        public FoodRepo(ApplicationDbContext context) : base(context)
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