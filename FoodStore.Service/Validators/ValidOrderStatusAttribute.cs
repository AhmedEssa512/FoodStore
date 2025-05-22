using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using FoodStore.Data.Entities;

namespace FoodStore.Service.Validators
{
    public class ValidOrderStatusAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value is string str)
            {
                return Enum.TryParse(typeof(OrderStatus), str, true, out _); 
            }
            return false; 
        }
    }
}