using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodStore.Service.Authorization;
using FoodStore.Service.GenericRepository;
using FoodStore.Service.Abstracts;
using FoodStore.Service.Implementations;
using Microsoft.Extensions.DependencyInjection;
using FoodStore.Service.Authentication;

namespace FoodStore.Service
{
    public static class ServiceDependencies
    {
         public static IServiceCollection AddServiceDependencies(this IServiceCollection services, Microsoft.Extensions.Configuration.ConfigurationManager configuration)
        {
             services.AddTransient(typeof(IGenericBase<>), typeof(GenericBase<>));
             services.AddTransient<ICategoryService,CategoryService>();
             services.AddTransient<IFoodService,FoodService>();
             services.AddTransient<IShoppingCartService,ShoppingCartService>();
             services.AddTransient<IAuthService,AuthService>();
             services.AddTransient<IOrderService,OrderService>();
             services.AddTransient<IOrderDetailsService,OrderDetailsService>();
             services.AddTransient<IAuthorizationService,AuthorizationService>();
             
            return services;
        }
    }
}