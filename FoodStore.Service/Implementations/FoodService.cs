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
using Microsoft.AspNetCore.Http;

namespace FoodStore.Service.Implementations
{
    public class FoodService : IFoodService
    {

        private readonly IUnitOfWork _unitOfWork;
        public FoodService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Food> CreateFoodAsync(FoodDto foodDto)
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

                string imageFilePath = null;
                if (foodDto.Photo != null && foodDto.Photo.Length > 0)
                {
                    imageFilePath = await SaveImageAsync(foodDto.Photo); 
                }
     
               var food = new Food{
                   Name = foodDto.Name,
                   Description = foodDto.Description,
                   Price = foodDto.Price,
                   CategoryId = foodDto.CategoryId,
                   ImageUrl = imageFilePath
               };

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

        public async Task<string> SaveImageAsync(IFormFile image)
        {

            // Generate a unique file name for the image
            string imageFileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);

            // Define the path to save the image (wwwroot/images folder)
            var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", imageFileName);

            // Ensure the directory exists
            var imageDirectory = Path.GetDirectoryName(imagePath);
            if (!Directory.Exists(imageDirectory))
            {
                Directory.CreateDirectory(imageDirectory);
            }

            // Save the image asynchronously to the file system
            using (var fileStream = new FileStream(imagePath, FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }

            // Return the relative image file path 
            return $"images/{imageFileName}";
        }

        public void DeleteImageAsync(string imagePath)
        {
           if(! string.IsNullOrWhiteSpace(imagePath))
           {
                // Combine the relative path with the root directory of the application (wwwroot)
                string fullPath =  Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", imagePath);

                // Check if the file exists
                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);                  
                }
                else
                {
                    // If the file doesn't exist, you can log it or throw an exception if needed
                    throw new NotFoundException($"File not found: {imagePath}");
                }
           }
        
        }


        public async Task DeleteFoodAsync(int foodId)
        {
            var food = await _unitOfWork.Food.GetByIdAsync(foodId) ?? throw new NotFoundException("Food is not found");

             await _unitOfWork.BeginTransactionAsync();

            try
            {
                // Delete the associated image if it exists
                if (!string.IsNullOrWhiteSpace(food.ImageUrl))
                {
                    DeleteImageAsync(food.ImageUrl); 
                }

                // Delete the food item from the database
                await _unitOfWork.Food.DeleteAsync(food);
                await _unitOfWork.SaveChangesAsync(); 

                // Commit the transaction if everything succeeds
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

        public async Task<IEnumerable<Food>> GetFoodsAsync(PaginationParams paginationParams)
        {
            return await _unitOfWork.Food.GetPaginatedFoods(paginationParams);
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
                string newImageUrl = await SaveImageAsync(foodDto.Photo);

                food.ImageUrl = newImageUrl;

                // Deleting old image from file sytsem
                DeleteImageAsync(oldImageUrl);
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