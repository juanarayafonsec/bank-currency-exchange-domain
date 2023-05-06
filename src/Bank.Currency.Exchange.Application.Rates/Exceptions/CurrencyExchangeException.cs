namespace Bank.Currency.Exchange.Application.Rates.Exceptions;

public class CurrencyExchangeException : ApplicationException
{
    private const string DefaultMessage =
        "Something went wrong during the currency exchange, please try later or contact the suppor team";

    protected CurrencyExchangeException() : base(DefaultMessage)
    {
    }

    public CurrencyExchangeException(string message) : base(message)
    {
    }

    public CurrencyExchangeException(string message, Exception innerException) : base(message, innerException)
    {
    }
}