using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodStore.Contracts.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace FoodStore.Services.Implementations
{
    public class CacheService : ICacheService
    {
        private readonly IMemoryCache _memoryCache;
        private readonly HashSet<string> _keys = new();
        private readonly object _keysLock = new();

        public CacheService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public void Set<T>(string key, T value, TimeSpan slidingExpiration, TimeSpan? absoluteExpiration = null)
        {
            var options = new MemoryCacheEntryOptions()
            .SetSlidingExpiration(slidingExpiration);

            if (absoluteExpiration.HasValue)
                options.SetAbsoluteExpiration(absoluteExpiration.Value);

            _memoryCache.Set(key, value, options);

            lock (_keysLock)
            {
                _keys.Add(key);
            }
        }

        public T? Get<T>(string key)
        {
            if (_memoryCache.TryGetValue(key, out var value) && value is T typedValue)
            return typedValue;

            return default;
        }

        public void Remove(string key)
        {
            _memoryCache.Remove(key);
            lock (_keysLock)
            {
                _keys.Remove(key);
            }
        }

        public void RemoveByPrefix(string prefix)
        {
            lock (_keysLock)
            {
                var keysToRemove = _keys.Where(k => k.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                .ToList();
                foreach (var key in keysToRemove)
                {
                    _memoryCache.Remove(key);
                    _keys.Remove(key);
                }
            }
        }

    }
}