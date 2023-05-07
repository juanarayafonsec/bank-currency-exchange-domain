using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace Bank.Currency.Exchange.Api.Controllers;

[Authorize]
public class ExchangeController : BaseApiController
{
    private readonly IRateService _rateService;

    public ExchangeController(IRateService rateService)
    {
        _rateService = rateService;
    }

    [HttpGet]
    public async Task<ActionResult<RateResponse>> Exchange([FromBody] ExchangeRequest request)
    {
        var users = await _rateService.CurrencyExchange(request, User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        return users;
    }
}