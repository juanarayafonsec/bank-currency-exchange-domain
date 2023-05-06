namespace Bank.Currency.Exchange.Application.Exceptions;

public class LoginException : ApplicationException
{
    private const string DefaultMessage = "Something went wrong during the login, please try later or contact support!";

    protected LoginException() : base(DefaultMessage)
    {
    }

    public LoginException(string message) : base(message)
    {
    }

    public LoginException(string message, Exception innerException) : base(message, innerException)
    {
    }
}