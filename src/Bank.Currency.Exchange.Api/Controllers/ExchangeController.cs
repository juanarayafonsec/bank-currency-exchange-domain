using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Bank.Currency.Exchange.Api.Controllers;

[Authorize]
public class ExchangeController : BaseApiController
{
    private readonly DataContext _dataContext;

    public ExchangeController(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    [AllowAnonymous]
    [HttpGet("get-users")]
    public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()
    {
        var users = await _dataContext.Users.ToListAsync();
        return users;
    }

    [HttpGet("get-user/{id}")]
    public async Task<AppUser> GetUser(Guid id)
    {
        var users = await _dataContext.Users.FindAsync(id);
        return users;
    }
}