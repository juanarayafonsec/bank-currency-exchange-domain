using AutoMapper;
using Bank.Currency.Exchange.Application.Rates.Exceptions;
using Microsoft.Extensions.Caching.Memory;
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
    private IMemoryCache _cache;
    private readonly MemoryCacheEntryOptions _cacheEntryOptions;

    public RateService(IHttpClientFactory httpClientFactory, IMapper mapper, ILogger<RateService> logger,
        IOptions<ExchangeApiConfig> config, IMemoryCache cache)
    {
        _httpClientFactory = httpClientFactory;
        _mapper = mapper;
        _logger = logger;
        _cache = cache;
        _config = config.Value;

        _cacheEntryOptions = new MemoryCacheEntryOptions()
            .SetAbsoluteExpiration(TimeSpan.FromSeconds(1800)) //30 minutes
            .SetPriority(CacheItemPriority.Normal);
    }

    public async Task<RateResponse> CurrencyExchange(ExchangeRequest request, string username)
    {
        try
        {
            if (_cache.TryGetValue($"{username}-{request.From}-{request.To}", out ExchangeResponse rate))
            {
                var dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
                if ((DateTime.Now - dateTime.AddSeconds(rate.Info.Timestamp)).TotalMinutes < 30)
                    return _mapper.Map<RateResponse>(rate);
                _cache.Remove($"{username}-{request.From}-{request.To}");
            }

            var response =
                await HttpRequest($"{_config.BaseUrl}?to={request.To}&from={request.From}&amount={request.Amount}");
            if (!response.IsSuccessStatusCode)
                throw new CurrencyExchangeException("Something went wrong, please try later");

            var deserializedResponse =
                JsonConvert.DeserializeObject<ExchangeResponse>(await response.Content.ReadAsStringAsync());

            _cache.Set($"{username}-{request.From}-{request.To}", deserializedResponse, _cacheEntryOptions);

            return _mapper.Map<RateResponse>(deserializedResponse);
        }
        catch (CurrencyExchangeException e)
        {
            _logger.LogError(e, "Error during the exchange conversion");
            throw;
        }
    }

    private async Task<HttpResponseMessage> HttpRequest(string uri)
    {
        var httpClient = _httpClientFactory.CreateClient();
        httpClient.DefaultRequestHeaders.Add("apikey", _config.ApiKey);
        return await httpClient.GetAsync(new Uri(uri));
    }
}