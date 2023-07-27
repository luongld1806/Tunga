using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TungaRestaurant.Models;

namespace TungaRestaurant.Data
{
    public class TungaRestaurantDbContext : IdentityDbContext<UserInfo>
    {
        public TungaRestaurantDbContext(DbContextOptions<TungaRestaurantDbContext> options)
            : base(options)
        {
        }
    
        public DbSet<Table> Table { get; set; }
        public DbSet<Branch> Branch { get; set; }
        public DbSet<Food> Foods { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
      
        public DbSet<Order> Orders { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Room> Rooms { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            
            builder.Entity<OrderDetail>().HasKey(order => new { order.OrderId, order.FoodId});
            
            builder.Entity<Branch>().HasMany(b=>b.Users).WithOne(u=>u.Branch).HasForeignKey(u => u.BranchId);
            builder.Entity<Branch>().HasMany(b=>b.Rooms).WithOne(r=>r.Branch).HasForeignKey(r => r.BranchId);
            builder.Entity<Room>().HasMany(r=>r.Tables).WithOne(t=>t.Room).HasForeignKey(t=>t.RoomId);

        }

    }
}
