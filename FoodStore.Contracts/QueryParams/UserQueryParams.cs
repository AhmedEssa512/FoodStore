using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodStore.Contracts.QueryParams
{
    public class UserQueryParams
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        // Searching
        public string? Search { get; set; } 

        // Filtering
        public bool? IsActive { get; set; } 

        // Sorting
        public string? SortBy { get; set; } = "UserName"; 
        public bool Descending { get; set; } = false; 

    }
}