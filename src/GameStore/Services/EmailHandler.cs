using Microsoft.Extensions.Options;
using GameStore.Models;
using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using GameStore.Models.Entity;
using GameStore.Models.DataProvider;
using System.Collections;
using FluentEmail.Razor;
using FluentEmail.Core;
using System.IO;

namespace GameStore.Services
{
    public class EmailHandler : IEmailSender
    {
        public AppSetings _options { get; }
        public EmailHandler(IOptions<AppSetings> options)
        {
            _options = options.Value;
        }
        
        

        public async Task<DbResult> SendEmailAsync(string name, string email, string subject, string message)
        {
            var result = new DbResult { Success = true };
            try
            {
                using (var mail = new MailMessage())
                {                    
                    mail.Body = message;
                    mail.IsBodyHtml = true;
                    mail.Subject = subject;
                    mail.Priority = MailPriority.High;
                    mail.To.Add(new MailAddress(email, name));
                    mail.From = new MailAddress(_options.From, _options.Display_Name);
                    using (var client = GetSmtpClient())
                    {
                        await client.SendMailAsync(mail);                        
                    }
                }
            }
            catch(Exception ex)
            {
                result.Success = false; result.Message = ex.Message;
                string info = string.Format("SendEmailAsync: Name:- {0}, Email:- {1}, Subject:- {2}", name, email, subject);
                ExceptionGateway.AddException(new ExceptionLog(ex, info));
            }
            return await Task.FromResult(result);
        }

        public async Task<DbResult> Send_Email_With_Attachement(string name, string email, string subject, string message, string file)
        {
            var result = new DbResult { Success = true };
            try
            {
                using (var mail = new MailMessage())
                {
                    mail.Body = message;
                    mail.IsBodyHtml = true;
                    mail.Subject = subject;
                    mail.Priority = MailPriority.High;
                    mail.To.Add(new MailAddress(email, name));
                    mail.From = new MailAddress(_options.From, _options.Display_Name);
                    mail.Attachments.Add(new Attachment(file));
                    using (var client = GetSmtpClient())
                    {
                        await client.SendMailAsync(mail);
                    }
                }
            }
            catch (Exception ex)
            {
                result.Success = false; result.Message = ex.Message;
                string info = string.Format("SendEmailAsync: Name:- {0}, Email:- {1}, Subject:- {2}", name, email, subject);
                ExceptionGateway.AddException(new ExceptionLog(ex, info));
            }
            return await Task.FromResult(result);
        }


        public string GetMailTemplate(dynamic obj, string template)
        {
            string htmlBody = string.Empty;
            try
            {
                Email.DefaultRenderer = new RazorRenderer();
                var template_path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "email-template", template);
                var testEmail = Email.From(_options.From).UsingTemplateFromFile(template_path, obj);
                htmlBody = testEmail.Data.Body;
            }
            catch (Exception ex)
            {
                string messgae = ex.Message;
                string info = string.Format("GetMailTemplate: Template:- {0}", template);
                ExceptionGateway.AddException(new ExceptionLog(ex, info));
            }
            return htmlBody;
        }

        private SmtpClient GetSmtpClient()
        {
            var client = new SmtpClient(_options.Host_Name, _options.Port_Number);
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential(_options.From, _options.Password);
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.EnableSsl = _options.EnableSSL;
            return client;
        }
    }
}
