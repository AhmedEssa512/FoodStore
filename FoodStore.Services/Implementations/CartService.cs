using FoodStore.Contracts.DTOs.Cart;
using FoodStore.Data.Entities;
using FoodStore.Services.Exceptions;
using FoodStore.Data.Repositories.Interfaces;
using FoodStore.Contracts.Interfaces;

namespace FoodStore.Services.Implementations
{
    public class CartService : ICartService
    {
     
        private readonly IUnitOfWork _unitOfWork;
        private readonly IImageService _imageService;

         public CartService(IUnitOfWork unitOfWork, IImageService imageService)
         {
            _unitOfWork = unitOfWork;
            _imageService = imageService;
         }

        public async Task AddToCartAsync(string userId,CartItemDto cartItemDto)
        {


            var food = await _unitOfWork.Food.GetByIdAsync(cartItemDto.FoodId);

            if (food is null)
                throw new NotFoundException("Food item does not exist.");

            if (!food.IsAvailable)
                throw new ValidationException("This food item is not available.");

            if (food.Price <= 0)
                throw new ValidationException("Invalid price. Price must be greater than 0");

                await _unitOfWork.BeginTransactionAsync();

          try
           {

                var cart = await _unitOfWork.Cart.GetCartWithCartItemsAsync(userId);
                
                if(cart is null)
                {
                    cart = new Cart { UserId = userId };
                    await _unitOfWork.Cart.AddAsync(cart);
                    await _unitOfWork.SaveChangesAsync();
                }
                 
                 // Check if the item already exists in the cart
                var existingItem = cart.Items.FirstOrDefault(f => f.FoodId == cartItemDto.FoodId);

                if(existingItem is null)
                { 
                    cart.Items.Add(new CartItem
                    {
                        FoodId = cartItemDto.FoodId,
                        Quantity = cartItemDto.Quantity,
                        Price = food.Price,
                        CartId = cart.Id
                    });
                }
                else
                {
                    existingItem.Quantity += cartItemDto.Quantity;
                }

                cart.Total = cart.Items.Sum(i => i.Price * i.Quantity);

               await _unitOfWork.SaveChangesAsync();

               await _unitOfWork.CommitTransactionAsync();
            }
            catch(Exception)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
              
    } 

        public async Task<CartResponseDto> GetUserCartAsync(string userId)
        {
            var cart = await _unitOfWork.Cart.GetCartWithCartItemsAsync(userId);

            if(cart is null)
             return new CartResponseDto{
                UserId = userId,
                Items = [],
                Total = 0,
             };

            var cartResponse = new CartResponseDto
            {
                Id = cart.Id,
                UserId = cart.UserId,
                Total = cart.Total,
                Items = cart.Items.Select(item => new CartItemResponseDto
                {
                    Id = item.Id,
                    Quantity = item.Quantity,
                    Food = new FoodInCartResponseDto
                    {
                        Id = item.Food!.Id,
                        Name = item.Food.Name,
                        Price = item.Food.Price,
                        ImageUrl = _imageService.GetFullUrl(item.Food.ImageUrl)
                    }
                }).ToList()
            };


            return cartResponse; 
        }

        public async Task DeleteCartItemAsync(string userId,int cartItemId)
        {
            
                await _unitOfWork.BeginTransactionAsync();
                
            try{
                var cartItem = await _unitOfWork.CartItem.GetCartItemWithCartAsync(cartItemId) ?? throw new NotFoundException("Cart item not found");

                // Verify ownership of the cart
                if (cartItem.Cart == null || cartItem.Cart.UserId != userId ) throw new ForbiddenException("You do not have permission to update this cart item");

                await _unitOfWork.CartItem.DeleteAsync(cartItem);

                cartItem.Cart.Total -= cartItem.Price * cartItem.Quantity;


                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();
            }
            catch(Exception)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }

        }

        public async Task DeleteCartItemsAsync(string userId)
        {
            //get cart Included CartItems
            var cart = await _unitOfWork.Cart.GetCartWithCartItemsAsync(userId) ?? 
                throw new NotFoundException("Cart not found.");

            if (cart.Items.Count == 0) throw new InvalidOperationException("Cart is already empty.");

            await _unitOfWork.CartItem.DeleteRangeAsync(cart.Items);

            cart.Total = cart.Items.Sum(i => i.Price * i.Quantity);

            await _unitOfWork.SaveChangesAsync();
           
        }

        public async Task UpdateCartItemAsync(string userId,int cartItemId, int newQuantity)
        {
                await _unitOfWork.BeginTransactionAsync();

            try{
                
                var cartItem = await _unitOfWork.CartItem.GetCartItemWithCartAsync(cartItemId) ?? 
                    throw new NotFoundException("Cart item not found");

                // Verify ownership of the cart
                if (cartItem.Cart == null || cartItem.Cart.UserId != userId ) throw new ForbiddenException("You do not have permission to update this cart item");
                
                cartItem.Quantity = newQuantity;

                cartItem.Cart.Total = cartItem.Cart.Items.Sum(i => i.Price * i.Quantity);

                await _unitOfWork.SaveChangesAsync();

                await _unitOfWork.CommitTransactionAsync();
            }
            catch(Exception)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }

        }

        public async Task MergeCartAsync(string userId, List<CartItemDto> guestItems)
        {

            if (guestItems == null || guestItems.Count == 0)
                return; // Nothing to merge

            await _unitOfWork.BeginTransactionAsync();

            try
            {
                // Retrieve user's existing cart or create new one
                var cart = await _unitOfWork.Cart.GetCartWithCartItemsAsync(userId); 
                       
                if(cart is null)
                {
                    cart = new Cart { UserId = userId };
                    await _unitOfWork.Cart.AddAsync(cart);
                    await _unitOfWork.SaveChangesAsync();
                }

                foreach (var guestItem in guestItems)
                {
                    if (guestItem.Quantity <= 0)
                        continue; 

                    var foodExists = await _unitOfWork.Food.AnyFoodAsync(guestItem.FoodId);
                    if (!foodExists)
                        continue; 

                    var price = await _unitOfWork.Food.GetPriceAsync(guestItem.FoodId);
                    if (price <= 0)
                        continue;


                    var existingItem = cart.Items.FirstOrDefault(i => i.FoodId == guestItem.FoodId);
                    if (existingItem != null)
                    {
                        existingItem.Quantity += guestItem.Quantity;
                    }
                    else
                    {

                        cart.Items.Add(new CartItem
                        {
                            FoodId = guestItem.FoodId,
                            Quantity = guestItem.Quantity,
                            Price = price,
                            CartId = cart.Id
                        });
                    }
                }


                cart.Total = cart.Items.Sum(i => i.Price * i.Quantity);

                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }


       
 
    }
}