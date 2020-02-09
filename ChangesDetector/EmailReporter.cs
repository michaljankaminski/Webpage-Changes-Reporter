using ChangesDetector.interfaces;
using ChangesDetector.model;
using ChangesDetector.service;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Text;
using MimeKit.Text;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace ChangesDetector
{
    interface IEmailReporter: IReporter
    {

    }
    public class EmailReporter : IEmailReporter
    {
        private readonly MailConfiguration _configuration;

        public EmailReporter(MailConfiguration configuration)
        {
            _configuration = configuration;
        }

        public bool Report()
        {
            throw new NotImplementedException();
        }

        public bool SendMail(string subject, string body, IEnumerable<string> destinationMails)
        {
            try
            {
                using (SmtpClient client = new SmtpClient())
                {
                    var secured = _configuration.SslSmtp == true ? true : false;

                    client.Connect(_configuration.HostSmtp, _configuration.PortSmtp, secured);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    client.Timeout = 10000;

                    client.Authenticate(_configuration.LoginSmtp, _configuration.PasswordSmtp);

                    var message = new MimeMessage();
                    message.From.Add(new MailboxAddress(_configuration.MailFromSmtp));

                    message.Subject = subject;
                    message.Body = new TextPart(TextFormat.Html)
                    {
                        Text = body
                    };


                    foreach (var mail in destinationMails)
                    {
                        message.To.Add( new MailboxAddress(mail));
                    }

                    client.Send(message);

                    client.Disconnect(true);
                    return true;
                }
            }
            catch (Exception e)
            {
                Logger.Instance.Log(e.ToString());
                return false;
            }

        }

    }
}
