using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using UsKirtasiye.Models;

namespace UsKirtasiye.Shared
{
    public static class EmailSender
    {
        public static async Task SendMail(EmailModel emailModel)
        {
            string apiKey = WebConfigurationManager.AppSettings["SendGridApiKey"];
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress(emailModel.FromEmailAdress);
            var subject = emailModel.Subject;
            var to = new EmailAddress(emailModel.ToEmailAdress);
            var htmlContent = emailModel.HtmlContent;
            var msg = MailHelper.CreateSingleEmail(from, to, subject, null, htmlContent);
            var response = await client.SendEmailAsync(msg).ConfigureAwait(false);
        }
    }
}