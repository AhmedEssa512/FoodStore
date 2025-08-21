using Microsoft.EntityFrameworkCore;

namespace FoodStore.Data.Extensions
{
    public static class IQueryableExtensions
    {
         public static async Task<(IReadOnlyList<T> Items, int TotalCount)> 
        ToPaginatedListAsync<T>(this IQueryable<T> query, int pageNumber, int pageSize) where T : class
        {
            int totalCount = await query.CountAsync();

            var items = await query
                .AsNoTracking()
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items.AsReadOnly(), totalCount);
        }
    }
}