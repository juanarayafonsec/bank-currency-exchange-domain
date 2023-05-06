namespace Bank.Currency.Exchange.Application.Rates.Services;

public interface IRateService
{
    public Task<RateResponse> CurrencyExchange(ExchangeRequest request);
}