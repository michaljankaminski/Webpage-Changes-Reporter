using System;
using System.Net.Mail;
using ChangesDetector.interfaces;
using ChangesDetector.model;

namespace ChangesDetector
{
    interface IEmailReporter: IReporter
    {

    }
    class EmailReporter : IEmailReporter
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

        private bool SendMail(string subject, string body)
        {
            SmtpClient client = new SmtpClient(_configuration.HostSmtp, _configuration.PortSmtp);

            if (_configuration.SslSmtp)
                client.EnableSsl = true;
            else
            {
                client.EnableSsl = false;
            }

            using (MailMessage message = new MailMessage("", ""))
            {
                message.Body = body;
                message.BodyEncoding = System.Text.Encoding.UTF8;
                message.Subject = subject;
                message.SubjectEncoding = System.Text.Encoding.UTF8;
                string userState = "test message1";
                client.SendAsync(message, userState);
                Console.WriteLine("Sending message... press c to cancel mail. Press any other key to exit.");
                string answer = Console.ReadLine();
                // If the user canceled the send, and mail hasn't been sent yet,
                // then cancel the pending operation.
                Console.WriteLine("Goodbye.");
            }

            return false;

        }
    }
}
