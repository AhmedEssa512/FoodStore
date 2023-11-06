using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodStore.Service.Authorization;
using FoodStore.Service.GenericRepository;
using FoodStore.Service.IRepos;
using FoodStore.Service.Repos;
using Microsoft.Extensions.DependencyInjection;
using FoodStore.Service.Authentication;

namespace FoodStore.Service
{
    public static class ServiceDependencies
    {
         public static IServiceCollection AddServiceDependencies(this IServiceCollection services, Microsoft.Extensions.Configuration.ConfigurationManager configuration)
        {
             services.AddTransient(typeof(IGenericBase<>), typeof(GenericBase<>));
             services.AddTransient<ICategoryRepo,CategoryRepo>();
             services.AddTransient<IFoodRepo,FoodRepo>();
             services.AddTransient<IShoppingCartRepo,ShoppingCartRepo>();
             services.AddTransient<IAuthService,AuthService>();
             services.AddTransient<IOrderRepo,OrderRepo>();
             services.AddTransient<IOrderDetailsRepo,OrderDetailsRepo>();
             services.AddTransient<IAuthorizationService,AuthorizationService>();
             
            return services;
        }
    }
}