namespace Bank.Currency.Exchange.Domain.DTOs;

public class AddUserDto
{
    public AddUserDto(string username, string password)
    {
        Username = username;
        Password = password;
    }

    [Required(AllowEmptyStrings = false)] public string Username { get; set; }
    [Required(AllowEmptyStrings = false)] public string Password { get; set; }
}