using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Bank.Currency.Exchange.Api.Services;

public class AccountService : IAccountService
{
    private readonly DataContext _dataContext;

    public AccountService(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public async Task<AppUser> AddUser(AddUserDto newUser)
    {
        if (await UserExist(newUser.Username).ConfigureAwait(false))
            throw new Exception("User is taken");
        
        using var hmac = new HMACSHA512();

        var user = new AppUser
        {
            UserName = newUser.Username.ToLower(),
            PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(newUser.Password)),
            PasswordSalt = hmac.Key
        };

        _dataContext.Users.Add(user);
        await _dataContext.SaveChangesAsync();

        return user;
    }

    public async Task<AppUser> Login(LoginDto login)
    {
        var user = await _dataContext.Users.FirstOrDefaultAsync(x => x.UserName == login.Username.ToLower());

        if (user is null) throw new Exception("Invalid user or password");
        
        using var hmac = new HMACSHA512(user.PasswordSalt);

        var computeHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(login.Password));
        for (int i = 0; i < computeHash.Length; i++)
        {
            if(computeHash[i] != user.PasswordHash[i]) throw new Exception("Invalid user or password");
        }

        return user;

    }

    private async Task<bool> UserExist(string username)
    {
        return await _dataContext.Users.AnyAsync(x => x.UserName == username.ToLower());
    }
}