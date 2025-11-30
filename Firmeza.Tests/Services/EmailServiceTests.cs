using Xunit;
using Moq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Firmeza.Api.Services;
using Firmeza.Data.Data;
using Firmeza.Data.Models;

namespace Firmeza.Tests.Services;

public class EmailServiceTests
{
    private readonly Mock<IConfiguration> _mockConfiguration;
    private readonly Mock<ILogger<EmailService>> _mockLogger;
    private readonly ApplicationDbContext _context;

    public EmailServiceTests()
    {
        // Setup in-memory database
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        _context = new ApplicationDbContext(options);

        // Setup mock configuration
        _mockConfiguration = new Mock<IConfiguration>();
        _mockConfiguration.Setup(c => c["Email:SmtpServer"]).Returns("smtp.test.com");
        _mockConfiguration.Setup(c => c["Email:SmtpPort"]).Returns("587");
        _mockConfiguration.Setup(c => c["Email:SenderEmail"]).Returns("test@firmeza.com");
        _mockConfiguration.Setup(c => c["Email:SenderName"]).Returns("Firmeza Test");
        _mockConfiguration.Setup(c => c["Email:Username"]).Returns("");
        _mockConfiguration.Setup(c => c["Email:Password"]).Returns("");

        _mockLogger = new Mock<ILogger<EmailService>>();
    }

    [Fact]
    public async Task SendReceiptEmailAsync_InvalidSaleId_ReturnsFalse()
    {
        // Arrange
        var emailService = new EmailService(_context, _mockConfiguration.Object, _mockLogger.Object);

        // Act
        var result = await emailService.SendReceiptEmailAsync(999);

        // Assert
        Assert.False(result);
        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Sale with ID 999 not found")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [Fact]
    public async Task SendReceiptEmailAsync_NoCustomerEmail_ReturnsFalse()
    {
        // Arrange
        var customer = new Customer
        {
            Id = 1,
            FirstName = "Test",
            LastName = "User",
            DocumentNumber = "123456",
            Email = null, // No email
            CreatedAt = DateTime.UtcNow
        };
        _context.Customers.Add(customer);

        var sale = new Sale
        {
            Id = 1,
            CustomerId = customer.Id,
            Customer = customer,
            SaleDate = DateTime.UtcNow,
            SubTotal = 100,
            Tax = 10,
            Discount = 0,
            Total = 110,
            Status = "Completed",
            CreatedAt = DateTime.UtcNow
        };
        _context.Sales.Add(sale);
        await _context.SaveChangesAsync();

        var emailService = new EmailService(_context, _mockConfiguration.Object, _mockLogger.Object);

        // Act
        var result = await emailService.SendReceiptEmailAsync(1);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task GenerateReceiptHtml_ValidSale_ContainsOrderDetails()
    {
        // Arrange
        var customer = new Customer
        {
            Id = 1,
            FirstName = "John",
            LastName = "Doe",
            DocumentNumber = "123456",
            Email = "john@test.com",
            CreatedAt = DateTime.UtcNow
        };
        _context.Customers.Add(customer);

        var product = new Product
        {
            Id = 1,
            Name = "Test Product",
            SKU = "TEST001",
            Price = 50,
            Stock = 10,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };
        _context.Products.Add(product);

        var sale = new Sale
        {
            Id = 1,
            CustomerId = customer.Id,
            Customer = customer,
            SaleDate = DateTime.UtcNow,
            SubTotal = 100,
            Tax = 10,
            Discount = 0,
            Total = 110,
            Status = "Completed",
            PaymentMethod = "Credit Card",
            CreatedAt = DateTime.UtcNow,
            SaleDetails = new List<SaleDetail>
            {
                new SaleDetail
                {
                    Id = 1,
                    ProductId = product.Id,
                    Product = product,
                    Quantity = 2,
                    UnitPrice = 50,
                    Discount = 0,
                    SubTotal = 100
                }
            }
        };
        _context.Sales.Add(sale);
        await _context.SaveChangesAsync();

        var emailService = new EmailService(_context, _mockConfiguration.Object, _mockLogger.Object);

        // Act - Using reflection to access private method
        var method = typeof(EmailService).GetMethod("GenerateReceiptHtml", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var html = method?.Invoke(emailService, new object[] { sale }) as string;

        // Assert
        Assert.NotNull(html);
        Assert.Contains("Order Number:", html);
        Assert.Contains("#1", html);
        Assert.Contains("John Doe", html);
        Assert.Contains("Test Product", html);
        Assert.Contains("$110.00", html);
        Assert.Contains("Credit Card", html);
    }

    [Fact]
    public void EmailService_Constructor_InitializesCorrectly()
    {
        // Arrange & Act
        var emailService = new EmailService(_context, _mockConfiguration.Object, _mockLogger.Object);

        // Assert
        Assert.NotNull(emailService);
    }

    public void Dispose()
    {
        _context?.Dispose();
    }
}
