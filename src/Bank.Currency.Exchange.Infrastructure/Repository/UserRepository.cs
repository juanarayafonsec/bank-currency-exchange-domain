using Bank.Currency.Exchange.Domain.Models;
using Bank.Currency.Exchange.Domain.Repositories;
using Bank.Currency.Exchange.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Bank.Currency.Exchange.Infrastructure.Repository;

public class UserRepository : IUserRepository
{
    private readonly DataContext _dataContext;

    public UserRepository(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public async Task AddUserAsync(AppUser newUser)
    {
        _dataContext.Users.Add(newUser);
        await _dataContext.SaveChangesAsync();
    }

    public async Task<AppUser?> GetUserAsync(string name)
    {
        return await _dataContext.Users.FirstOrDefaultAsync(x => x.UserName == name);
    }
}