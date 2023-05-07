namespace Bank.Currency.Exchange.Api.Controllers;

public class AccountController : BaseApiController
{
    private readonly IAccountService _accountService;

    public AccountController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<User>> Register([FromBody] AddUserDto addUser)
    {
        return await _accountService.AddUser(addUser);
    }

    [HttpPost("login")]
    public async Task<ActionResult<User>> Login([FromBody] LoginDto login)
    {
        return await _accountService.Login(login);
    }
}