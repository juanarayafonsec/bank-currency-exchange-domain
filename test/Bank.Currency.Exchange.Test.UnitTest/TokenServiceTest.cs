using Bank.Currency.Exchange.Domain.Configurations;
using Bank.Currency.Exchange.Domain.Models;
using Bank.Currency.Exchange.Infrastructure.Services;
using Microsoft.Extensions.Options;

namespace Bank.Currency.Exchange.Test.UnitTest;

public class TokenServiceTest
{
    private readonly Mock<IOptions<JwtConfig>> _configMock = new();
    private readonly TokenService _tokenService;

    public TokenServiceTest()
    {
        _configMock.Setup(p => p.Value)
            .Returns(() => new JwtConfig { TokenKey = "kjdssudhuh8IJ299iH67*i2ihjiaasdlak" });
        _tokenService = new TokenService(_configMock.Object);
    }

    [Fact]
    public void JwtCreation()
    {
        //Arrange
        var user = new AppUser { UserName = "Test" };

        //Act
        var result = _tokenService.CreateToke(user);

        //Assert
        Assert.NotNull(result);
    }

    [Fact]
    public void InvalidObjectCheck()
    {
        //Arrange
        var emptyPassword = new AppUser { UserName = "" };
        var nullPassword = new AppUser { UserName = null };

        //Act
        var validateEmptyPassword =
            Validator.TryValidateObject(emptyPassword, new ValidationContext(emptyPassword, null, null), null, true);
        var validateNullPassword =
            Validator.TryValidateObject(nullPassword, new ValidationContext(nullPassword, null, null), null, true);

        //Assert
        Assert.False(validateEmptyPassword);
        Assert.False(validateNullPassword);
    }
}