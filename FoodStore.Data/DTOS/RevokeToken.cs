using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodStore.Data.DTOS
{
    public class RevokeToken
    {
        public required string Token { get; set; }
    }
}