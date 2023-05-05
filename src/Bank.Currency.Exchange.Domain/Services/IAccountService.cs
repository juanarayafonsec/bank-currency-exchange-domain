namespace Bank.Currency.Exchange.Domain.Services;

public interface IAccountService
{
    public Task<AppUser> AddUser(AddUserDto newUser);
    public Task<AppUser> Login(LoginDto login);
}