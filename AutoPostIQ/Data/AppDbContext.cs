csharp
using Microsoft.EntityFrameworkCore;
using AutoPostIQ.Models; // Assuming PostModel is in the Models namespace

namespace AutoPostIQ.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<PostModel> Posts { get; set; }

        // You can add more DbSets for other models here
        // public DbSet<AnotherModel> AnotherModels { get; set; }

        // Optional: Configure model properties if needed
        // protected override void OnModelCreating(ModelBuilder modelBuilder)
        // {
        //     base.OnModelCreating(modelBuilder);
        //     // Configure properties, relationships, etc.
        //     // modelBuilder.Entity<PostModel>().Property(p => p.Title).IsRequired();
        // }
    }
}