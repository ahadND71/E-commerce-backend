using Microsoft.AspNetCore.Identity;

using Backend.Models;

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

        //
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
                    Password = _passwordHasher.HashPassword(null, "Test@123")
                },
                new Admin
                {
                    AdminId = Guid.Parse("03c18f24-d667-4c07-8c4f-454dea50c115"),
                    FirstName = "Enas",
                    LastName = "Batarfi",
                    Email = "enas@example.com",
                    Password = _passwordHasher.HashPassword(null, "Test@123")
                },
                new Admin
                {
                    AdminId = Guid.Parse("04c18f24-d667-4c07-8c4f-454dea50c115"),
                    FirstName = "Ahad",
                    LastName = "Nasser",
                    Email = "ahad@example.com",
                    Password = _passwordHasher.HashPassword(null, "Test@123")
                },
                new Admin
                {
                    AdminId = Guid.Parse("05c18f24-d667-4c07-8c4f-454dea50c115"),
                    FirstName = "Shahad",
                    LastName = "Draim",
                    Email = "shahad@example.com",
                    Password = _passwordHasher.HashPassword(null, "Test@123")
                }
            };

            foreach (Admin admin in admins)
            {
                context.Admins.Add(admin);
            }
        }

        //
        // if no customers found add these
        if (!context.Customers.Any())
        {
            var customers = new Customer[]
            {
                new Customer
                {
                    CustomerId = Guid.Parse("feee9ca6-fd69-46cf-a990-64db26780922"),
                    FirstName = "Marim",
                    LastName = "Ahmad",
                    Email = "marim@example.com",
                    Password = _passwordHasher.HashPassword(null, "Test@123")
                },
                new Customer
                {
                    CustomerId = Guid.Parse("12295810-446c-4ef3-b5f9-8b7ec0a81e88"),
                    FirstName = "Nora",
                    LastName = "Faisal",
                    Email = "nora@example.com",
                    Password = _passwordHasher.HashPassword(null, "Test@123")
                },
                new Customer
                {
                    CustomerId = Guid.Parse("a470781d-df28-4a68-b5f4-9b259b4b69d9"),
                    FirstName = "Fahad",
                    LastName = "Abdulrahman",
                    Email = "fahad@example.com",
                    Password = _passwordHasher.HashPassword(null, "Test@123")
                },
                new Customer
                {
                    CustomerId = Guid.Parse("cb45d6e0-b134-491a-8968-9f27fc5c4c37"),
                    FirstName = "Somyia",
                    LastName = "Saad",
                    Email = "somyia@example.com",
                    Password = _passwordHasher.HashPassword(null, "Test@123")
                }
            };

            foreach (Customer customer in customers)
            {
                context.Customers.Add(customer);
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
                    Name = "Stickers",
                    Slug = "stickers",
                    Description = "All things stickers, awesome, testing",
                    AdminId = Guid.Parse("02c18f24-d667-4c07-8c4f-454dea50c115")
                },
                new Category
                {
                    CategoryId = Guid.Parse("c94d673b-be8d-4b1f-8a36-cbd6ed765644"),
                    Name = "Notebooks",
                    Slug = "notebooks",
                    Description = "Journals, sketchbooks, planners, and notepads",
                    AdminId = Guid.Parse("02c18f24-d667-4c07-8c4f-454dea50c115")
                },
                new Category
                {
                    CategoryId = Guid.Parse("9c506bbf-0fd7-43af-9507-40fb32d8bdbd"),
                    Name = "Pens",
                    Slug = "pens",
                    Description = "Fountain pens, ballpoint pens, gel pens, and more!",
                    AdminId = Guid.Parse("02c18f24-d667-4c07-8c4f-454dea50c115")
                },
                new Category
                {
                    CategoryId = Guid.Parse("fafbdf01-de53-486b-9e4b-5b501cc8369e"),
                    Name = "Pencils",
                    Slug = "pencils",
                    Description = "Mechanical pencils, graphite pencils, colored pencils",
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
                    Price = 39.9m,
                    Description = "Saudi related stickers",
                    StockQuantity = 110,
                    CategoryId = Guid.Parse("bca83459-e31f-4ffc-9573-9245c9cbe6b7"),
                    AdminId = Guid.Parse("02c18f24-d667-4c07-8c4f-454dea50c115")
                },
                new Product
                {
                    ProductId = Guid.Parse("4963d195-33e0-4718-9c0c-f7db68678917"),
                    Name = "Planner Stickers",
                    Price = 5.9m,
                    Description = "Sheet of functional stickers for organizing your planner",
                    StockQuantity = 110,
                    CategoryId = Guid.Parse("bca83459-e31f-4ffc-9573-9245c9cbe6b7"),
                    AdminId = Guid.Parse("02c18f24-d667-4c07-8c4f-454dea50c115")
                },
                new Product
                {
                    ProductId = Guid.Parse("210518cb-b4c4-4d2f-9a53-f6520b534657"),
                    Name = "Stars Notebooks",
                    Price = 25.5m,
                    Description = "Notebook will take you to the stars",
                    StockQuantity = 40,
                    CategoryId = Guid.Parse("c94d673b-be8d-4b1f-8a36-cbd6ed765644"),
                    AdminId = Guid.Parse("02c18f24-d667-4c07-8c4f-454dea50c115")
                },
                new Product
                {
                    ProductId = Guid.Parse("33ad3125-c70f-485e-bf32-b90ad76e3ad4"),
                    Name = "Blue Gel pen",
                    Price = 35.5m,
                    Description = "Write your ideas",
                    StockQuantity = 30,
                    CategoryId = Guid.Parse("9c506bbf-0fd7-43af-9507-40fb32d8bdbd"),
                    AdminId = Guid.Parse("02c18f24-d667-4c07-8c4f-454dea50c115")
                },
                new Product
                {
                    ProductId = Guid.Parse("b106d590-002f-4b10-9237-b3954651efd0"),
                    Name = "Pencil",
                    Price = 35.5m,
                    Description = "Write your ideas",
                    StockQuantity = 30,
                    CategoryId = Guid.Parse("fafbdf01-de53-486b-9e4b-5b501cc8369e"),
                    AdminId = Guid.Parse("02c18f24-d667-4c07-8c4f-454dea50c115")
                }
            };

            foreach (Product p in products)
            {
                context.Products.Add(p);
            }
        }

        //
        // orderProduct
        // if (!context.OrderProducts.Any())
        // {
        //     var orderProduct = new OrderProduct[]
        //     {
        //         new OrderProduct
        //         {
        //             OrderProductId= Guid.Parse(""),
        //             ProductId = Guid.Parse("7b88a4f8-ee9f-44f7-99ef-e084da0c8ee9"),
        //             OrderId = Guid.Parse(""),
        //             ProductPrice = 12,
        //             Quantity = 2,
        //         },
        //
        //     };
        //
        //     foreach (OrderProduct op in orderProduct)
        //     {
        //         context.OrderProducts.Add(op);
        //     }
        // }

        //
        // order
        // if (!context.Orders.Any())
        // {
        //     var order = new Order[]
        //     {
        //         new Order
        //         {
        //             OrderId = Guid.Parse(""),
        //             AddressId = Guid.Parse(""),
        //             CreatedAt = DateTime.UtcNow,
        //             CustomerId = Guid.Parse(""),
        //             // OrderProducts = ,
        //             
        //         },
        //
        //     };
        //
        //     foreach (Order o in order)
        //     {
        //         context.Orders.Add(o);
        //     }
        // }


        //review
        // if (!context.Reviews.Any())
        // {
        //     var review = new Review[]
        //     {
        //         new Review
        //         {
        //             ReviewId = Guid.Parse(""),
        //             Comment = "",
        //             OrderId = Guid.Parse(""),
        //             CustomerId = Guid.Parse(""),
        //             ProductId = Guid.Parse(""),
        //             Rating = 3,
        //         },
        //
        //     };
        //
        //     foreach (Review r in review)
        //     {
        //         context.Reviews.Add(r);
        //     }
        // }


        context.SaveChanges();
    }
}