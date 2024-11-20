using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodStore.Service.Exceptions
{
    public class OperationFailedException : Exception
    {
        public OperationFailedException(string message) : base(message)
        {
        }

        public OperationFailedException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}