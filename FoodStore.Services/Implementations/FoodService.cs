using FoodStore.Data.Entities;
using FoodStore.Contracts.DTOs.Food;
using FoodStore.Services.Exceptions;
using Microsoft.Extensions.Caching.Memory;
using AutoMapper;
using FoodStore.Data.Repositories.Interfaces;
using FoodStore.Contracts.Common;
using FoodStore.Contracts.QueryParams;
using FoodStore.Contracts.Interfaces;

namespace FoodStore.Services.Implementations
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
        public async Task<FoodResponseDto> CreateFoodAsync(FoodCreateDto foodCreateDto, Stream imageStream, string originalFileName)
        {
           
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                if(! await _unitOfWork.Category.AnyCategoryAsync(foodCreateDto.CategoryId) ) 
                     throw new NotFoundException("Category is not found");

                 // Save the image and get the URL
                string imageUrl = await _imageService.SaveImageAsync(imageStream, originalFileName);

                var food = _mapper.Map<Food>(foodCreateDto);

                food.ImageUrl = imageUrl;

                await _unitOfWork.Food.AddAsync(food);
                await _unitOfWork.SaveChangesAsync();

                await _unitOfWork.CommitTransactionAsync();

                return _mapper.Map<FoodResponseDto>(food);
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
               
        }


        public async Task UpdateFoodAsync(int foodId, FoodUpdateDto foodDto, Stream? imageStream = null, string? originalFileName = null)
        {
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                var food = await _unitOfWork.Food.GetByIdAsync(foodId) ??
                    throw new NotFoundException("Food is not found");

                if (foodDto.CategoryId.HasValue)
                {
                    bool categoryExists = await _unitOfWork.Category
                    .AnyCategoryAsync(foodDto.CategoryId.Value);

                    if (!categoryExists)
                        throw new NotFoundException("Category is not found");

                    food.CategoryId = foodDto.CategoryId.Value; 
                }


                if(imageStream != null && originalFileName != null)
                {
                    food.ImageUrl = await _imageService.ReplaceImageAsync(imageStream, originalFileName, food.ImageUrl);
                }

                _mapper.Map(foodDto, food); 

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

        public async Task<FoodResponseDto> GetFoodByIdAsync(int foodId)
        {
            var food =  await _unitOfWork.Food.GetByIdAsync(foodId) ??
             throw new NotFoundException($"Food with ID {foodId} was not found.");

             var responseDto = _mapper.Map<FoodResponseDto>(food);

             return responseDto;
        }

        public async Task<PagedResponse<FoodResponseDto>> GetFoodsAsync(PaginationParams paginationParams, int? categoryId = null)
        {
            string cacheKey = $"Foods_Category_{categoryId?.ToString() ?? "All"}_Page_{paginationParams.PageNumber}_Size_{paginationParams.PageSize}";

            // Use pattern matching to ensure the cached object is of the expected type.
            if (_memoryCache.TryGetValue(cacheKey, out var cachedObj) && 
                cachedObj is PagedResponse<FoodResponseDto> cachedResponse)
                return cachedResponse;

            var (foods ,totalCount) = await _unitOfWork.Food.GetPaginatedFoods(paginationParams.PageNumber, paginationParams.PageSize,categoryId);

            var foodDtos = _mapper.Map<IReadOnlyList<FoodResponseDto>>(foods);

            var response = new PagedResponse<FoodResponseDto>(
                items: foodDtos,
                totalCount: totalCount,
                pageNumber: paginationParams.PageNumber,
                pageSize: paginationParams.PageSize
            );

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(10));

                _memoryCache.Set(cacheKey, response, cacheEntryOptions);
            
            return response;
        }



        public async Task<PagedResponse<FoodResponseDto>> SearchFoodsAsync(string searchQuery, PaginationParams paginationParams)
        {

            string normalizedQuery = searchQuery.Trim().ToLower();

            string cacheKey = $"Foods_Search_{normalizedQuery}_Page_{paginationParams.PageNumber}_Size_{paginationParams.PageSize}";


            if (_memoryCache.TryGetValue(cacheKey, out var cachedObj) && 
                cachedObj is PagedResponse<FoodResponseDto> cachedResult)
                return cachedResult;

            var (foods, totalCount) = await _unitOfWork.Food.SearchFoodsInDatabaseAsync(normalizedQuery, paginationParams.PageNumber, paginationParams.PageSize);

            var foodDtos = _mapper.Map<IReadOnlyList<FoodResponseDto>>(foods);

            var result = new PagedResponse<FoodResponseDto>(
                items: foodDtos,
                totalCount: totalCount,
                pageNumber: paginationParams.PageNumber,
                pageSize: paginationParams.PageSize
            );

            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(10));

            _memoryCache.Set(cacheKey, result, cacheEntryOptions);
            
            return result;
        }

        public async Task<IReadOnlyList<FoodResponseDto>> GetFoodDetailsByIdsAsync(List<int> foodIds)
        {
            var foods = await _unitOfWork.Food.GetFoodsByIdsAsync(foodIds);

            return _mapper.Map<IReadOnlyList<FoodResponseDto>>(foods);
        }


    }
}