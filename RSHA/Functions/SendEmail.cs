using MimeKit;
using MailKit.Net.Smtp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RSHA.Models;
using Microsoft.Extensions.Configuration;
using System.Web;

namespace RSHA.Functions
{
    public class SendEmail
    {
        public void RequestNotification(Mechanics mechanic, ApplicationUser mechanicUser, ApplicationUser customerUser, Requests requestFromDb, string text, string password)
        {
            MimeMessage message = new MimeMessage();

            MailboxAddress from = new MailboxAddress(mechanic.Name, mechanicUser.Email);
            message.From.Add(from);

            MailboxAddress to = new MailboxAddress(requestFromDb.FirstName + " " + requestFromDb.LastName, customerUser.Email);
            message.To.Add(to);

            message.Subject = "Request scheduled on date " + requestFromDb.RequestScheduledDate + " was accepted";

            //BodyBuilder bodyBuilder = new BodyBuilder();
            //bodyBuilder.HtmlBody = "<h1>Hello World!</h1>";
            //bodyBuilder.TextBody = "Hello World!";
            //message.Body = bodyBuilder.ToMessageBody();
            message.Body = new TextPart("plain")
            {
                //Text = @"Hello, " + customerUser.LastName + " Your " + requestFromDb.ProblemTypes.Name + " request on the date " + requestFromDb.RequestScheduledDate + " was accepted by " + mechanic.Name + ".\r" + "Here's the messaged that was sent to " + mechanic.Name + ":\r" + requestFromDb.Message + ".\r\r" + "Have a pleasant day."
                Text = text
            };

            using (var client = new SmtpClient())
            {
                // For demo-purposes, accept all SSL certificates (in case the server supports STARTTLS)
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                client.Connect("smtp.gmail.com", 587, false);

                // Note: only needed if the SMTP server requires authentication
                client.Authenticate("rsha.noreply@gmail.com", password);

                client.Send(message);
                client.Disconnect(true);
            }
        }

        public void ConfirmEmail(string name, string email, string text, string password)
        {
            MimeMessage message = new MimeMessage();

            MailboxAddress from = new MailboxAddress("RSHA", "RSHA@gmail.com");
            message.From.Add(from);

            MailboxAddress to = new MailboxAddress(name, email);
            message.To.Add(to);

            message.Subject = "Email confirmation";

            BodyBuilder bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = text;
            //bodyBuilder.TextBody = "Hello World!";
            message.Body = bodyBuilder.ToMessageBody();
            //message.Body = new TextPart("plain")
            //{
            //    //Text = @"Hello, " + customerUser.LastName + " Your " + requestFromDb.ProblemTypes.Name + " request on the date " + requestFromDb.RequestScheduledDate + " was accepted by " + mechanic.Name + ".\r" + "Here's the messaged that was sent to " + mechanic.Name + ":\r" + requestFromDb.Message + ".\r\r" + "Have a pleasant day."
            //    Text = text
            //};

            using (var client = new SmtpClient())
            {
                // For demo-purposes, accept all SSL certificates (in case the server supports STARTTLS)
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                client.Connect("smtp.gmail.com", 587, false);

                // Note: only needed if the SMTP server requires authentication
                client.Authenticate("rsha.noreply@gmail.com", password);

                client.Send(message);
                client.Disconnect(true);
            }
        }


    }
}
