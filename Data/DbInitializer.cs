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
                },
                new Admin
                {
                    AdminId = Guid.Parse("03c18f24-d667-4c07-8c4f-454dea50c115"),
                    FirstName = "Enas",
                    LastName = "Batarfi",
                    Email = "enas@example.com",
                    Image = "enas.webp",
                    Mobile = "966554556677",
                    CreatedAt = new DateTime(2024, 4, 29, 15, 44, 22, DateTimeKind.Utc),
                    Password = _passwordHasher.HashPassword(null, "Test@123")
                },
                new Admin
                {
                    AdminId = Guid.Parse("04c18f24-d667-4c07-8c4f-454dea50c115"),
                    FirstName = "Ahad",
                    LastName = "Nasser",
                    Email = "ahad@example.com",
                    Image = "ahad.webp",
                    Mobile = "966554556677",
                    CreatedAt = new DateTime(2024, 4, 28, 8, 18, 32, DateTimeKind.Utc),
                    Password = _passwordHasher.HashPassword(null, "Test@123")
                },
                new Admin
                {
                    AdminId = Guid.Parse("05c18f24-d667-4c07-8c4f-454dea50c115"),
                    FirstName = "Shahad",
                    LastName = "Draim",
                    Email = "shahad@example.com",
                    Image = "shahad.webp",
                    Mobile = "966554556677",
                    CreatedAt = new DateTime(2024, 4, 30, 18, 55, 55, DateTimeKind.Utc),
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
                    Image = "image.webp",
                    Mobile = "966554556677",
                    CreatedAt = new DateTime(2024, 5, 1, 12, 12, 12, DateTimeKind.Utc),
                    Password = _passwordHasher.HashPassword(null, "Test@123")
                },
                new Customer
                {
                    CustomerId = Guid.Parse("12295810-446c-4ef3-b5f9-8b7ec0a81e88"),
                    FirstName = "Nora",
                    LastName = "Faisal",
                    Email = "nora@example.com",
                    Image = "image.webp",
                    Mobile = "966554556677",
                    CreatedAt = new DateTime(2024, 5, 1, 12, 12, 12, DateTimeKind.Utc),
                    Password = _passwordHasher.HashPassword(null, "Test@123")
                },
                new Customer
                {
                    CustomerId = Guid.Parse("a470781d-df28-4a68-b5f4-9b259b4b69d9"),
                    FirstName = "Fahad",
                    LastName = "Abdulrahman",
                    Email = "fahad@example.com",
                    Image = "image.webp",
                    Mobile = "966554556677",
                    CreatedAt = new DateTime(2024, 5, 1, 12, 12, 12, DateTimeKind.Utc),
                    Password = _passwordHasher.HashPassword(null, "Test@123")
                },
                new Customer
                {
                    CustomerId = Guid.Parse("cb45d6e0-b134-491a-8968-9f27fc5c4c37"),
                    FirstName = "Somyia",
                    LastName = "Saad",
                    Email = "somyia@example.com",
                    Image = "image.webp",
                    Mobile = "966554556677",
                    CreatedAt = new DateTime(2024, 5, 1, 12, 12, 12, DateTimeKind.Utc),
                    Password = _passwordHasher.HashPassword(null, "Test@123")
                }
            };

            foreach (Customer customer in customers)
            {
                context.Customers.Add(customer);
            }
        }
        //
        // // if no customers found add these
        // if (!context.Addresses.Any())
        // {
        //     var Addresses = new Address[]
        //     {
        //         new Address
        //             {
        //                 AddressId = Guid.Parse("9d358843-fd84-4af3-b486-fa8725c8af42"),
        //                 Name = "John Smith",
        //                 AddressLine1 = "123 Maple Street",
        //                 AddressLine2 = "Apt 301",
        //                 Country = "United States",
        //                 Province = "California",
        //                 City = "Los Angeles",
        //                 ZipCode = "90001",
        //                 CustomerId = Guid.Parse("cb45d6e0-b134-491a-8968-9f27fc5c4c37")
        //             },
        //             new Address
        //             {
        //                 AddressId = Guid.Parse("76f4d253-2171-4985-8c9e-f1979423ebc8"),
        //                 Name = "Jane Doe",
        //                 AddressLine1 = "456 Oak Avenue",
        //                 AddressLine2 = "",
        //                 Country = "United States",
        //                 Province = "New York",
        //                 City = "New York City",
        //                 ZipCode = "10001",
        //                 CustomerId = Guid.Parse("cb45d6e0-b134-491a-8968-9f27fc5c4c37")
        //             },
        //             new Address
        //             {
        //                 AddressId = Guid.Parse("76f4d253-2171-4985-8c9e-f1979423ebc8"),
        //                 Name = "David Johnson",
        //                 AddressLine1 = "789 Elm Street",
        //                 AddressLine2 = "Suite 102",
        //                 Country = "United States",
        //                 Province = "Texas",
        //                 City = "Houston",
        //                 ZipCode = "77002",
        //                 CustomerId = Guid.Parse("a470781d-df28-4a68-b5f4-9b259b4b69d9")
        //             },
        //             new Address
        //             {
        //                 AddressId = Guid.Parse("fb639447-d196-4de4-b03d-6bcc3ca000e4"),
        //                 Name = "Emily Williams",
        //                 AddressLine1 = "321 Pine Avenue",
        //                 AddressLine2 = "Apt 10B",
        //                 Country = "United States",
        //                 Province = "Florida",
        //                 City = "Miami",
        //                 ZipCode = "33101",
        //                 CustomerId = Guid.Parse("a470781d-df28-4a68-b5f4-9b259b4b69d9")
        //             },
        //             new Address
        //             {
        //                 AddressId = Guid.Parse("c13854e7-aab7-4e5f-88dd-8a701924d80b"),
        //                 Name = "Michael Brown",
        //                 AddressLine1 = "567 Cedar Street",
        //                 AddressLine2 = "Suite 501",
        //                 Country = "United States",
        //                 Province = "Illinois",
        //                 City = "Chicago",
        //                 ZipCode = "60601",
        //                 CustomerId = Guid.Parse("12295810-446c-4ef3-b5f9-8b7ec0a81e88")
        //             },
        //             new Address
        //             {
        //                 AddressId = Guid.Parse("e29e8d72-a949-4fe9-a986-eae03ed2a48f"),
        //                 Name = "Sarah Wilson",
        //                 AddressLine1 = "901 Walnut Avenue",
        //                 AddressLine2 = "",
        //                 Country = "United States",
        //                 Province = "Pennsylvania",
        //                 City = "Philadelphia",
        //                 ZipCode = "19101",
        //                 CustomerId = Guid.Parse("12295810-446c-4ef3-b5f9-8b7ec0a81e88")
        //             },
        //             new Address
        //             {
        //                 AddressId = Guid.Parse("e89df668-cef1-402f-88c2-7c00990867aa"),
        //                 Name = "Matthew Taylor",
        //                 AddressLine1 = "234 Birch Street",
        //                 AddressLine2 = "Apt 15C",
        //                 Country = "United States",
        //                 Province = "Massachusetts",
        //                 City = "Boston",
        //                 ZipCode = "02101",
        //                 CustomerId = Guid.Parse("12295810-446c-4ef3-b5f9-8b7ec0a81e88")
        //             },
        //             new Address
        //             {
        //                 AddressId = Guid.Parse("b931adf8-5ae3-41c0-b6d2-c7da06a90b2b"),
        //                 Name = "Amanda Martinez",
        //                 AddressLine1 = "678 Pineapple Avenue",
        //                 AddressLine2 = "",
        //                 Country = "United States",
        //                 Province = "Florida",
        //                 City = "Orlando",
        //                 ZipCode = "32801",
        //                 CustomerId = Guid.Parse("feee9ca6-fd69-46cf-a990-64db26780922")
        //             },
        //             new Address
        //             {
        //                 AddressId = Guid.Parse("d5c6760b-8563-43e6-950b-2ebdd9ace603"),
        //                 Name = "Christopher Garcia",
        //                 AddressLine1 = "890 Cherry Street",
        //                 AddressLine2 = "Suite 401",
        //                 Country = "United States",
        //                 Province = "California",
        //                 City = "San Francisco",
        //                 ZipCode = "94101",
        //                 CustomerId = Guid.Parse("feee9ca6-fd69-46cf-a990-64db26780922")
        //             },
        //             new Address
        //             {
        //                 AddressId = Guid.Parse("7a5787cd-2d91-430e-a421-9bd60fa072f1"),
        //                 Name = "Jennifer Rodriguez",
        //                 AddressLine1 = "543 Plum Avenue",
        //                 AddressLine2 = "",
        //                 Country = "United States",
        //                 Province = "Texas",
        //                 City = "Austin",
        //                 ZipCode = "78701",
        //                 CustomerId = Guid.Parse("feee9ca6-fd69-46cf-a990-64db26780922")
        //             }
        //     };

        //     foreach (Address address in Addresses)
        //     {
        //         context.Addresses.Add(address);
        //     }
        // }

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
                    CreatedAt = new DateTime(2024, 4, 28, 22, 25, 25, DateTimeKind.Utc),
                    AdminId = Guid.Parse("02c18f24-d667-4c07-8c4f-454dea50c115")
                },
                new Category
                {
                    CategoryId = Guid.Parse("c94d673b-be8d-4b1f-8a36-cbd6ed765644"),
                    Name = "Notebooks",
                    Slug = "notebooks",
                    Description = "Journals, sketchbooks, planners, and notepads",
                    CreatedAt = new DateTime(2024, 4, 28, 22, 25, 25, DateTimeKind.Utc),
                    AdminId = Guid.Parse("02c18f24-d667-4c07-8c4f-454dea50c115")
                },
                new Category
                {
                    CategoryId = Guid.Parse("9c506bbf-0fd7-43af-9507-40fb32d8bdbd"),
                    Name = "Pens",
                    Slug = "pens",
                    Description = "Fountain pens, ballpoint pens, gel pens, and more!",
                    CreatedAt = new DateTime(2024, 4, 28, 23, 25, 25, DateTimeKind.Utc),
                    AdminId = Guid.Parse("02c18f24-d667-4c07-8c4f-454dea50c115")
                },
                new Category
                {
                    CategoryId = Guid.Parse("fafbdf01-de53-486b-9e4b-5b501cc8369e"),
                    Name = "Pencils",
                    Slug = "pencils",
                    Description = "Mechanical pencils, graphite pencils, colored pencils",
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
                    SKU = "PRO-TES-ING-THIS",
                    ImgUrl = "product-1.webp",
                    CreatedAt = new DateTime(2024, 4, 29, 15, 25, 25, DateTimeKind.Utc),
                    CategoryId = Guid.Parse("bca83459-e31f-4ffc-9573-9245c9cbe6b7"),
                    AdminId = Guid.Parse("02c18f24-d667-4c07-8c4f-454dea50c115")
                },
                new Product
                {
                    ProductId = Guid.Parse("4963d195-33e0-4718-9c0c-f7db68678917"),
                    Name = "Planner Stickers",
                    Slug = "planner-stickers",
                    Price = 5.9m,
                    Description = "Sheet of functional stickers for organizing your planner",
                    StockQuantity = 110,
                    SKU = "PRO-TES-ING-THIS",
                    ImgUrl = "product-1.webp",
                    CreatedAt = new DateTime(2024, 4, 29, 16, 25, 25, DateTimeKind.Utc),
                    CategoryId = Guid.Parse("bca83459-e31f-4ffc-9573-9245c9cbe6b7"),
                    AdminId = Guid.Parse("02c18f24-d667-4c07-8c4f-454dea50c115")
                },
                new Product
                {
                    ProductId = Guid.Parse("210518cb-b4c4-4d2f-9a53-f6520b534657"),
                    Name = "Stars Notebooks",
                    Slug = "stars-notebooks",
                    Price = 25.5m,
                    Description = "Notebook will take you to the stars",
                    StockQuantity = 40,
                    SKU = "PRO-TES-ING-THIS",
                    ImgUrl = "product-1.webp",
                    CreatedAt = new DateTime(2024, 4, 29, 17, 25, 25, DateTimeKind.Utc),
                    CategoryId = Guid.Parse("c94d673b-be8d-4b1f-8a36-cbd6ed765644"),
                    AdminId = Guid.Parse("02c18f24-d667-4c07-8c4f-454dea50c115")
                },
                new Product
                {
                    ProductId = Guid.Parse("33ad3125-c70f-485e-bf32-b90ad76e3ad4"),
                    Name = "Blue Gel pen",
                    Slug = "blue-gel-pen",
                    Price = 35.5m,
                    Description = "Write your ideas",
                    StockQuantity = 30,
                    SKU = "PRO-TES-ING-THIS",
                    ImgUrl = "product-1.webp",
                    CreatedAt = new DateTime(2024, 4, 29, 17, 25, 25, DateTimeKind.Utc),
                    CategoryId = Guid.Parse("9c506bbf-0fd7-43af-9507-40fb32d8bdbd"),
                    AdminId = Guid.Parse("02c18f24-d667-4c07-8c4f-454dea50c115")
                },
                new Product
                {
                    ProductId = Guid.Parse("b106d590-002f-4b10-9237-b3954651efd0"),
                    Name = "Pencil",
                    Slug = "pencil",
                    Price = 35.5m,
                    Description = "Write your ideas",
                    StockQuantity = 30,
                    SKU = "PRO-TES-ING-THIS",
                    ImgUrl = "product-1.webp",
                    CreatedAt = new DateTime(2024, 4, 29, 17, 25, 25, DateTimeKind.Utc),
                    CategoryId = Guid.Parse("fafbdf01-de53-486b-9e4b-5b501cc8369e"),
                    AdminId = Guid.Parse("02c18f24-d667-4c07-8c4f-454dea50c115")
                }
            };

            foreach (Product p in products)
            {
                context.Products.Add(p);
            }
        }

        // if no order products found add these
        if (!context.OrderProducts.Any())
        {
            var orderProducts = new List<OrderProduct>
            {
                // Order 1 Products
                new OrderProduct
                {
                    OrderProductId = Guid.Parse("51f1f3a8-23d2-4c80-b159-88a1ee8f893b"),
                    ProductId = Guid.Parse("7b88a4f8-ee9f-44f7-99ef-e084da0c8ee9"),
                    OrderId = Guid.Parse("d043296e-d2d5-4374-88f8-5fe0d6d71e5e"),
                    ProductPrice = 12m,
                    Quantity = 2,
                },
                new OrderProduct
                {
                    OrderProductId = Guid.Parse("6727fb21-4ad2-4ad3-8915-8feee0a3f1aa"),
                    ProductId = Guid.Parse("4963d195-33e0-4718-9c0c-f7db68678917"),
                    OrderId = Guid.Parse("d043296e-d2d5-4374-88f8-5fe0d6d71e5e"),
                    ProductPrice = 5.9m,
                    Quantity = 3,
                },
                // Order 2 Products
                new OrderProduct
                {
                    OrderProductId = Guid.Parse("abf95b0e-bdd5-40c1-aa46-1f5e7803d8c7"),
                    ProductId = Guid.Parse("210518cb-b4c4-4d2f-9a53-f6520b534657"),
                    OrderId = Guid.Parse("f1c9eae7-7ba4-482e-ba5f-f4a795b2c228"),
                    ProductPrice = 25.5m,
                    Quantity = 1,
                },
                new OrderProduct
                {
                    OrderProductId = Guid.Parse("73a0f514-65fb-4667-ba2a-df028fa6d6fd"),
                    ProductId = Guid.Parse("33ad3125-c70f-485e-bf32-b90ad76e3ad4"),
                    OrderId = Guid.Parse("f1c9eae7-7ba4-482e-ba5f-f4a795b2c228"),
                    ProductPrice = 35.5m,
                    Quantity = 2,
                },
                new OrderProduct
                {
                    OrderProductId = Guid.Parse("74b8ed87-7b15-4514-86de-3e39343d35d6"),
                    ProductId = Guid.Parse("b106d590-002f-4b10-9237-b3954651efd0"),
                    OrderId = Guid.Parse("f1c9eae7-7ba4-482e-ba5f-f4a795b2c228"),
                    ProductPrice = 35.5m,
                    Quantity = 1,
                },
                // Order 3 Products
                new OrderProduct
                {
                    OrderProductId = Guid.Parse("6b05c920-0e45-44e7-af46-dc6dcf332b18"),
                    ProductId = Guid.Parse("7b88a4f8-ee9f-44f7-99ef-e084da0c8ee9"),
                    OrderId = Guid.Parse("b0cd279d-5316-45b5-8c47-fcd3cb707cb2"),
                    ProductPrice = 12m,
                    Quantity = 1,
                },
                new OrderProduct
                {
                    OrderProductId = Guid.Parse("d20a0ae1-2b34-4b60-976e-4ec6a34d393f"),
                    ProductId = Guid.Parse("4963d195-33e0-4718-9c0c-f7db68678917"),
                    OrderId = Guid.Parse("b0cd279d-5316-45b5-8c47-fcd3cb707cb2"),
                    ProductPrice = 5.9m,
                    Quantity = 1,
                },
                new OrderProduct
                {
                    OrderProductId = Guid.Parse("045b27bf-43e9-4d08-b192-67dc4d4d4c65"),
                    ProductId = Guid.Parse("210518cb-b4c4-4d2f-9a53-f6520b534657"),
                    OrderId = Guid.Parse("b0cd279d-5316-45b5-8c47-fcd3cb707cb2"),
                    ProductPrice = 25.5m,
                    Quantity = 2,
                },
            };

            foreach (OrderProduct o in orderProducts)
            {
                context.OrderProducts.Add(o);
            }
        }

        // if no orders found add these
        if (!context.Orders.Any())
        {
            var orders = new List<Order>
            {
                // Order 1
                new Order
                {
                    OrderId = Guid.Parse("d043296e-d2d5-4374-88f8-5fe0d6d71e5e"),
                    AddressId = Guid.Parse("cfb3ff24-82db-492e-b7bb-1a02727bc399"),
                    CreatedAt = DateTime.UtcNow,
                    CustomerId = Guid.Parse("feee9ca6-fd69-46cf-a990-64db26780922"),
                },
                // Order 2
                new Order
                {
                    OrderId = Guid.Parse("f1c9eae7-7ba4-482e-ba5f-f4a795b2c228"),
                    AddressId = Guid.Parse("cfb3ff24-82db-492e-b7bb-1a02727bc399"),
                    CreatedAt = DateTime.UtcNow,
                    CustomerId = Guid.Parse("12295810-446c-4ef3-b5f9-8b7ec0a81e88"),
                },
                // Order 3
                new Order
                {
                    OrderId = Guid.Parse("b0cd279d-5316-45b5-8c47-fcd3cb707cb2"),
                    AddressId = Guid.Parse("cfb3ff24-82db-492e-b7bb-1a02727bc399"),
                    CreatedAt = DateTime.UtcNow,
                    CustomerId = Guid.Parse("a470781d-df28-4a68-b5f4-9b259b4b69d9"),
                }
            };

            foreach (Order o in orders)
            {
                context.Orders.Add(o);
            }
        }

        // if no reviews found add these
        if (!context.Reviews.Any())
        {
            var reviews = new List<Review>
            {
                // Review 1
                new Review
                {
                    ReviewId = Guid.Parse("e4f95d92-575f-48da-a001-1f99c7d2d799"),
                    Comment = "Great product!",
                    OrderId = Guid.Parse("d043296e-d2d5-4374-88f8-5fe0d6d71e5e"),
                    CustomerId = Guid.Parse("feee9ca6-fd69-46cf-a990-64db26780922"),
                    ProductId = Guid.Parse("7b88a4f8-ee9f-44f7-99ef-e084da0c8ee9"),
                    Rating = 5,
                },
                // Review 2
                new Review
                {
                    ReviewId = Guid.Parse("189b91f7-1c57-4684-91cc-5baf7e4d3784"),
                    Comment = "Fast delivery!",
                    OrderId = Guid.Parse("f1c9eae7-7ba4-482e-ba5f-f4a795b2c228"),
                    CustomerId = Guid.Parse("12295810-446c-4ef3-b5f9-8b7ec0a81e88"),
                    ProductId = Guid.Parse("210518cb-b4c4-4d2f-9a53-f6520b534657"),
                    Rating = 4,
                },
                // Review 3
                new Review
                {
                    ReviewId = Guid.Parse("3c477a6c-ba9c-45d1-bab8-5c656d09de12"),
                    Comment = "Not as described.",
                    OrderId = Guid.Parse("f1c9eae7-7ba4-482e-ba5f-f4a795b2c228"),
                    CustomerId = Guid.Parse("12295810-446c-4ef3-b5f9-8b7ec0a81e88"),
                    ProductId = Guid.Parse("33ad3125-c70f-485e-bf32-b90ad76e3ad4"),
                    Rating = 2,
                },
                // Review 4
                new Review
                {
                    ReviewId = Guid.Parse("1f63a23a-4f55-4e69-bfe0-23830bdc44b0"),
                    Comment = "Good quality, thank you!",
                    OrderId = Guid.Parse("b0cd279d-5316-45b5-8c47-fcd3cb707cb2"),
                    CustomerId = Guid.Parse("a470781d-df28-4a68-b5f4-9b259b4b69d9"),
                    ProductId = Guid.Parse("7b88a4f8-ee9f-44f7-99ef-e084da0c8ee9"),
                    Rating = 4,
                },
                // Review 5
                new Review
                {
                    ReviewId = Guid.Parse("8aa7e85a-c9c5-4c92-b6bc-3f401c48d04d"),
                    Comment = "Nice planner stickers!",
                    OrderId = Guid.Parse("b0cd279d-5316-45b5-8c47-fcd3cb707cb2"),
                    CustomerId = Guid.Parse("a470781d-df28-4a68-b5f4-9b259b4b69d9"),
                    ProductId = Guid.Parse("4963d195-33e0-4718-9c0c-f7db68678917"),
                    Rating = 4,
                },
            };

            foreach (Review r in reviews)
            {
                context.Reviews.Add(r);
            }
        }


        context.SaveChanges();
    }
}