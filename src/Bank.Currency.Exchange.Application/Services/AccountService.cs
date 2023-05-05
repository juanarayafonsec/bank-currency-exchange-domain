using System.Security.Cryptography;
using System.Text;
using Bank.Currency.Exchange.Application.Interfaces;
using Bank.Currency.Exchange.Domain.DTOs;
using Bank.Currency.Exchange.Domain.Repositories;
using Bank.Currency.Exchange.Domain.Services;
using Bank.Currency.Exchange.Domain.Models;

namespace Bank.Currency.Exchange.Application.Services;

public class AccountService : IAccountService
{
    private readonly ITokenService _tokenService;
    private readonly IUserRepository _userRepository;

    public AccountService(ITokenService tokenService, IUserRepository userRepository)
    {
        _tokenService = tokenService;
        _userRepository = userRepository;
    }

    public async Task<UserDto> AddUser(AddUserDto newUser)
    {
        if (await _userRepository.GetUserAsync(newUser.Username) is not null)
            throw new Exception("User is taken");

        using var hmac = new HMACSHA512();

        var user = new AppUser
        {
            UserName = newUser.Username.ToLower(),
            PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(newUser.Password)),
            PasswordSalt = hmac.Key
        };

        await _userRepository.AddUserAsync(user);

        return new UserDto
        {
            Username = user.UserName,
            Token = _tokenService.CreateToke(user)
        };
    }

    public async Task<UserDto> Login(LoginDto login)
    {
        var user = await _userRepository.GetUserAsync(login.Username);

        if (user is null) throw new Exception("Invalid user or password");

        using var hmac = new HMACSHA512(user.PasswordSalt);

        var computeHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(login.Password));
        
        for (var i = 0; i < computeHash.Length; i++)
            if (computeHash[i] != user.PasswordHash[i])
                throw new Exception("Invalid user or password");

        return new UserDto
        {
            Username = user.UserName,
            Token = _tokenService.CreateToke(user)
        };
    }
}