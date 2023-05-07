using System.ComponentModel.DataAnnotations;

namespace Bank.Currency.Exchange.Application.Rates.Models;

public class ExchangeRequest
{
    public ExchangeRequest(string to, string from, decimal amount)
    {
        To = to;
        From = from;
        Amount = amount;
    }

    [MaxLength(3, ErrorMessage = "Currency type name is 3 characters")]
    [MinLength(3, ErrorMessage = "Currency type name is 3 characters")]
    [Required]
    public string To { get; set; }

    [MaxLength(3, ErrorMessage = "Currency type name is 3 characters")]
    [MinLength(3, ErrorMessage = "Currency type name is 3 characters")]
    [Required]
    public string From { get; set; }

    [Required] public decimal Amount { get; set; }
}