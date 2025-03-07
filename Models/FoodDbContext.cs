using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Models
{
    public class FoodDbContext : DbContext
    {
        public FoodDbContext()
        {
        }

        public FoodDbContext(DbContextOptions<FoodDbContext> options) : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Order> Orders { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                    .Build();

                optionsBuilder.UseSqlServer(configuration.GetConnectionString("Food"));
            }

            optionsBuilder.EnableSensitiveDataLogging(); // Optional: chỉ sử dụng trong môi trường phát triển
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure relationships for Order entity
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Product)
                .WithMany(p => p.Orders)
                .HasForeignKey(o => o.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.Payment)
                .WithMany(p => p.Orders)
                .HasForeignKey(o => o.PaymentId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure relationships for Cart entity
            modelBuilder.Entity<Cart>()
                .HasOne(c => c.Product)
                .WithMany(p => p.Carts)
                .HasForeignKey(c => c.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Cart>()
                .HasOne(c => c.User)
                .WithMany(u => u.Carts)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure unique constraints and other configurations if needed
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();
            // Seeding data
            modelBuilder.Entity<Category>().HasData(
                new Category { CategoryId = 1, Name = "Fruits", ImageUrl = "fruits.jpg", IsActive = true },
                new Category { CategoryId = 2, Name = "Vegetables", ImageUrl = "vegetables.jpg", IsActive = true }
            );

            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    ProductId = 1,
                    Name = "Apple",
                    Description = "Fresh red apples",
                    Price = 1.50m,
                    Quantity = 100,
                    ImageUrl = "apple.jpg",
                    CategoryId = 1,
                    IsActive = true
                },
                new Product
                {
                    ProductId = 2,
                    Name = "Carrot",
                    Description = "Organic carrots",
                    Price = 0.80m,
                    Quantity = 200,
                    ImageUrl = "carrot.jpg",
                    CategoryId = 2,
                    IsActive = true
                }
            );

            modelBuilder.Entity<User>().HasData(
                new User
                {
                    UserId = 1,
                    Name = "Admin User",
                    Username = "admin",
                    Password = "admin123", // Passwords should be hashed in production
                    Email = "admin@example.com",
                    Role = "Admin",
                    CreatedDate = DateTime.Now
                },
                new User
                {
                    UserId = 2,
                    Name = "Regular User",
                    Username = "user",
                    Password = "user123", // Passwords should be hashed in production
                    Email = "user@example.com",
                    Role = "User",
                    CreatedDate = DateTime.Now
                }
            );

            modelBuilder.Entity<Payment>().HasData(
                new Payment
                {
                    PaymentId = 1,
                    Name = "John Doe",
                    CardNo = "1234567812345678",
                    ExpiryDate = new DateTime(2025, 12, 31),
                    CvvNo = "123",
                    Address = "123 Main St",
                    PaymentMode = "Credit Card"
                }
            );
            modelBuilder.Entity<Order>().HasData(
    new Order
    {
        OrderDetailsId = 1,
        OrderNo = "ORD001",  // Mã đơn hàng
        ProductId = 1, // ID sản phẩm (ví dụ là Apple)
        Quantity = 2, // Số lượng
        UserId = 1, // ID người dùng (ví dụ là Admin User)
        Status = "Pending", // Trạng thái đơn hàng
        PaymentId = 1, // ID thanh toán (ví dụ là payment cho John Doe)
        OrderDate = DateTime.Now // Ngày đặt hàng
    },
    new Order
    {
        OrderDetailsId = 2,
        OrderNo = "ORD002",
        ProductId = 2, // Sản phẩm là Carrot
        Quantity = 3,
        UserId = 2, // Người dùng là Regular User
        Status = "Completed", // Đơn hàng đã hoàn tất
        PaymentId = 1, // Thanh toán từ John Doe
        OrderDate = DateTime.Now
    }
        );
            modelBuilder.Entity<Cart>().HasData(
    new Cart
    {
        CartId = 1,
        ProductId = 1,  // Sản phẩm Apple
        Quantity = 2,   // Số lượng 2
        UserId = 1      // Người dùng là Admin User
    },
    new Cart
    {
        CartId = 2,
        ProductId = 2,  // Sản phẩm Carrot
        Quantity = 3,   // Số lượng 3
        UserId = 1      // Người dùng là Admin User
    },
    new Cart
    {
        CartId = 3,
        ProductId = 1,  // Sản phẩm Apple
        Quantity = 1,   // Số lượng 1
        UserId = 2      // Người dùng là Regular User
    },
    new Cart
    {
        CartId = 4,
        ProductId = 2,  // Sản phẩm Carrot
        Quantity = 4,   // Số lượng 4
        UserId = 2      // Người dùng là Regular User
    },
    new Cart
    {
        CartId = 5,
        ProductId = 1,  // Sản phẩm Apple
        Quantity = 5,   // Số lượng 5
        UserId = 2      // Người dùng là Regular User
    }
);

        }
    }
}
