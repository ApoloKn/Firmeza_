using Xunit;
using Microsoft.EntityFrameworkCore;
using Firmeza.Data.Data;
using Firmeza.Data.Models;

namespace Firmeza.Tests.Fixtures;

public class DatabaseFixture : IDisposable
{
    public ApplicationDbContext Context { get; private set; }

    public DatabaseFixture()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        Context = new ApplicationDbContext(options);
        SeedTestData();
    }

    private void SeedTestData()
    {
        // Seed test customers
        var customers = new List<Customer>
        {
            new Customer
            {
                Id = 1,
                FirstName = "Test",
                LastName = "Customer",
                DocumentNumber = "12345678",
                Email = "test@customer.com",
                CreatedAt = DateTime.UtcNow
            },
            new Customer
            {
                Id = 2,
                FirstName = "Jane",
                LastName = "Doe",
                DocumentNumber = "87654321",
                Email = "jane@test.com",
                CreatedAt = DateTime.UtcNow
            }
        };
        Context.Customers.AddRange(customers);

        // Seed test products
        var products = new List<Product>
        {
            new Product
            {
                Id = 1,
                Name = "Test Product 1",
                Description = "Test Description 1",
                SKU = "TEST001",
                Price = 10.99M,
                Stock = 100,
                Unit = "unit",
                Category = "Electronics",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new Product
            {
                Id = 2,
                Name = "Test Product 2",
                Description = "Test Description 2",
                SKU = "TEST002",
                Price = 25.50M,
                Stock = 50,
                Unit = "unit",
                Category = "Books",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            }
        };
        Context.Products.AddRange(products);

        Context.SaveChanges();
    }

    public void Dispose()
    {
        Context?.Dispose();
    }
}

[CollectionDefinition("Database collection")]
public class DatabaseCollection : ICollectionFixture<DatabaseFixture>
{
    // This class has no code, and is never created. Its purpose is simply
    // to be the place to apply [CollectionDefinition] and all the
    // ICollectionFixture<> interfaces.
}
