using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodStore.Contracts.DTOs.Order
{
    public class UpdateOrderStatusRequest
    {
        public string NewStatus { get; set; } = string.Empty;
    }
}