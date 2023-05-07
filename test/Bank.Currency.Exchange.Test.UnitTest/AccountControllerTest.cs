using Bank.Currency.Exchange.Api.Controllers;
using Bank.Currency.Exchange.Application.Services;
using Bank.Currency.Exchange.Domain.DTOs;
using Bank.Currency.Exchange.Domain.Repositories;
using Bank.Currency.Exchange.Domain.Services;

namespace Bank.Currency.Exchange.Test.UnitTest;

public class AccountControllerTest
{
    private readonly Mock<IAccountService> _accountServiceMock = new();
    private readonly Mock<ILogger<AccountService>> _loggerMock = new();
    private readonly Mock<ITokenService> _tokenServiceMock = new();
    private readonly Mock<IUserRepository> _userRepoMock = new();

    [Fact]
    public void AddUserSuccessResult()
    {
        //Arrange
        var request = new AddUserDto("juan", "Hell0Wr0d!");
        var response = new User("juan", "This is my fake token");

        _accountServiceMock.Setup(p => p.AddUser(request)).ReturnsAsync(response);

        var accountController = new AccountController(_accountServiceMock.Object);

        //Act
        var result = accountController.Register(request);

        //Assert
        Assert.Equal(request.Username, result.Result.Value?.Username);
        Assert.Equal(response.Token, result.Result.Value?.Token);
    }

    [Fact]
    public void LoginSuccessResult()
    {
        //Arrange
        var request = new LoginDto("juan", "Hell0Wr0d!");
        var response = new User("juan", "This is my fake token");

        _accountServiceMock.Setup(p => p.Login(request)).ReturnsAsync(response);

        var accountController = new AccountController(_accountServiceMock.Object);

        //Act
        var result = accountController.Login(request);

        //Assert
        Assert.Equal(request.Username, result.Result.Value?.Username);
        Assert.Equal(response.Token, result.Result.Value?.Token);
    }

    [Fact]
    public void InvalidObjectCheck()
    {
        //Arrange
        var emptyUsername = new AddUserDto("", "Hell0Wr0d!");
        var emptyPassword = new AddUserDto("test", "");
        var nullPassword = new AddUserDto("test", null);
        var nullUsername = new AddUserDto(null, "Hell0Wr0d!");
        var nullObject = new AddUserDto(null, null);
        var emptyObject = new AddUserDto("", "");

        //Act
        var validateEmptyUsername =
            Validator.TryValidateObject(emptyUsername, new ValidationContext(emptyUsername, null, null), null, true);
        var validateEmptyPassword =
            Validator.TryValidateObject(emptyPassword, new ValidationContext(emptyPassword, null, null), null, true);
        var validateNullPassword =
            Validator.TryValidateObject(nullPassword, new ValidationContext(nullPassword, null, null), null, true);
        var validateNullUsername =
            Validator.TryValidateObject(nullUsername, new ValidationContext(nullUsername, null, null), null, true);
        var validateNullObject =
            Validator.TryValidateObject(nullObject, new ValidationContext(nullObject, null, null), null, true);
        var validateEmptyObject =
            Validator.TryValidateObject(emptyObject, new ValidationContext(emptyObject, null, null), null, true);

        //Assert
        Assert.False(validateEmptyUsername);
        Assert.False(validateEmptyPassword);
        Assert.False(validateNullPassword);
        Assert.False(validateNullUsername);
        Assert.False(validateNullObject);
        Assert.False(validateEmptyObject);
    }
}