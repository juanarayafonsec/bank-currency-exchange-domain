namespace Bank.Currency.Exchange.Domain.Repositories;

public interface IUserRepository
{
    Task AddUserAsync(AppUser newUser);
    Task<AppUser> GetUserAsync(string name);
}