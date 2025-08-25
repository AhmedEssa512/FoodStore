

namespace FoodStore.Contracts.DTOs.Customer
{
    public class UpdateUserStatusDto
    {
        /// <summary>
        /// If true → active user.
        /// If false → blocked or suspended.
        /// </summary>
        public bool IsActive { get; set; }
        /// <summary>
        /// Optional suspension end date. 
        /// If set, user is suspended until this date.
        /// If null and IsActive = false → permanent ban.
        /// </summary>
        public DateTimeOffset? SuspensionEnd  { get; set; }
    }
}