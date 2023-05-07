namespace Bank.Currency.Exchange.Domain.DTOs;

public class User
{
    public User(string username, string token)
    {
        Username = username;
        Token = token;
    }

    public string Username { get; set; }
    public string Token { get; set; }
}