using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodStore.Contracts.Common
{
    public class OperationResult
    {
        public bool Success { get; set; }
        public List<string> Errors { get; set; } = new();
    }
}