using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodStore.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace FoodStore.Data.Context
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
    {
        public DbSet<Category> categories  { get; set; } 
        public DbSet<Food> foods  { get; set; } 
        public DbSet<Order> orders  { get; set; } 
        public DbSet<OrderDetail> orderDetails  { get; set; } 
        public DbSet<Cart> carts  { get; set; } 
        public DbSet<CartItem> cartItems  { get; set; } 

        // public DbSet<RefreshToken> refreshTokens  { get; set; }


 

    }
}