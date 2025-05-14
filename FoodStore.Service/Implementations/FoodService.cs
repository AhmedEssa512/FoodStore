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
using AutoMapper;

namespace FoodStore.Service.Implementations
{
    public class FoodService : IFoodService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMemoryCache _memoryCache;
        private readonly IImageService _imageService;
        private readonly IMapper _mapper;
        public FoodService(IUnitOfWork unitOfWork, IMemoryCache memoryCache, IImageService imageService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _memoryCache = memoryCache;
            _imageService = imageService;
            _mapper = mapper;
        }
        public async Task<Food> CreateFoodAsync(FoodCreateDto foodCreateDto)
        {
           
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                if(! await _unitOfWork.Category.AnyCategoryAsync(foodCreateDto.CategoryId) ) 
                 throw new NotFoundException("Category is not found");

                  var food = _mapper.Map<Food>(foodCreateDto);

                if (foodCreateDto.Photo != null && foodCreateDto.Photo.Length > 0)
                {
                    food.ImageUrl = await _imageService.SaveImageAsync(foodCreateDto.Photo);
                }


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
                   await _imageService.DeleteImageAsync(food.ImageUrl); 
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
            return await _unitOfWork.Food.GetByIdAsync(foodId) ??
             throw new NotFoundException("Food is not found");
        }

        public async Task<PaginatedResult<Food>> GetFoodsAsync(PaginationParams paginationParams, int? categoryId = null)
        {
            string cacheKey = $"Foods_Category_{categoryId?.ToString() ?? "All"}_Page_{paginationParams.PageNumber}_Size_{paginationParams.PageSize}";

            if (_memoryCache.TryGetValue(cacheKey, out PaginatedResult<Food> cachedResult))
                return cachedResult;

            var (foods ,totalCount) = await _unitOfWork.Food.GetPaginatedFoods(paginationParams,categoryId);

            var result = new PaginatedResult<Food>
            {
                Items = foods,
                TotalCount = totalCount
            };

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(10));

                _memoryCache.Set(cacheKey, result, cacheEntryOptions);
            
            return result;
        }


        public async Task UpdateFoodAsync(int foodId, FoodUpdateDto foodDto)
        {
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                var food = await _unitOfWork.Food.GetByIdAsync(foodId) 
                            ?? throw new NotFoundException("Food is not found");

                if (foodDto.CategoryId.HasValue)
                {
                    bool categoryExists = await _unitOfWork.Category
                    .AnyCategoryAsync(foodDto.CategoryId.Value);

                    if (!categoryExists)
                        throw new NotFoundException("Category is not found");

                    food.CategoryId = foodDto.CategoryId.Value; 
                }


                _mapper.Map(foodDto, food); 

               
                if (foodDto.Image != null && foodDto.Image.Length > 0)
                {
                    string oldImageUrl = food.ImageUrl;
                    string newImageUrl = await _imageService.SaveImageAsync(foodDto.Image);

                    food.ImageUrl = newImageUrl;

                    await _imageService.DeleteImageAsync(oldImageUrl);
                }

                await _unitOfWork.Food.UpdateAsync(food);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }


        public async Task<PaginatedResult<Food>> SearchFoodsAsync(string searchQuery, PaginationParams paginationParams)
        {

            string normalizedQuery = searchQuery.Trim().ToLower();

            string cacheKey = $"Foods_Search_{normalizedQuery}_Page_{paginationParams.PageNumber}_Size_{paginationParams.PageSize}";


            if (_memoryCache.TryGetValue(cacheKey, out PaginatedResult<Food> cashedFoods))
                return cashedFoods;

            var (foods, totalCount) = await _unitOfWork.Food.SearchFoodsInDatabaseAsync(normalizedQuery, paginationParams);

            var result = new PaginatedResult<Food>
            {
                Items = foods,
                TotalCount = totalCount
            };

            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(10));

            _memoryCache.Set(cacheKey, result, cacheEntryOptions);
            

            return result;
        }

        public async Task<IEnumerable<Food>> GetFoodDetailsByIdsAsync(List<int> foodIds)
        {
            return await _unitOfWork.Food.GetFoodsByIdsAsync(foodIds);
        }


    }
}