namespace Bank.Currency.Exchange.Application.Exceptions;

public class RegistrationException : ApplicationException
{
    private const string DefaultMessage = "Something went wrong during the signup, please try later";

    protected RegistrationException() : base(DefaultMessage)
    {
    }

    public RegistrationException(string message) : base(message)
    {
    }

    public RegistrationException(string message, Exception innerException) : base(message, innerException)
    {
    }
}