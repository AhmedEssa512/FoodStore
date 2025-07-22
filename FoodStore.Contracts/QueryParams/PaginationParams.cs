using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodStore.Contracts.QueryParams
{
    public class PaginationParams
    {
        private const int MaxPageSize = 50;
        private int _pageSize = 9;
        private int _pageNumber = 1;  

        public int PageNumber
        {
            get => _pageNumber;
            set => _pageNumber = (value <= 0) ? 1 : value;  
        }

        public int PageSize
        {
            get => _pageSize;
            set
            {
                if (value <= 0)
                    _pageSize = 9;  
                else
                    _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
            }
        }
    }
}