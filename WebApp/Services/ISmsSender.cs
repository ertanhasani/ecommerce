using System.Collections.Generic;
using System.Threading.Tasks;

namespace Indoor.Services;

public interface ISmsSender
{
    Task<string> SendSmsAsync(List<string> phoneNumber, List<string> message);
}