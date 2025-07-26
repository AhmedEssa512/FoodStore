
namespace FoodStore.Contracts.Common
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }

        public static ApiResponse<T> Ok(T data, string? message = null) =>
        new() { Success = true, Data = data, Message = message };

        public static ApiResponse<T> Fail(string message) =>
        new() { Success = false, Message = message };

        public static ApiResponse<T> FailWithErrors(T data, string? message) =>
        new() { Success = false, Data = data, Message = message };

    }
}