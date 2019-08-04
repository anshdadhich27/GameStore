using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameStore.Models
{
    public class AppSetings
    {
        public string Display_Name { get; set; } = string.Empty;
        public string Host_Name { get; set; } = string.Empty;
        public int Port_Number { get; set; } = 0;
        public bool EnableSSL { get; set; } = true;
        public string From { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string ReplyTo { get; set; } = string.Empty;

        public string Sms_User { get; set; } = string.Empty;
        public string Sms_Pwd { get; set; } = string.Empty;
        public string Sms_Sender { get; set; } = string.Empty;

    }
}
