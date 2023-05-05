using Microsoft.EntityFrameworkCore;

namespace Bank.Currency.Exchange.Api.Extensions;

public static class ApplicationServicesExtension
{
    public static void AddServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<DataContext>(opt =>
        {
            opt.UseSqlServer(config.GetConnectionString("DefaultConnection"));
        });
    }
}