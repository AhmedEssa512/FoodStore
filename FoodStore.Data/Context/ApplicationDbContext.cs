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
        public DbSet<Category> categories  { get; set; } = null!;
        public DbSet<Food> foods  { get; set; } = null!;
        public DbSet<Order> orders  { get; set; } = null!;
        public DbSet<OrderDetail> orderDetails  { get; set; } = null!;
        public DbSet<Cart> carts  { get; set; } = null!;
        public DbSet<CartItem> cartItems  { get; set; } = null!;



 

    }
}