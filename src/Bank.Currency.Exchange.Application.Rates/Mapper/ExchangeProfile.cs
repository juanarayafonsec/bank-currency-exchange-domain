using AutoMapper;

namespace Bank.Currency.Exchange.Application.Rates.Mapper;

public class ExchangeProfile : Profile
{
    public ExchangeProfile()
    {
        CreateMap<ExchangeResponse, RateResponse>().ConstructUsing(src =>
            new RateResponse(src.Query.Amount, src.Query.From, src.Query.To, src.Info.Rate, src.Result));
    }
}