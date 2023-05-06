namespace Bank.Currency.Exchange.Domain.Models;

public class RateResponse
{
    public RateResponse(decimal amount, string from, string to, decimal rate, decimal result)
    {
        Amount = amount;
        From = from;
        To = to;
        Rate = rate;
        Result = result;
    }

    public RateResponse()
    {
    }

    public decimal Amount { get; set; }
    public string From { get; set; }
    public string To { get; set; }
    public decimal Rate { get; set; }
    public decimal Result { get; set; }
}