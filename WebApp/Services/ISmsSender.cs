namespace WebApp.Services;

public interface ISmsSender
{
    Task<string> SendSmsAsync(List<string> phoneNumber, List<string> message);
}