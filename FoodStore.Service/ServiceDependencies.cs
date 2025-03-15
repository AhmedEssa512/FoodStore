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
using FoodStore.Service.IRepository;
using FoodStore.Service.Repository;

namespace FoodStore.Service
{
    public static class ServiceDependencies
    {
         public static IServiceCollection AddServiceDependencies(this IServiceCollection services, Microsoft.Extensions.Configuration.ConfigurationManager configuration)
        {
             services.AddTransient(typeof(IGenericBase<>), typeof(GenericBase<>));

             services.AddScoped<ICategoryService,CategoryService>();
             services.AddScoped<IFoodService,FoodService>();
             services.AddScoped<IAuthService,AuthService>();
             services.AddScoped<IOrderService,OrderService>();
             services.AddScoped<ICartService,CartService>();
             services.AddScoped<IAuthorizationService,AuthorizationService>();
             services.AddScoped<IUnitOfWork,UnitOfWork>();




             
            return services;
        }
    }
}