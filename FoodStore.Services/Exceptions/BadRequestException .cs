using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodStore.Services.Exceptions
{
    public class BadRequestException(string message) : Exception(message)
    {
    }
}