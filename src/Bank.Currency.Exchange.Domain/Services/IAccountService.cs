namespace Bank.Currency.Exchange.Domain.Services;

public interface IAccountService
{
    public Task<UserDto> AddUser(AddUserDto newUser);
    public Task<UserDto> Login(LoginDto login);
}