using System.ComponentModel.DataAnnotations;
using FoodStore.Data.Entities.Enums;

namespace FoodStore.Services.Validators
{
    public class ValidOrderStatusAttribute : ValidationAttribute
    {
        public override bool IsValid(object ?value)
        {
            if (value is string str)
            {
                return Enum.TryParse(typeof(OrderStatus), str, true, out _); 
            }
            return false; 
        }
    }
}