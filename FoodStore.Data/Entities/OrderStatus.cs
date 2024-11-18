using System.Runtime.Serialization;

namespace FoodStore.Data.Entities
{
    public enum OrderStatus
    {
        [EnumMember(Value = "Pending")]
        Pending,
        [EnumMember(Value = "Shipped")]
        Shipped,
        [EnumMember(Value = "Delivered")]
        Delivered,
        [EnumMember(Value = "Canceled")]
        Canceled,
        [EnumMember(Value = "Failed")]
        Failed,
    }

}