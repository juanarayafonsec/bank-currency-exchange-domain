namespace Bank.Currency.Exchange.Api.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("[controller]/api/v{version:apiVersion}/")]
public class BaseApiController : ControllerBase
{
}