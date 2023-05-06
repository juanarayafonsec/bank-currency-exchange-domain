using AutoMapper;
using Bank.Currency.Exchange.Application.Rates.Exceptions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Bank.Currency.Exchange.Application.Rates.Services;

public class RateService : IRateService
{
    private readonly ExchangeApiConfig _config;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<RateService> _logger;
    private readonly IMapper _mapper;

    public RateService(IHttpClientFactory httpClientFactory, IMapper mapper, ILogger<RateService> logger,
        IOptions<ExchangeApiConfig> config)
    {
        _httpClientFactory = httpClientFactory;
        _mapper = mapper;
        _logger = logger;
        _config = config.Value;
    }

    public async Task<RateResponse> CurrencyExchange(ExchangeRequest request)
    {
        try
        {
            var httpClient = _httpClientFactory.CreateClient();

            httpClient.DefaultRequestHeaders.Add("apikey", _config.ApiKey);
            var response =
                await httpClient.GetAsync(
                    new Uri($"{_config.BaseUrl}?to={request.To}&from={request.From}&amount={request.Amount}"));

            if (!response.IsSuccessStatusCode)
                throw new CurrencyExchangeException("Something went wrong, please try later");


            var deserializedResponse =
                JsonConvert.DeserializeObject<ExchangeResponse>(await response.Content.ReadAsStringAsync());

            return _mapper.Map<RateResponse>(deserializedResponse);
        }
        catch (CurrencyExchangeException e)
        {
            _logger.LogError(e, "Error during the exchange conversion");
            throw;
        }
    }
}