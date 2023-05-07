using System.Security.Cryptography;
using System.Text;
using Bank.Currency.Exchange.Application.Services;
using Bank.Currency.Exchange.Domain.Configurations;
using Bank.Currency.Exchange.Domain.DTOs;
using Bank.Currency.Exchange.Domain.Models;
using Bank.Currency.Exchange.Domain.Repositories;
using Bank.Currency.Exchange.Domain.Services;
using Bank.Currency.Exchange.Infrastructure.Services;
using Microsoft.Extensions.Options;

namespace Bank.Currency.Exchange.Test.UnitTest;

public class AccountServiceTest
{
    private readonly Mock<IOptions<JwtConfig>> _configMock = new();
    private readonly Mock<ILogger<AccountService>> _loggerMock = new();
    private readonly Mock<IUserRepository> _userRepoMock = new();
    private readonly Mock<ITokenService> _tokenMock = new();
    private readonly TokenService _tokenService;
    
    public AccountServiceTest()
    {
        _configMock.Setup(p => p.Value)
            .Returns(() => new JwtConfig { TokenKey = "kjdssudhuh8IJ299iH67*i2ihjiaasdlak" });
        _tokenService = new TokenService(_configMock.Object);
    }

    [Fact]
    public void AddUserExpectedResult()
    {
        //Arrange
        var user = new AddUserDto("Test", "Pass");
        _userRepoMock.Setup(x => x.AddUserAsync(It.IsAny<AppUser>()));
     
        var accountService = new AccountService(_tokenService, _userRepoMock.Object, _loggerMock.Object);

        //Act
        var response = accountService.AddUser(user);

        //Assert
        Assert.Equal(response.Result.Username, user.Username.ToLower());
        Assert.NotNull(response.Result.Token);
    }

    [Fact]
    public void AddUserErrorValidation()
    {
        //Arrange
        var user = new AddUserDto("Test", "Pass");
        var dbUser = new AppUser { Id = new Guid(), UserName = user.Username };
        _userRepoMock.Setup(x => x.GetUserAsync(It.IsAny<string>())).ReturnsAsync(dbUser);

        var accountService = new AccountService(_tokenMock.Object, _userRepoMock.Object, _loggerMock.Object);

        //Act
        var response = accountService.AddUser(user);

        //Assert
        Assert.Equal("One or more errors occurred. (User is taken)", response.Exception?.Message);
    }


    [Fact]
    public void LoginExpectedResult()
    {
        //Arrange
        var loginUser = new LoginDto("Test", "Pass");

        using var hmac = new HMACSHA512();
        var user = new AppUser
        {
            UserName = loginUser.Username.ToLower(),
            PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginUser.Password)),
            PasswordSalt = hmac.Key
        };

        _userRepoMock.Setup(x => x.GetUserAsync(It.IsAny<string>())).ReturnsAsync(user);


        var accountService = new AccountService(_tokenService, _userRepoMock.Object, _loggerMock.Object);

        //Act
        var response = accountService.Login(loginUser);

        //Assert
        Assert.Equal(response.Result.Username, loginUser.Username.ToLower());
        Assert.NotNull(response.Result.Token);
    }

    [Fact]
    public void LoginErrorValidation()
    {
        //Arrange
        var user = new LoginDto("Test", "Pass");
        _userRepoMock.Setup(x => x.GetUserAsync(It.IsAny<string>()));

        var accountService = new AccountService(_tokenMock.Object, _userRepoMock.Object, _loggerMock.Object);

        //Act
        var response = accountService.Login(user);

        //Assert
        Assert.Equal("One or more errors occurred. (Invalid user or password)", response.Exception?.Message);
    }

    [Fact]
    public void LoginInvalidUserValidation()
    {
        //Arrange
        var user = new LoginDto("Test", "Pass");
        _userRepoMock.Setup(x => x.GetUserAsync(It.IsAny<string>()))
            .ReturnsAsync(new AppUser { UserName = "Username", PasswordSalt = new byte[1000 * 1002 * 3],PasswordHash = new byte[1000 * 1002 * 3]});


        var accountService = new AccountService(_tokenMock.Object, _userRepoMock.Object, _loggerMock.Object);

        //Act
        var response = accountService.Login(user);

        //Assert
        Assert.Equal("One or more errors occurred. (Invalid user or password)", response.Exception?.Message);
    }
}