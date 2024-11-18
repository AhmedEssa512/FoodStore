using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodStore.Data.Entities;
using FoodStore.Service.Context;
using FoodStore.Service.GenericRepository;
using FoodStore.Service.Abstracts;
using Microsoft.EntityFrameworkCore;
using FoodStore.Data.DTOS;
using FoodStore.Service.IRepository;
using FoodStore.Service.Exceptions;

namespace FoodStore.Service.Implementations
{
    public class FoodService : IFoodService
    {

        private readonly IFoodRepository _foodRepo;
        private readonly ICategoryRepository _categoryRepo;
        public FoodService(IFoodRepository foodRepo,ICategoryRepository categoryRepo)
        {
            _foodRepo = foodRepo;
            _categoryRepo = categoryRepo;
        }
        public async Task AddFoodAsync(FoodDto foodDto)
        {
               if(! await _categoryRepo.AnyCategoryAsync(foodDto.CategoryId) ) 
                 throw new NotFoundException("Category is not found");

               if (string.IsNullOrWhiteSpace(foodDto.Name))
                 throw new ValidationException("Name cannot be empty.");

               if (string.IsNullOrWhiteSpace(foodDto.Description))
                  throw new ValidationException("Description cannot be empty.");

                
               var food = new Food{
                   Name = foodDto.Name,
                   Description = foodDto.Description,
                   Price = foodDto.price,
                   CategoryId = foodDto.CategoryId,
               };

               if(foodDto.Photo is not null)
                {
                 using var  DataStream = new MemoryStream();
                 await foodDto.Photo.CopyToAsync(DataStream);
                 food.Photo = DataStream.ToArray();
                }

            await _foodRepo.AddAsync(food);
        }

        public async Task DeleteFoodAsync(int foodId)
        {
            var food = await _foodRepo.GetByIdAsync(foodId) ?? throw new NotFoundException("Food is not found");
            await _foodRepo.DeleteAsync(food);
        }

        public async Task<Food> GetFoodAsync(int foodId)
        {
            return await _foodRepo.GetByIdAsync(foodId) ?? throw new NotFoundException("Food is not found");
        }

        public async Task<List<Food>> GetFoodsAsync()
        {
            return await _foodRepo.GetFoodsAsync();
        }

        public async Task UpdateFoodAsync(int foodId, FoodDto foodDto)
        {
            var food = await _foodRepo.GetByIdAsync(foodId) ?? throw new NotFoundException("Food is not found");

            if(! await _categoryRepo.AnyCategoryAsync(foodDto.CategoryId)) throw new NotFoundException("Category is not found");

            food.Name = foodDto.Name;
            food.Description = foodDto.Description;
            food.Price = foodDto.price;
            food.CategoryId = foodDto.CategoryId;

            if(foodDto.Photo is not null){
                using var  DataStream = new MemoryStream();
                await foodDto.Photo.CopyToAsync(DataStream);
                food.Photo = DataStream.ToArray();
            }

            await _foodRepo.UpdateAsync(food);
        }
    }
}