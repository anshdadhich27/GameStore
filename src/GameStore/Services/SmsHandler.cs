using GameStore.Models;
using Microsoft.Extensions.Options;
using System.Net;
using System.Threading.Tasks;

namespace GameStore.Services
{

    public class SmsHandler : ISmsSender
    {
        public AppSetings _options { get; }
        public SmsHandler(IOptions<AppSetings> options)
        {
            _options = options.Value;
        }

        public Task SendSmsAsync(string number, string message)
        {
            using (WebClient client = new WebClient())
            {
                message = WebUtility.UrlEncode(message);
                string baseurl = string.Format("http://www.mshastra.com/sendurlcomma.aspx?user={0}&pwd={1}&senderid={2}&mobileno={3}&msgtext={4}&smstype=3", _options.Sms_User, _options.Sms_Pwd, _options.Sms_Sender, number, message);
                string s = client.DownloadString(baseurl);
            }
            return Task.FromResult(0);
        }
    }
}
