using Xunit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Moq;
using Firmeza.Api.Controllers;
using Firmeza.Api.DTOs;

namespace Firmeza.Tests.Controllers;

public class AuthControllerTests
{
    private readonly Mock<UserManager<IdentityUser>> _mockUserManager;
    private readonly Mock<SignInManager<IdentityUser>> _mockSignInManager;
    private readonly Mock<IConfiguration> _mockConfiguration;

    public AuthControllerTests()
    {
        var userStoreMock = new Mock<IUserStore<IdentityUser>>();
        _mockUserManager = new Mock<UserManager<IdentityUser>>(
            userStoreMock.Object, null, null, null, null, null, null, null, null);

        var contextAccessor = new Mock<Microsoft.AspNetCore.Http.IHttpContextAccessor>();
        var claimsFactory = new Mock<IUserClaimsPrincipalFactory<IdentityUser>>();
        _mockSignInManager = new Mock<SignInManager<IdentityUser>>(
            _mockUserManager.Object, contextAccessor.Object, claimsFactory.Object, null, null, null, null);

        _mockConfiguration = new Mock<IConfiguration>();
        _mockConfiguration.Setup(c => c["Jwt:Key"]).Returns("YourSuperSecretKeyForJWTTokenGenerationShouldBeAtLeast32CharactersLong");
        _mockConfiguration.Setup(c => c["Jwt:Issuer"]).Returns("FirmezaApi");
        _mockConfiguration.Setup(c => c["Jwt:Audience"]).Returns("FirmezaClient");
        _mockConfiguration.Setup(c => c["Jwt:ExpiresInMinutes"]).Returns("60");
    }

    [Fact]
    public async Task Login_InvalidCredentials_ReturnsUnauthorized()
    {
        // Arrange
        _mockUserManager.Setup(x => x.FindByNameAsync(It.IsAny<string>()))
            .ReturnsAsync((IdentityUser)null);

        var controller = new AuthController(
            _mockUserManager.Object,
            _mockSignInManager.Object,
            _mockConfiguration.Object);

        var loginDto = new LoginDto
        {
            Username = "nonexistent",
            Password = "password"
        };

        // Act
        var result = await controller.Login(loginDto);

        // Assert
        Assert.IsType<UnauthorizedObjectResult>(result);
    }

    [Fact]
    public async Task Login_ValidCredentials_ReturnsOkWithToken()
    {
        // Arrange
        var user = new IdentityUser
        {
            UserName = "testuser",
            Email = "test@test.com"
        };

        _mockUserManager.Setup(x => x.FindByNameAsync("testuser"))
            .ReturnsAsync(user);
        _mockSignInManager.Setup(x => x.CheckPasswordSignInAsync(user, "password", false))
            .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Success);
        _mockUserManager.Setup(x => x.GetRolesAsync(user))
            .ReturnsAsync(new List<string> { "Customer" });

        var controller = new AuthController(
            _mockUserManager.Object,
            _mockSignInManager.Object,
            _mockConfiguration.Object);

        var loginDto = new LoginDto
        {
            Username = "testuser",
            Password = "password"
        };

        // Act
        var result = await controller.Login(loginDto);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.NotNull(okResult.Value);
    }

    [Fact]
    public async Task Register_ValidData_ReturnsOk()
    {
        // Arrange
        _mockUserManager.Setup(x => x.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Success);
        _mockUserManager.Setup(x => x.AddToRoleAsync(It.IsAny<IdentityUser>(), "Customer"))
            .ReturnsAsync(IdentityResult.Success);

        var controller = new AuthController(
            _mockUserManager.Object,
            _mockSignInManager.Object,
            _mockConfiguration.Object);

        var registerDto = new RegisterDto
        {
            Username = "newuser",
            Email = "new@test.com",
            Password = "Password123!"
        };

        // Act
        var result = await controller.Register(registerDto);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.NotNull(okResult.Value);
    }

    [Fact]
    public async Task Register_FailedCreation_ReturnsBadRequest()
    {
        // Arrange
        var errors = new List<IdentityError>
        {
            new IdentityError { Description = "Username already exists" }
        };
        
        _mockUserManager.Setup(x => x.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Failed(errors.ToArray()));

        var controller = new AuthController(
            _mockUserManager.Object,
            _mockSignInManager.Object,
            _mockConfiguration.Object);

        var registerDto = new RegisterDto
        {
            Username = "existinguser",
            Email = "existing@test.com",
            Password = "Password123!"
        };

        // Act
        var result = await controller.Register(registerDto);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }
}
