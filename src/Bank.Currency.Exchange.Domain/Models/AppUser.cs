namespace Bank.Currency.Exchange.Domain.Models;

public class AppUser
{
    public Guid Id { get; set; }
    [Required] public string UserName { get; set; }
    [Required] public byte[] PasswordHash { get; set; }
    [Required] public byte[] PasswordSalt { get; set; }
}