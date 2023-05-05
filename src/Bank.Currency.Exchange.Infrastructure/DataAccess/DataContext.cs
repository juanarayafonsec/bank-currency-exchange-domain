using Microsoft.EntityFrameworkCore;

namespace Bank.Currency.Exchange.Infrastructure.DataAccess;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<AppUser> Users { get; set; }
}