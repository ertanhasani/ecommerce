namespace WebApp.Data;

public class SenderSettings
{
    public int Id { get; set; }

    public string Server { get; set; }

    public int? PortNumber { get; set; }

    public string Token { get; set; }

    public string Key { get; set; }

    public string From { get; set; }


    public string Password { get; set; }

    public string Username { get; set; }

    public bool IsEmail { get; set; }

    public string To { get; set; }
}