using FoodStore.Data.Context;
using Microsoft.EntityFrameworkCore.Storage;
using FoodStore.Data.Repositories.Interfaces;
using FoodStore.Data.Extensions;

namespace FoodStore.Data.Repositories.Implementations
{
    public class GenericBase<T> : IGenericBase<T> where T : class
    {

        private readonly ApplicationDbContext _context;

        public GenericBase(ApplicationDbContext context)
        {
            _context = context;
        }

    public async Task<T> AddAsync(T entity)
    {
        await _context.Set<T>().AddAsync(entity);
        return entity;
    }


    public async Task AddRangeAsync(IEnumerable<T> entities) =>
        await _context.Set<T>().AddRangeAsync(entities);

    public void Update(T entity) =>
         _context.Set<T>().Update(entity);

    public void UpdateRange(IEnumerable<T> entities) =>
        _context.Set<T>().UpdateRange(entities);

    public void Delete(T entity) =>
        _context.Set<T>().Remove(entity);

    public void DeleteRange(IEnumerable<T> entities) =>
        _context.Set<T>().RemoveRange(entities);

    public async Task<T?> GetByIdAsync(int id) =>
        await _context.Set<T>().FindAsync(id);

    }
}