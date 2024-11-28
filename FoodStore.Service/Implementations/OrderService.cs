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
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;

          public OrderService(IUnitOfWork unitOfWork)
          {
             _unitOfWork = unitOfWork;
          }

        public async Task AddOrderAsync(string userId, OrderDto orderDto)
        {


            if(string.IsNullOrWhiteSpace(userId))  throw new ValidationException("User Id can not be empty.");

            if (string.IsNullOrWhiteSpace(orderDto.Address))
                throw new ValidationException("Address cannot be empty.");

            if (string.IsNullOrWhiteSpace(orderDto.Phone))
                throw new ValidationException("Phone cannot be empty.");

            await _unitOfWork.BeginTransactionAsync();
           try
           {
         
                var cart = await _unitOfWork.Cart.GetCartWithCartItemsAsync(userId);

                if(cart is null || cart.Items.Count == 0) throw new NotFoundException("You should add item at least to the cart to can make an order");

                if(cart.UserId != userId) throw new ForbiddenException("You do not have permission to create an order");

                var order = new Order{
                    Address = orderDto.Address,
                    Phone = orderDto.Phone,
                    Total = cart.Total,
                    UserId = userId,
                    OrderDetails = []
                };
                

                await _unitOfWork.Order.AddAsync(order);
                
                var orderDetails = cart.Items.Select(cartItem => new OrderDetail
                {
                    orderId = order.Id, 
                    Quantity = cartItem.Quantity,
                    UnitPrice = cartItem.Price,
                    FoodId = cartItem.FoodId
                }).ToList();

                await _unitOfWork.OrderDetails.AddRangeAsync(orderDetails);

                await _unitOfWork.Cart.DeleteAsync(cart);

                await _unitOfWork.SaveChangesAsync();

                await _unitOfWork.CommitTransactionAsync();
         
           }
           catch(Exception)
           {
               await _unitOfWork.RollbackTransactionAsync();
               throw;
           }

        }

        public async Task DeleteOrderItemAsync(string userId, int orderItemId)
        {
            if(string.IsNullOrWhiteSpace(userId))  throw new ValidationException("User Id can not be empty.");

            await _unitOfWork.BeginTransactionAsync();

            try
            {
                var orderItem = await _unitOfWork.OrderDetails.GetOrderDetailsWithOrder(orderItemId) ?? throw new NotFoundException("Order item not found");
                
                if (orderItem.Order.UserId != userId) throw new ForbiddenException("You dont have permsiion");

                await _unitOfWork.OrderDetails.DeleteAsync(orderItem);

                orderItem.Order.Total -= orderItem.UnitPrice * orderItem.Quantity;

                await _unitOfWork.SaveChangesAsync();

                await _unitOfWork.CommitTransactionAsync();
            }
            catch(Exception)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task DeleteOrderAsync(string userId, int orderId)
        {
            if(string.IsNullOrWhiteSpace(userId))  throw new ValidationException("User Id can not be empty.");

            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var order = await _unitOfWork.Order.GetOrderWithOrderDetailsAsync(orderId) ?? throw new NotFoundException("Order not found");
                if (order.UserId != userId) throw new ForbiddenException("You do not have permission to do this operation");

                await _unitOfWork.Order.DeleteAsync(order);

                await _unitOfWork.SaveChangesAsync();

                await _unitOfWork.CommitTransactionAsync();
            }
            catch(Exception)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
           
        }

        public async Task UpdateOrderAsync(string userId,int orderItemId,int quantity)
        {
            if(string.IsNullOrWhiteSpace(userId))  throw new ValidationException("User Id can not be empty.");

            if(quantity <= 0) throw new ValidationException("Invalid quantity");

            await _unitOfWork.BeginTransactionAsync();

            try
            {
                var orderItem = await _unitOfWork.OrderDetails.GetOrderDetailsWithOrder(orderItemId) ?? throw new NotFoundException("Order not found");
                
                if (orderItem.Order.UserId != userId) throw new ForbiddenException("do not have permession");

                orderItem.Quantity = quantity;

                await _unitOfWork.OrderDetails.UpdateAsync(orderItem);

                orderItem.Order.Total = orderItem.Order.OrderDetails.Sum(o => o.UnitPrice * o.Quantity);

                await _unitOfWork.SaveChangesAsync();

                await _unitOfWork.CommitTransactionAsync();
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }

            
        }

        public async Task<List<Order>> GetOrdersAsync(string UserId)
        {
            return await _unitOfWork.Order.GetOrdersAsync(UserId);
        }
    }
}