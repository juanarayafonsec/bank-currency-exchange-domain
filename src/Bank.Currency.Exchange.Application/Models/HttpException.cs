namespace Bank.Currency.Exchange.Application.Models;

public class HttpException
{
    public HttpException(int statusCode, string message)
    {
        StatusCode = statusCode;
        Message = message;
    }

    public int StatusCode { get; set; }
    public string Message { get; set; }
}