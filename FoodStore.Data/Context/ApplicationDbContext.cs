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


          // Override SaveChangesAsync
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.Entity is Food foodEntity)
                {
                    if (entry.State == EntityState.Added)
                    {
                        foodEntity.CreatedAt = DateTime.UtcNow;
                        foodEntity.UpdatedAt = DateTime.UtcNow;
                    }
                    else if (entry.State == EntityState.Modified)
                    {
                        foodEntity.UpdatedAt = DateTime.UtcNow;
                    }
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
 

    }
}