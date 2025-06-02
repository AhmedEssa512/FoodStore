using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodStore.Contracts.DTOs.Category
{
    public class CategoryResponseDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
    }
}