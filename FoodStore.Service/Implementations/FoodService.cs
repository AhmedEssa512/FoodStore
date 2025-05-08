using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodStore.Data.Entities;
using FoodStore.Data.Context;
using FoodStore.Service.Abstracts;
using Microsoft.EntityFrameworkCore;
using FoodStore.Data.DTOS;
using FoodStore.Data.IRepository;
using FoodStore.Service.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;

namespace FoodStore.Service.Implementations
{
    public class FoodService : IFoodService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMemoryCache _memoryCache;
        private readonly IImageService _imageService;
        public FoodService(IUnitOfWork unitOfWork, IMemoryCache memoryCache, IImageService imageService)
        {
            _unitOfWork = unitOfWork;
            _memoryCache = memoryCache;
            _imageService = imageService;
        }
        public async Task<Food> CreateFoodAsync(Food food)
        {

            await _unitOfWork.BeginTransactionAsync();

            try
            {
                if(! await _unitOfWork.Category.AnyCategoryAsync(food.CategoryId) ) 
                 throw new NotFoundException("Category is not found");

                await _unitOfWork.Food.AddAsync(food);

                await _unitOfWork.SaveChangesAsync();

                await _unitOfWork.CommitTransactionAsync();

                return food;
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
               
        }


        public async Task DeleteFoodAsync(int foodId)
        {
            var food = await _unitOfWork.Food.GetByIdAsync(foodId) ?? throw new NotFoundException("Food is not found");

             await _unitOfWork.BeginTransactionAsync();

            try
            {
                if (!string.IsNullOrWhiteSpace(food.ImageUrl))
                {
                    _imageService.DeleteImageAsync(food.ImageUrl); 
                }

                await _unitOfWork.Food.DeleteAsync(food);
                await _unitOfWork.SaveChangesAsync(); 

                await _unitOfWork.CommitTransactionAsync();
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }

        }

        public async Task<Food> GetFoodAsync(int foodId)
        {
            return await _unitOfWork.Food.GetByIdAsync(foodId) ?? throw new NotFoundException("Food is not found");
        }

        public async Task<IEnumerable<Food>> GetFoodsAsync(PaginationParams paginationParams, int? categoryId = null)
        {
            // Generate a cache key based on the pagination params
            string cacheKey = $"Foods_Category_{(categoryId.HasValue ? categoryId.Value.ToString() : "All")}_Page_{paginationParams.PageNumber}_Size_{paginationParams.PageSize}";

            if (!_memoryCache.TryGetValue(cacheKey, out IEnumerable<Food> foods))
            {
                foods = await _unitOfWork.Food.GetPaginatedFoods(paginationParams,categoryId);
 
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(10));

                _memoryCache.Set(cacheKey, foods, cacheEntryOptions);
            }

            return foods;
        }

        public async Task UpdateFoodAsync(int foodId, FoodDto foodDto)
        {

            await _unitOfWork.BeginTransactionAsync();

            try
            {
                var food = await _unitOfWork.Food.GetByIdAsync(foodId) ?? throw new NotFoundException("Food is not found");

                if(! await _unitOfWork.Category.AnyCategoryAsync(foodDto.CategoryId)) throw new NotFoundException("Category is not found");

                food.Name = foodDto.Name;
                food.Description = foodDto.Description;
                food.Price = foodDto.Price;
                food.CategoryId = foodDto.CategoryId;

            string oldImageUrl = food.ImageUrl;

            if(foodDto.Photo != null && foodDto.Photo.Length > 0)
            {
                string newImageUrl = await _imageService.SaveImageAsync(foodDto.Photo);

                food.ImageUrl = newImageUrl;

                _imageService.DeleteImageAsync(oldImageUrl);
            }

                await _unitOfWork.Food.UpdateAsync(food);

                await _unitOfWork.SaveChangesAsync();

                await _unitOfWork.CommitTransactionAsync();
            }
            catch(Exception)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }

        }

        public async Task<IReadOnlyList<Food>> SearchFoodsAsync(string searchQuery, PaginationParams paginationParams)
        {
            if (string.IsNullOrWhiteSpace(searchQuery))
            {
                return new List<Food>().AsReadOnly(); 
            }

            string cacheKey = $"Foods_Search_{searchQuery.Trim().ToLower()}_Page_{paginationParams.PageNumber}_Size_{paginationParams.PageSize}";


            if (!_memoryCache.TryGetValue(cacheKey, out IReadOnlyList<Food> foods))
            {

                var foodList = await _unitOfWork.Food.SearchFoodsInDatabaseAsync(searchQuery, paginationParams);

                foods = foodList;

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(10));

                _memoryCache.Set(cacheKey, foods, cacheEntryOptions);
            }

            return foods;
        }

        public async Task<IEnumerable<Food>> GetFoodDetailsByIdsAsync(List<int> foodIds)
        {
            if (foodIds == null || !foodIds.Any())
               return [];

             return await _unitOfWork.Food.GetFoodsByIdsAsync(foodIds);

        }


    }
}