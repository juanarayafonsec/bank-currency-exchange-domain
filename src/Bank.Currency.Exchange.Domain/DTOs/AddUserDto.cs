using System.ComponentModel.DataAnnotations;

namespace Bank.Currency.Exchange.Domain.DTOs;

public class AddUserDto
{
    [Required]
    public string Username { get; set; }
    [Required]
    public string Password { get; set; }
}