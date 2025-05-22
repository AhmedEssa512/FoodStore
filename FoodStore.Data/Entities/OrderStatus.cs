using System.Runtime.Serialization;

namespace FoodStore.Data.Entities
{
    public enum OrderStatus
    {

        Pending,

        Preparing,

        Delivered,
      
        Canceled,

        Failed,
    }

}