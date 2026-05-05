using Microsoft.EntityFrameworkCore;
using MRAYAQ.Domain.Entities;

namespace MRAYAQ.Infrastructure.Data;

public class MrayaqDbContext : DbContext
{
    public MrayaqDbContext(DbContextOptions<MrayaqDbContext> options)
        : base(options)
    {
    }

    public DbSet<ContactMessage> ContactMessages { get; set; } = null!;
    public DbSet<Category> Categories { get; set; } = null!;
    public DbSet<Product> Products { get; set; } = null!;
    public DbSet<Order> Orders { get; set; } = null!;
    public DbSet<OrderItem> OrderItems { get; set; } = null!;
    public DbSet<AdminUser> AdminUsers { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var contact = modelBuilder.Entity<ContactMessage>();
        contact.HasKey(x => x.Id);
        contact.Property(x => x.Name).HasMaxLength(120).IsRequired();
        contact.Property(x => x.Email).HasMaxLength(200).IsRequired();
        contact.Property(x => x.Message).HasMaxLength(2000).IsRequired();
        contact.Property(x => x.Phone).HasMaxLength(40);
        contact.Property(x => x.CreatedAt).IsRequired();

        var category = modelBuilder.Entity<Category>();
        category.HasKey(x => x.Id);
        category.Property(x => x.Name).HasMaxLength(120).IsRequired();
        category.Property(x => x.Slug).HasMaxLength(120).IsRequired();

        var product = modelBuilder.Entity<Product>();
        product.HasKey(x => x.Id);
        product.Property(x => x.Name).HasMaxLength(200).IsRequired();
        product.Property(x => x.Description).HasMaxLength(2000).IsRequired();
        product.Property(x => x.Price).HasColumnType("decimal(10,2)");
        product.Property(x => x.Tag).HasMaxLength(60).IsRequired();
        product.Property(x => x.ImageUrl).HasMaxLength(500).IsRequired();
        product.HasOne(x => x.Category)
            .WithMany(x => x.Products)
            .HasForeignKey(x => x.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        var order = modelBuilder.Entity<Order>();
        order.HasKey(x => x.Id);
        order.Property(x => x.CustomerName).HasMaxLength(140).IsRequired();
        order.Property(x => x.Email).HasMaxLength(200).IsRequired();
        order.Property(x => x.Phone).HasMaxLength(40).IsRequired();
        order.Property(x => x.Address).HasMaxLength(260).IsRequired();
        order.Property(x => x.City).HasMaxLength(100).IsRequired();
        order.Property(x => x.Notes).HasMaxLength(500);
        order.Property(x => x.Status).HasMaxLength(30).IsRequired().HasDefaultValue("pending");
        order.Property(x => x.TotalAmount).HasColumnType("decimal(10,2)");
        order.Property(x => x.CreatedAt).IsRequired();

        var orderItem = modelBuilder.Entity<OrderItem>();
        orderItem.HasKey(x => x.Id);
        orderItem.Property(x => x.UnitPrice).HasColumnType("decimal(10,2)");
        orderItem.HasOne(x => x.Order)
            .WithMany(x => x.Items)
            .HasForeignKey(x => x.OrderId)
            .OnDelete(DeleteBehavior.Cascade);
        orderItem.HasOne(x => x.Product)
            .WithMany()
            .HasForeignKey(x => x.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        var admin = modelBuilder.Entity<AdminUser>();
        admin.HasKey(x => x.Id);
        admin.Property(x => x.Username).HasMaxLength(80).IsRequired();
        admin.Property(x => x.PasswordHash).HasMaxLength(200).IsRequired();
        admin.Property(x => x.PasswordSalt).HasMaxLength(200).IsRequired();
        admin.Property(x => x.CreatedAt).IsRequired();

        var teesId = Guid.Parse("a1111111-1111-1111-1111-111111111111");
        var sweatsId = Guid.Parse("b2222222-2222-2222-2222-222222222222");
        var jeansId = Guid.Parse("c3333333-3333-3333-3333-333333333333");
        var jacketsId = Guid.Parse("d4444444-4444-4444-4444-444444444444");
        var pantsId = Guid.Parse("e5555555-5555-5555-5555-555555555555");
        var accessoriesId = Guid.Parse("f6666666-6666-6666-6666-666666666666");

        category.HasData(
            new Category { Id = teesId, Name = "T-shirts & Tops", Slug = "tshirts" },
            new Category { Id = sweatsId, Name = "Sweats & Hoodies", Slug = "sweats" },
            new Category { Id = jeansId, Name = "Jeans & Denim", Slug = "jeans" },
            new Category { Id = jacketsId, Name = "Jackets & Outerwear", Slug = "jackets" },
            new Category { Id = pantsId, Name = "Pants & Chinos", Slug = "pants" },
            new Category { Id = accessoriesId, Name = "Accessories", Slug = "accessories" }
        );

        product.HasData(
            new Product
            {
                Id = Guid.Parse("11111111-aaaa-4aaa-8aaa-111111111111"),
                Name = "Hammamet Noir Tee",
                Description = "Minimal, coupe large, signature MRAYAQ.",
                Price = 79,
                Tag = "Core",
                ImageUrl = "https://images.unsplash.com/photo-1521572163474-6864f9cf17ab?auto=format&fit=crop&w=800&q=80",
                IsFeatured = true,
                CategoryId = teesId
            },
            new Product
            {
                Id = Guid.Parse("22222222-bbbb-4bbb-8bbb-222222222222"),
                Name = "La Marsa Hoodie",
                Description = "Hoodie oversized, fleece dense, vibes nocturnes.",
                Price = 159,
                Tag = "Drop 02",
                ImageUrl = "https://images.unsplash.com/photo-1512436991641-6745cdb1723f?auto=format&fit=crop&w=800&q=80",
                IsFeatured = true,
                CategoryId = sweatsId
            },
            new Product
            {
                Id = Guid.Parse("33333333-cccc-4ccc-8ccc-333333333333"),
                Name = "Carthage Denim",
                Description = "Denim brut, coupe droite, détail brodé.",
                Price = 179,
                Tag = "Denim",
                ImageUrl = "https://images.unsplash.com/photo-1542291026-7eec264c27ff?auto=format&fit=crop&w=800&q=80",
                IsFeatured = false,
                CategoryId = jeansId
            },
            new Product
            {
                Id = Guid.Parse("44444444-dddd-4ddd-8ddd-444444444444"),
                Name = "Kasbah Street Jacket",
                Description = "Twill coupe droite, broderie inspirée des médinas.",
                Price = 229,
                Tag = "Outer",
                ImageUrl = "https://images.unsplash.com/photo-1521572163474-6864f9cf17ab?auto=format&fit=crop&w=800&q=80",
                IsFeatured = true,
                CategoryId = jacketsId
            },
            new Product
            {
                Id = Guid.Parse("55555555-eeee-4eee-8eee-555555555555"),
                Name = "Sfax Linen Chino",
                Description = "Léger, respirant, ton sable pour les nuits chaudes.",
                Price = 149,
                Tag = "Summer",
                ImageUrl = "https://images.unsplash.com/photo-1524504388940-b1c1722653e1?auto=format&fit=crop&w=800&q=80",
                IsFeatured = false,
                CategoryId = pantsId
            },
            new Product
            {
                Id = Guid.Parse("66666666-ffff-4fff-8fff-666666666666"),
                Name = "Medina Cap",
                Description = "Casquette brodée, accent gold, fit urbain.",
                Price = 59,
                Tag = "Accessory",
                ImageUrl = "https://images.unsplash.com/photo-1524504388940-b1c1722653e1?auto=format&fit=crop&w=800&q=80",
                IsFeatured = false,
                CategoryId = accessoriesId
            }
        );
    }
}
