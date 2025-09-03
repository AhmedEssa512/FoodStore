using System.Runtime.Serialization;

namespace FoodStore.Data.Entities.Enums
{
    public enum OrderStatus
    {
        Pending,
        Preparing,
        OutForDelivery,
        Delivered,
        Canceled,
    }

}