using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;

namespace FoodStore.Data.Repositories.Interfaces
{
    public interface IGenericBase<T> where T : class
    {

    Task<T?> GetByIdAsync(int id);
    Task<T> AddAsync(T entity);
    Task AddRangeAsync(IEnumerable<T> entities);

   
    void Update(T entity);
    void UpdateRange(IEnumerable<T> entities);
    void Delete(T entity);
    void DeleteRange(IEnumerable<T> entities);


    }
}