using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodStore.Contracts.Interfaces
{
    public interface ICacheService
    {
        T? Get<T>(string key);
        void Set<T>(string key, T value, TimeSpan slidingExpiration, TimeSpan? absoluteExpiration = null);
        void Remove(string key);
        void RemoveByPrefix(string prefix);
    }
}