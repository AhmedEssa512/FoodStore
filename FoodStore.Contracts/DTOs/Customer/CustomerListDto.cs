
namespace FoodStore.Contracts.DTOs.Customer
{
    public class CustomerListDto
    {
        public string Id { get; set; } = default!;
        public string UserName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string PhoneNumber { get; set; } = default!;

        public bool IsActive { get; set; } = default!;
    }
}