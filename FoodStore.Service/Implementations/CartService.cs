using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodStore.Data.DTOS;
using FoodStore.Data.Entities;
using FoodStore.Service.Abstracts;
using FoodStore.Service.Context;
using FoodStore.Service.Exceptions;
using FoodStore.Service.GenericRepository;
using FoodStore.Service.IRepository;
using Microsoft.EntityFrameworkCore;

namespace FoodStore.Service.Implementations
{
    public class CartService : ICartService
    {
     
         private readonly ICartRepository _cartRepository;
         private readonly ICartItemRepository _cartItemRepository;
         private readonly IFoodRepository _foodRepository;
         
         public CartService(ICartRepository cartRepository,IFoodRepository foodRepository,ICartItemRepository cartItemRepository)
         {
            _cartRepository = cartRepository;
            _foodRepository = foodRepository;
            _cartItemRepository = cartItemRepository;
         }

        public async Task AddToCartAsync(string userId,CartItemDto cartItemDto)
        {
            
                if(string.IsNullOrWhiteSpace(userId))  throw new ValidationException("User Id can not be empty.");

                var foodExists = await _foodRepository.AnyFoodAsync(cartItemDto.FoodId);

                if (!foodExists) throw new NotFoundException($"Food item does not exist.");
                  
               if(cartItemDto.quantity <= 0) throw new ValidationException("Invalid quantity number");

                 // Retrieve the cart or create a new one if it doesn't exist
                var cart = await _cartRepository.GetCartWithCartItemsAsync(userId) ?? new Cart{UserId = userId, Items = new List<CartItem>()};

                 
                if(cart.Id == 0) await _cartRepository.AddAsync(cart);
                   
                double foodPrice = await _foodRepository.GetPriceAsync(cartItemDto.FoodId);

               if(foodPrice < 0) throw new ValidationException("Invalid price. Price must be greater than 0");

                 // Check if the item already exists in the cart
                var existingItem = cart.Items.FirstOrDefault(f => f.FoodId == cartItemDto.FoodId);

                if(existingItem is null)
                {
                    
                    
                    var newItem = new CartItem {
                        FoodId = cartItemDto.FoodId,
                        Quantity = cartItemDto.quantity,
                        Price = foodPrice,
                        CartId = cart.Id
                        };

                    cart.Items.Add(newItem);
                    cart.Total += foodPrice * cartItemDto.quantity;
               }
                else
                {
                    existingItem.Quantity += cartItemDto.quantity;
                }

                cart.Total = cart.Items.Sum(i => i.Price * i.Quantity);

                await _cartRepository.SaveChangesAsync();
              
    } 

        public async Task<Cart> GetCartItemsAsync(string userId)
        {
            if(string.IsNullOrEmpty(userId))  throw new ValidationException("User Id can not be null.");

            var cart = await _cartRepository.GetCartWithCartItemsAsync(userId);

            return cart; 
        }

        public async Task DeleteCartItemAsync(string userId,int cartItemId)
        {
            if(cartItemId <= 0) throw new ValidationException("Invalid data. CartItemId must be greater than 0");

             // Retrieve the cartItem 
             var cartItem =  await _cartItemRepository.GetCartItemWithCartAsync(cartItemId);
           
            if(cartItem == null) throw new Exception("Cart item not found");

             // Verify ownership of the cart
            if(cartItem.Cart.UserId != userId ) throw new ForbiddenException("You do not have permission to update this cart item");

            
            await _cartItemRepository.DeleteAsync(cartItem);

            cartItem.Cart.Total = cartItem.Cart.Items.Sum(i => i.Price * i.Quantity);

            await _cartRepository.SaveChangesAsync();

        }

        public async Task DeleteCartItemsAsync(string userId, int cartId)
        {
            //get cart Included CartItems
            var cart = await _cartRepository.GetCartWithCartItemsAsync(userId);

            if(cart is null) throw new NotFoundException("Cart not found.");

            if(cart.Items.Count == 0) throw new InvalidOperationException("Cart is already empty.");

            await _cartItemRepository.DeleteRangeAsync(cart.Items);

            cart.Total = cart.Items.Sum(i => i.Price * i.Quantity);

            await _cartItemRepository.SaveChangesAsync();
           
        }

        public async Task UpdateCartItemAsync(string userId,int cartItemId, int newQuantity)
        {
              // Retrieve the cart item with its associated cart
            var cartItem = await _cartItemRepository.GetCartItemWithCartAsync(cartItemId);

            if(cartItem == null) throw new NotFoundException("Cart item not found");

             // Verify ownership of the cart
            if(cartItem.Cart.UserId != userId ) throw new ForbiddenException("You do not have permission to update this cart item");

            if(newQuantity <= 0) throw new ValidationException("Invalid quantity number");
            
            double foodPrice = await _foodRepository.GetPriceAsync(cartItem.FoodId);

            if(foodPrice < 0) throw new ValidationException("Invalid data. Price must be greater than 0");

             // Update the quantity and recalculate the price
            cartItem.Quantity = newQuantity;
            cartItem.Price = newQuantity * foodPrice;

             // Update the total of the cart
            cartItem.Cart.Total = cartItem.Cart.Items.Sum(i => i.Price * i.Quantity);

            await _cartRepository.SaveChangesAsync();

        }

       
 
    }
}