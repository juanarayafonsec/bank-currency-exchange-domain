using System.Security.Cryptography;
using System.Text;
using Bank.Currency.Exchange.Application.Exceptions;
using Bank.Currency.Exchange.Domain.DTOs;
using Bank.Currency.Exchange.Domain.Models;
using Bank.Currency.Exchange.Domain.Repositories;
using Bank.Currency.Exchange.Domain.Services;
using Microsoft.Extensions.Logging;

namespace Bank.Currency.Exchange.Application.Services;

public class AccountService : IAccountService
{
    private readonly ILogger<AccountService> _logger;
    private readonly ITokenService _tokenService;
    private readonly IUserRepository _userRepository;

    public AccountService(ITokenService tokenService, IUserRepository userRepository, ILogger<AccountService> logger)
    {
        _tokenService = tokenService;
        _userRepository = userRepository;
        _logger = logger;
    }

    public async Task<UserDto> AddUser(AddUserDto newUser)
    {
        if (await _userRepository.GetUserAsync(newUser.Username) is not null)
            throw new RegistrationException("User is taken");

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

        if (user is null) throw new LoginException("Invalid user or password");

        using var hmac = new HMACSHA512(user.PasswordSalt);

        var computeHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(login.Password));

        for (var i = 0; i < computeHash.Length; i++)
            if (computeHash[i] != user.PasswordHash[i])
                throw new LoginException("Invalid user or password");

        _logger.LogInformation("User {user} logged in successfully on {datetime}", login.Username, DateTime.Now);

        return new UserDto
        {
            Username = user.UserName,
            Token = _tokenService.CreateToke(user)
        };
    }
}