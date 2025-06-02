using FoodStore.Contracts.DTOs.Cart;


namespace FoodStore.Contracts.Interfaces
{
    public interface ICartService
    {
        Task AddToCartAsync(string userId, CartItemDto cartItemDto);
        Task UpdateCartItemAsync(string userId,int cartItemId, int newQuantity);
        Task DeleteCartItemAsync(string userId,int cartItemId);
        Task DeleteCartItemsAsync(string userId);
        Task<CartResponseDto> GetUserCartAsync(string userId);
        Task MergeCartAsync(string userId, List<CartItemDto> guestItems);
        
    }
}