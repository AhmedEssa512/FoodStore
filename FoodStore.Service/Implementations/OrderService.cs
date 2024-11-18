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
          private readonly IOrderRepository _orderRepo;
          private readonly ICartRepository _cartRepo;

          private readonly IOrderDetailsRepository _orderDetailsRepo;

          public OrderService(IOrderRepository orderRepo,IOrderDetailsRepository orderDetailsRepo,ICartRepository cartRepo)
          {
            _orderRepo = orderRepo;
            _orderDetailsRepo = orderDetailsRepo;
            _cartRepo = cartRepo;
          }

        public async Task AddOrderAsync(string userId, OrderDto orderDto)
        {

            if(string.IsNullOrWhiteSpace(userId))  throw new ValidationException("User Id can not be empty.");

            if (string.IsNullOrWhiteSpace(orderDto.Address))
                throw new ValidationException("Address cannot be empty.");

            if (string.IsNullOrWhiteSpace(orderDto.Phone))
                throw new ValidationException("Phone cannot be empty.");

            var cart = await _cartRepo.GetCartWithCartItemsAsync(userId);

            if(cart is null || cart.Items.Count == 0) throw new NotFoundException("You should add item at least to the cart to can make an order");

            if(cart.UserId != userId) throw new ForbiddenException("You do not have permission to create an order");

            var order = new Order{
                Address = orderDto.Address,
                Phone = orderDto.Phone,
                Total = cart.Total,
                UserId = userId,
                OrderDetails = new List<OrderDetail>()
            };
            

            await _orderRepo.AddAsync(order);
            
            foreach (var cartItem in cart.Items)
            {
                var orderDetail = new OrderDetail{
                    orderId = order.Id,
                    Quantity = cartItem.Quantity,
                    UnitPrice = cartItem.Price,
                    FoodId = cartItem.FoodId,    
                };

                order.OrderDetails.Add(orderDetail);
            }
           await _cartRepo.DeleteAsync(cart);
           await _orderRepo.SaveChangesAsync();
        }

        public async Task DeleteOrderItemAsync(string userId, int orderItemId)
        {
            var orderItem = await _orderDetailsRepo.GetOrderDetailsWithOrder(orderItemId);

            if(orderItem is null) throw new NotFoundException("Order item not found");

            if(orderItem.Order.UserId != userId) throw new ForbiddenException("You dont have permsiion");

            await _orderDetailsRepo.DeleteAsync(orderItem);

            orderItem.Order.Total = orderItem.Order.OrderDetails.Sum(o => o.UnitPrice * o.Quantity);

            await _orderRepo.SaveChangesAsync();
        }

        public async Task DeleteOrderAsync(string userId, int orderId)
        {
            var order = await _orderRepo.GetOrderWithOrderDetailsAsync(orderId) ?? throw new NotFoundException("Order not found");
            if (order.UserId != userId) throw new ForbiddenException("You do not have permission to do this operation");

            await _orderRepo.DeleteAsync(order);
           
        }

        public async Task UpdateOrderAsync(string userId,int orderItemId,int quantity)
        {
            if(quantity <= 0) throw new ValidationException("Invalid quantity");

            var orderItem = await _orderDetailsRepo.GetOrderDetailsWithOrder(orderItemId);

            if(orderItem is null) throw new NotFoundException("Order not found");

            if(orderItem.Order.UserId != userId) throw new ForbiddenException("do not have permession");

            orderItem.Quantity = quantity;

            await _orderDetailsRepo.UpdateAsync(orderItem);

            orderItem.Order.Total = orderItem.Order.OrderDetails.Sum(o => o.UnitPrice * o.Quantity);

            await _orderRepo.SaveChangesAsync();
        }

        public async Task<List<Order>> GetOrdersAsync(string UserId)
        {
            return await _orderRepo.GetOrdersAsync(UserId);
        }
    }
}