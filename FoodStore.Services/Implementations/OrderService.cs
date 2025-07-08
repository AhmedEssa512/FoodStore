using FoodStore.Data.Entities;
using FoodStore.Services.Exceptions;
using AutoMapper;
using FoodStore.Data.Repositories.Interfaces;
using FoodStore.Data.Entities.Enums;
using FoodStore.Contracts.DTOs.Order;
using FoodStore.Contracts.Interfaces;

namespace FoodStore.Services.Implementations
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

          public OrderService(IUnitOfWork unitOfWork , IMapper mapper)
          {
             _unitOfWork = unitOfWork;
             _mapper = mapper;
          }

        public async Task CreateOrderAsync(string userId, OrderDto orderDto)
        {

            await _unitOfWork.BeginTransactionAsync();
           try
           {
                var cart = await _unitOfWork.Cart.GetCartWithCartItemsAsync(userId);

                if(cart is null || cart.Items.Count is 0) 
                {
                    throw new NotFoundException("You should add item at least to the cart to can make an order");
                }
                
                if(cart.UserId != userId) 
                {
                    throw new ForbiddenException("You do not have permission to create an order");
                }
                
                var order = _mapper.Map<Order>(orderDto);

                order.UserId = userId;
                order.Total = cart.Total;

                 await _unitOfWork.Order.AddAsync(order);

                var orderDetails = cart.Items.Select(cartItem => new OrderDetail
                {
                    OrderId = order.Id, 
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


        public async Task DeleteOrderAsync(string userId, int orderId)
        {

            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var order = await _unitOfWork.Order.GetByIdAsync(orderId) ?? 
                   throw new NotFoundException("Order not found");

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

        public async Task UpdateOrderAsync(string userId, int orderId, OrderDto orderDto)
        {

            await _unitOfWork.BeginTransactionAsync();

            try
            {
                var order = await _unitOfWork.Order.GetByIdAsync(orderId) ??
                    throw new NotFoundException("Order not found");
                
                if (order.UserId != userId) 
                {
                    throw new ForbiddenException("do not have permession");
                }

                if (order.Status != OrderStatus.Pending)
                {
                    throw new ForbiddenException("You cannot update the order address or phone because the order has already been processed.");
                }

                 order.Address = orderDto.Address;
                 order.Phone = orderDto.Phone;

                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }

            
        }

        public async Task<IReadOnlyList<OrderListItemDto>> GetUserOrdersAsync(string UserId)
        {
            var orders = await _unitOfWork.Order.GetOrderSummariesAsync(UserId);

            return orders.Select(o => new OrderListItemDto
                {
                    Id = o.Id,
                    FullName = o.FullName,
                    Status = o.Status.ToString(),
                    Total = o.Total,
                    CreatedAt = o.CreatedAt.ToString("yyyy-MM-dd HH:mm")
                }).ToList();
        }

        public async Task<OrderResponseDto> GetOrderByIdAsync(int orderId)
        {
            var order = await _unitOfWork.Order.GetOrderWithDetailsAsync(orderId) ??
                throw new NotFoundException("Order not found");

             return new OrderResponseDto
            {
                Id = order.Id,
                FullName = order.FullName,
                Address = order.Address,
                Phone = order.Phone,
                CreatedAt = order.CreatedAt,
                Status = order.Status,
                Total = order.Total,
                OrderDetails = order.OrderDetails.Select(od => new OrderDetailsDto
                {
                    FoodName = od.FoodName,
                    Price = od.Price,
                    Quantity = od.Quantity,
                }).ToList()
            };
        }

        public async Task<OrderResponseDto> UpdateOrderStatusAsync(int orderId, string newStatus)
        {
            var order = await _unitOfWork.Order.GetByIdAsync(orderId) ?? 
                throw new NotFoundException("Not found order");

            if (!Enum.TryParse<OrderStatus>(newStatus, true, out var status))
                throw new BadRequestException($"Invalid order status: {newStatus}");

            if(!IsValidStatusTransition(order.Status, status))
                throw new ConflictException($"Cannot change status from {order.Status} to {newStatus}");
            
            order.Status = status;
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<OrderResponseDto>(order);

        }

        private bool IsValidStatusTransition(OrderStatus current, OrderStatus next)
        {
            return (current, next) switch
            {
                // Standard forward progression
                (OrderStatus.Pending, OrderStatus.Preparing) => true,
                (OrderStatus.Preparing, OrderStatus.Delivered) => true,

                // Allow cancel from early stages
                (OrderStatus.Pending, OrderStatus.Canceled) => true,
                (OrderStatus.Preparing, OrderStatus.Canceled) => true,

                // Allow failure from any non-final state
                (OrderStatus.Pending, OrderStatus.Failed) => true,
                (OrderStatus.Preparing, OrderStatus.Failed) => true,

                _ => false
            };
        }

        public bool IsValidOrderStatus(string statusString)
        {
            return Enum.TryParse<OrderStatus>(statusString, ignoreCase: true, out _);
        }

    }
}