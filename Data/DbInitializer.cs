using Microsoft.AspNetCore.Identity;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Data;

public class DbInitializer
{
    private readonly IPasswordHasher<Admin> _passwordHasher;

    public DbInitializer(IPasswordHasher<Admin> passwordHasher)
    {
        _passwordHasher = passwordHasher;
    }

    public async Task InitializeAsync(AppDbContext context, IServiceProvider serviceProvider)
    {
        context.Database.EnsureCreated();

        // if no admins found add these
        if (!context.Admins.Any())
        {
            var admins = new Admin[]
            {
                new Admin
                {
                    AdminId = Guid.Parse("02c18f24-d667-4c07-8c4f-454dea50c115"),
                    FirstName = "Mohammad",
                    LastName = "Alkhamis",
                    Email = "moh@example.com",
                    Image = "moh.webp",
                    Mobile = "966554556677",
                    CreatedAt = new DateTime(2024, 4, 28, 11, 35, 0, DateTimeKind.Utc),
                    Password = _passwordHasher.HashPassword(null, "Test@123")
                }
            };

            foreach (Admin admin in admins)
            {
                context.Admins.Add(admin);
            }
        }


        //
        // if no categories found add these
        if (!context.Categories.Any())
        {
            var categories = new Category[]
            {
                new Category
                {
                    CategoryId = Guid.Parse("bca83459-e31f-4ffc-9573-9245c9cbe6b7"),
                    Name = "Abaya",
                    Slug = "abaya",
                    Description = "",
                    CreatedAt = new DateTime(2024, 4, 28, 22, 25, 25, DateTimeKind.Utc),
                    AdminId = Guid.Parse("02c18f24-d667-4c07-8c4f-454dea50c115")
                }
            };

            foreach (Category c in categories)
            {
                context.Categories.Add(c);
            }
        }

        //
        // if no products found add these
        if (!context.Products.Any())
        {
            var products = new Product[]
            {
                new Product
                {
                    ProductId = Guid.Parse("7b88a4f8-ee9f-44f7-99ef-e084da0c8ee9"),
                    Name = "Saudi stickers",
                    Slug = "saudi-stickers",
                    Price = 39.9m,
                    Description = "Saudi related stickers",
                    StockQuantity = 110,
                    ImgUrl = "product-1.webp",
                    CreatedAt = new DateTime(2024, 4, 29, 15, 25, 25, DateTimeKind.Utc),
                    CategoryId = Guid.Parse("bca83459-e31f-4ffc-9573-9245c9cbe6b7")
                }            };

            foreach (Product p in products)
            {
                context.Products.Add(p);
            }
        }
        context.SaveChanges();
    }
}