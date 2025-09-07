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
        private readonly IImageService _imageService;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;
        public FoodService(IUnitOfWork unitOfWork, IImageService imageService, IMapper mapper, ICacheService cacheService)
        {
            _unitOfWork = unitOfWork;
            _imageService = imageService;
            _mapper = mapper;
            _cacheService = cacheService;
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
                _cacheService.RemoveByPrefix("Foods_");

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

                _unitOfWork.Food.Update(food);
                await _unitOfWork.SaveChangesAsync();

                await _unitOfWork.CommitTransactionAsync();
                _cacheService.RemoveByPrefix("Foods_");
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task UpdateFoodAvailabilityAsync(int foodId, bool isAvailable)
        {
            var food = await _unitOfWork.Food.GetByIdAsync(foodId)
                ?? throw new NotFoundException("Food is not found");

            food.IsAvailable = isAvailable;

             _unitOfWork.Food.Update(food);
            await _unitOfWork.SaveChangesAsync();
            _cacheService.RemoveByPrefix("Foods_");
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

                _unitOfWork.Food.Delete(food);
                await _unitOfWork.SaveChangesAsync(); 

                await _unitOfWork.CommitTransactionAsync();
                _cacheService.RemoveByPrefix("Foods_");
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

             var result = _mapper.Map<FoodResponseDto>(food);

             return result;
        }

        public async Task<PagedResponse<FoodResponseDto>> GetFoodsAsync(PaginationParams paginationParams, int? categoryId = null)
        {
            string cacheKey = $"Foods_Category_{categoryId?.ToString() ?? "All"}_Page_{paginationParams.PageNumber}_Size_{paginationParams.PageSize}";
            Console.WriteLine($"[Cache Key] get {cacheKey} ");
            var cachedResponse = _cacheService.Get<PagedResponse<FoodResponseDto>>(cacheKey);
            if (cachedResponse != null)
                return cachedResponse;

            var (foods ,totalCount) = await _unitOfWork.Food.GetPaginatedFoods(paginationParams.PageNumber, paginationParams.PageSize,categoryId);

            var foodDtos = _mapper.Map<IReadOnlyList<FoodResponseDto>>(foods);

            var response = new PagedResponse<FoodResponseDto>(
                items: foodDtos,
                totalCount: totalCount,
                pageNumber: paginationParams.PageNumber,
                pageSize: paginationParams.PageSize
            );

            _cacheService.Set(cacheKey, response, TimeSpan.FromMinutes(10), TimeSpan.FromMinutes(30));
            
            return response;
        }



        public async Task<PagedResponse<FoodResponseDto>> SearchFoodsAsync(string searchQuery, PaginationParams paginationParams)
        {

            string normalizedQuery = searchQuery.Trim().ToLower();

            string cacheKey = $"Foods_Search_{normalizedQuery}_Page_{paginationParams.PageNumber}_Size_{paginationParams.PageSize}";

            var cacheResult = _cacheService.Get<PagedResponse<FoodResponseDto>>(cacheKey);
            if(cacheResult != null)
                return cacheResult;

            var (foods, totalCount) = await _unitOfWork.Food.SearchFoodsInDatabaseAsync(normalizedQuery, paginationParams.PageNumber, paginationParams.PageSize);

            var foodDtos = _mapper.Map<IReadOnlyList<FoodResponseDto>>(foods);

            var result = new PagedResponse<FoodResponseDto>(
                items: foodDtos,
                totalCount: totalCount,
                pageNumber: paginationParams.PageNumber,
                pageSize: paginationParams.PageSize
            );


            _cacheService.Set(cacheKey, result, TimeSpan.FromMinutes(10), TimeSpan.FromMinutes(30));
            
            return result;
        }

        public async Task<IReadOnlyList<FoodResponseDto>> GetFoodDetailsByIdsAsync(List<int> foodIds)
        {
            var foods = await _unitOfWork.Food.GetFoodsByIdsAsync(foodIds);

            return _mapper.Map<IReadOnlyList<FoodResponseDto>>(foods);
        }

        public async Task<PagedResponse<FoodAdminListDto>> GetFoodsForAdminAsync(PaginationParams paginationParams,int? categoryId = null)
        {
            var (foods, totalCount) = await _unitOfWork.Food.GetPaginatedFoodsForAdmin(paginationParams.PageNumber, paginationParams.PageSize, categoryId);

            var foodDtos = _mapper.Map<IReadOnlyList<FoodAdminListDto>>(foods);

            var result = new PagedResponse<FoodAdminListDto>(
                items: foodDtos,
                totalCount: totalCount,
                pageNumber: paginationParams.PageNumber,
                pageSize: paginationParams.PageSize
            );
            return result;
        }

    }
}