using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Context;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Product> Products { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Map Product entity to 'products' table in PostgreSQL
        modelBuilder.Entity<Product>()
            .ToTable("products", "public")
            .HasKey(p => p.ProductID);
        
        // Map properties to lowercase column names (PostgreSQL convention)
        modelBuilder.Entity<Product>()
            .Property(p => p.ProductID)
            .HasColumnName("productid");
        
        modelBuilder.Entity<Product>()
            .Property(p => p.ProductName)
            .HasColumnName("productname");
        
        modelBuilder.Entity<Product>()
            .Property(p => p.Category)
            .HasColumnName("category");
        
        modelBuilder.Entity<Product>()
            .Property(p => p.UnitPrice)
            .HasColumnName("unitprice");
        
        modelBuilder.Entity<Product>()
            .Property(p => p.QuantityInStock)
            .HasColumnName("quantityinstock");
    }
}