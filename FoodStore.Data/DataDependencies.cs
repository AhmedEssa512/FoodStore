using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodStore.Data.Repositories.Implementations;
using FoodStore.Data.Repositories.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace FoodStore.Data
{
    public static class DataDependencies
    {
        public static IServiceCollection AddDataDependencies(this IServiceCollection services)
        {
        
            services.AddTransient(typeof(IGenericBase<>), typeof(GenericBase<>));

            services.AddScoped<IFoodRepository, FoodRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IOrderDetailsRepository, OrderDetailsRepository>();
            services.AddScoped<ICartRepository, CartRepository>();
            services.AddScoped<ICartItemRepository, CartItemRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IReportRepository, ReportRepository>();


            services.AddScoped<IUnitOfWork, UnitOfWork>();


             
            return services;
        }
    }
}