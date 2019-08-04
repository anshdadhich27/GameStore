using GameStore.Models.DataProvider;
using System.Threading.Tasks;

namespace GameStore.Services
{
    public interface IEmailSender
    {
        Task<DbResult> Send_Email_With_Attachement(string name, string email, string subject, string message, string file);
        Task<DbResult> SendEmailAsync(string name, string email, string subject, string message);
        string GetMailTemplate(object obj, string Template);
    }
}
