namespace Bank.Currency.Exchange.Domain.Services;

public interface IAccountService
{
    public Task<User> AddUser(AddUserDto newUser);
    public Task<User> Login(LoginDto login);
}