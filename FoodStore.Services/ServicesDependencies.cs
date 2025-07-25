using FoodStore.Services.Implementations;
using Microsoft.Extensions.DependencyInjection;
using FoodStore.Services.Implementations.Security;
using FoodStore.Data.Repositories.Implementations;
using FoodStore.Data.Repositories.Interfaces;
using FoodStore.Contracts.Interfaces;
using FoodStore.Contracts.Interfaces.Security;

namespace FoodStore.Services
{
    public static class ServicesDependencies
    {
         public static IServiceCollection AddServicesDependencies(this IServiceCollection services, Microsoft.Extensions.Configuration.ConfigurationManager configuration)
        {
             services.AddTransient(typeof(IGenericBase<>), typeof(GenericBase<>));
             services.AddTransient<IEmailService, EmailService>();

             services.AddScoped<ICategoryService,CategoryService>();
             services.AddScoped<IFoodService,FoodService>();
             services.AddScoped<IAuthService,AuthService>();
             services.AddScoped<IOrderService,OrderService>();
             services.AddScoped<ICartService,CartService>();
             services.AddScoped<IImageService,ImageService>();
             services.AddScoped<IAuthorizationService,AuthorizationService>();
             services.AddScoped<IUnitOfWork,UnitOfWork>();




             
            return services;
        }
    }
}