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

        private readonly IUnitOfWork _unitOfWork;
        public FoodService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task AddFoodAsync(FoodDto foodDto)
        {
            if (string.IsNullOrWhiteSpace(foodDto.Name))
                  throw new ValidationException("Name cannot be empty.");

            if (string.IsNullOrWhiteSpace(foodDto.Description))
                  throw new ValidationException("Description cannot be empty.");

            await _unitOfWork.BeginTransactionAsync();

            try
            {
                if(! await _unitOfWork.Category.AnyCategoryAsync(foodDto.CategoryId) ) 
                 throw new NotFoundException("Category is not found");
     
               var food = new Food{
                   Name = foodDto.Name,
                   Description = foodDto.Description,
                   Price = foodDto.Price,
                   CategoryId = foodDto.CategoryId,
               };

               if(foodDto.Photo is not null)
                {
                 using var  DataStream = new MemoryStream();
                 await foodDto.Photo.CopyToAsync(DataStream);
                 food.Photo = DataStream.ToArray();
                }

                await _unitOfWork.Food.AddAsync(food);

                await _unitOfWork.SaveChangesAsync();

                await _unitOfWork.CommitTransactionAsync();
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
            await _unitOfWork.Food.DeleteAsync(food);
        }

        public async Task<Food> GetFoodAsync(int foodId)
        {
            return await _unitOfWork.Food.GetByIdAsync(foodId) ?? throw new NotFoundException("Food is not found");
        }

        public async Task<List<Food>> GetFoodsAsync()
        {
            return await _unitOfWork.Food.GetFoodsAsync();
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

                if(foodDto.Photo is not null){
                    using var  DataStream = new MemoryStream();
                    await foodDto.Photo.CopyToAsync(DataStream);
                    food.Photo = DataStream.ToArray();
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
    }
}