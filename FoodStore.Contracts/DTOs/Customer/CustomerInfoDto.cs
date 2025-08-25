

namespace FoodStore.Contracts.DTOs.Customer
{
    public class CustomerInfoDto
    {
        public string Id { get; set; } = default!;            
        public string UserName { get; set; } = default!;
        public string Email { get; set; } = default!;    
        public string PhoneNumber { get; set; } = default!; 
        public bool PhoneNumberConfirmed { get; set; } 
        public bool EmailConfirmed { get; set; } 
        public bool LockoutEnabled { get; set; }
        public DateTimeOffset? LockoutEnd { get; set; }
        public bool IsActive { get; set; }
        public IList<string> Roles { get; set; }  = default!;
    }
}