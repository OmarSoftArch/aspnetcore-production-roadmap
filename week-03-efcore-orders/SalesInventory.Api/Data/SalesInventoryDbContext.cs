using Microsoft.EntityFrameworkCore;
using SalesInventory.Api.Models;

namespace SalesInventory.Api.Data;

public class SalesInventoryDbContext : DbContext
{
    public SalesInventoryDbContext(DbContextOptions<SalesInventoryDbContext> options)
        : base(options)
    {
    }

    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ConfigureCategories(modelBuilder);
        ConfigureProducts(modelBuilder);
        ConfigureCustomers(modelBuilder);
        ConfigureOrders(modelBuilder);
        ConfigureOrderItems(modelBuilder);
        SeedData(modelBuilder);
    }

    private static void ConfigureCategories(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(category => category.Id);

            entity.Property(category => category.Name)
                .IsRequired()
                .HasMaxLength(80);

            entity.Property(category => category.Description)
                .IsRequired()
                .HasMaxLength(250);

            entity.Property(category => category.IsActive)
                .HasDefaultValue(true);
        });
    }

    private static void ConfigureProducts(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(product => product.Id);

            entity.Property(product => product.Name)
                .IsRequired()
                .HasMaxLength(120);

            entity.Property(product => product.Description)
                .IsRequired()
                .HasMaxLength(500);

            entity.Property(product => product.Price)
                .HasPrecision(18, 2);

            entity.Property(product => product.IsActive)
                .HasDefaultValue(true);

            entity.HasOne(product => product.Category)
                .WithMany(category => category.Products)
                .HasForeignKey(product => product.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }

    private static void ConfigureCustomers(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(customer => customer.Id);

            entity.Property(customer => customer.FullName)
                .IsRequired()
                .HasMaxLength(120);

            entity.Property(customer => customer.Email)
                .IsRequired()
                .HasMaxLength(180);

            entity.HasIndex(customer => customer.Email)
                .IsUnique();

            entity.Property(customer => customer.PhoneNumber)
                .HasMaxLength(30);

            entity.Property(customer => customer.IsActive)
                .HasDefaultValue(true);
        });
    }

    private static void ConfigureOrders(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(order => order.Id);

            entity.Property(order => order.OrderNumber)
                .IsRequired()
                .HasMaxLength(40);

            entity.HasIndex(order => order.OrderNumber)
                .IsUnique();

            entity.Property(order => order.Notes)
                .HasMaxLength(500);

            entity.HasOne(order => order.Customer)
                .WithMany(customer => customer.Orders)
                .HasForeignKey(order => order.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }

    private static void ConfigureOrderItems(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasKey(orderItem => orderItem.Id);

            entity.Property(orderItem => orderItem.UnitPrice)
                .HasPrecision(18, 2);

            entity.HasOne(orderItem => orderItem.Order)
                .WithMany(order => order.Items)
                .HasForeignKey(orderItem => orderItem.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(orderItem => orderItem.Product)
                .WithMany(product => product.OrderItems)
                .HasForeignKey(orderItem => orderItem.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }

    private static void SeedData(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>().HasData(
            new Category
            {
                Id = 1,
                Name = "Software Subscriptions",
                Description = "Monthly and yearly SaaS products for business customers",
                IsActive = true
            },
            new Category
            {
                Id = 2,
                Name = "Warehouse Devices",
                Description = "Devices used in inventory and warehouse operations",
                IsActive = true
            },
            new Category
            {
                Id = 3,
                Name = "Office Equipment",
                Description = "Equipment used by internal business teams",
                IsActive = true
            });

        modelBuilder.Entity<Product>().HasData(
            new Product
            {
                Id = 1,
                Name = "Inventory Management Subscription",
                Description = "Cloud subscription for tracking products and stock levels",
                Price = 149.99m,
                StockQuantity = 100,
                CategoryId = 1,
                IsActive = true
            },
            new Product
            {
                Id = 2,
                Name = "Barcode Scanner",
                Description = "Handheld scanner for warehouse receiving and shipping",
                Price = 89.50m,
                StockQuantity = 25,
                CategoryId = 2,
                IsActive = true
            },
            new Product
            {
                Id = 3,
                Name = "Standing Desk",
                Description = "Adjustable office desk for internal teams",
                Price = 399.00m,
                StockQuantity = 12,
                CategoryId = 3,
                IsActive = true
            });

        modelBuilder.Entity<Customer>().HasData(
            new Customer
            {
                Id = 1,
                FullName = "Aisha Ahmed",
                Email = "aisha@example.com",
                PhoneNumber = "+966500000001",
                IsActive = true
            },
            new Customer
            {
                Id = 2,
                FullName = "Omar Khalid",
                Email = "omar@example.com",
                PhoneNumber = "+966500000002",
                IsActive = true
            });
    }
}
