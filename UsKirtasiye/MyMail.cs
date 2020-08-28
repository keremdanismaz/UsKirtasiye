using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Services.Description;

namespace UsKirtasiye
{
    public class MyMail
    {
        public string ToMail { get; private set; }
        public string Subject { get; private set; }
        public string Body { get; private set; }
        private const string password = "5534423783";

        public MyMail(string _toMail, string _body, string _subject)
        {
            this.ToMail = _toMail;
            this.Subject = _subject;
            this.Body = _body;
        }

        public void SendMail()
        {
            MailMessage mail = new MailMessage()
            {
                From = new MailAddress("danismaz2000@gmail.com", "Kerem Danışmaz sana bir mail gönderdi.") // kim tarafından gönderildiği
            };
            mail.To.Add(this.ToMail);
            mail.Subject = this.Subject; // kime ne gönderlidiği
            mail.Body = this.Body;

            SmtpClient client = new SmtpClient()
            {
                Port = 587,
                Host = "smtp.gmail.com",
                EnableSsl = true,
            };
            client.Credentials = new System.Net.NetworkCredential("danismaz2000@gmail.com", password);
            client.Send(mail);
        }
    }
}