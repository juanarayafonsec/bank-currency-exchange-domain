using Bank.Currency.Exchange.Application.Rates.Mapper;
using Bank.Currency.Exchange.Application.Services;
using Bank.Currency.Exchange.Domain.Configurations;
using Bank.Currency.Exchange.Domain.Repositories;
using Bank.Currency.Exchange.Infrastructure.Repository;
using Bank.Currency.Exchange.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

namespace Bank.Currency.Exchange.Api.Extensions;

public static class ApplicationServicesExtensions
{
    public static void AddServices(this IServiceCollection services, IConfiguration config)
    {
        services.Configure<JwtConfig>(config.GetSection(nameof(JwtConfig)));
        services.Configure<ExchangeApiConfig>(config.GetSection(nameof(ExchangeApiConfig)));
        services.AddAutoMapper(typeof(ExchangeProfile));
        services.AddMemoryCache();
        
        services.AddDbContext<DataContext>(opt =>
        {
            opt.UseSqlServer(config.GetConnectionString("DefaultConnection"));
        });

        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRateService, RateService>();

        services.AddApiVersioning(x =>
        {
            x.DefaultApiVersion = new ApiVersion(1, 0);
            x.AssumeDefaultVersionWhenUnspecified = true;
            x.ReportApiVersions = true;
        });

        services.AddHttpClient();
    }
}