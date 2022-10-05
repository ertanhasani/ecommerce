// using System.Collections.Generic;
// using System.Threading.Tasks;
// using SmsIrRestfulNetStandard.Classes;
// using SmsIrRestfulNetStandard.Operations;
//
// namespace WebApp.Services;
//
// public class SmsSender : ISmsSender
// {
//     private readonly MessageSend _messageSend = new MessageSend();
//
//     private readonly string _token;
//     private readonly string _lineNumber;
//
//     public SmsSender(string userApiKey, string secretKey, string lineNumber)
//     {
//         _token = new Token().GetToken(userApiKey, secretKey);
//         _lineNumber = lineNumber;
//     }
//
//     public string SendSmsWithResponse(List<string> phoneNumber, List<string> message)
//     {
//         MessageSendObject messageSend = new MessageSendObject
//         {
//             Messages = message.ToArray(),
//             MobileNumbers = phoneNumber.ToArray(),
//             LineNumber = _lineNumber,
//             SendDateTime = null,
//             CanContinueInCaseOfError = false
//         };
//
//         var res = _messageSend.Send(_token, messageSend);
//
//         return res.Message;
//     }
//
//     public Task<string> SendSmsAsync(List<string> phoneNumber, List<string> message)
//     {
//         MessageSendObject messageSend = new MessageSendObject
//         {
//             Messages = message.ToArray(),
//             MobileNumbers = phoneNumber.ToArray(),
//             LineNumber = _lineNumber,
//             SendDateTime = null,
//             CanContinueInCaseOfError = false
//         };
//
//         var res = _messageSend.Send(_token, messageSend);
//
//         return Task.FromResult(res.Message);
//     }
// }

