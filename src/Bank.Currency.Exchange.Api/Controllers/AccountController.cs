using Bank.Currency.Exchange.Application.Interfaces;

namespace Bank.Currency.Exchange.Api.Controllers;

public class AccountController : BaseApiController
{
    private readonly IAccountService _accountService;
    private readonly ITokenService _tokenService;


    public AccountController(IAccountService accountService, ITokenService tokenService)
    {
        _accountService = accountService;
        _tokenService = tokenService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> Register([FromBody] AddUserDto addUser)
    {
        return await _accountService.AddUser(@addUser);
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login([FromBody] LoginDto login)
    {
        return await _accountService.Login(login);
    }
}