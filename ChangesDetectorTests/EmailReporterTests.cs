using ChangesDetector.model;
using ChangesDetector.module;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace ChangesDetectorTests
{
    [TestClass()]
    public class EmailReporterTests
    {
        private readonly EmailReporter _emailReporter;

        public EmailReporterTests()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("configs/appsettings.json")
                .Build();
            var mailConfig = configuration.GetSection("mail");

            var mailConfiguration = new MailConfiguration()
            {
                HostSmtp = mailConfig.GetSection("hostSmtp").Value,
                LoginSmtp = mailConfig.GetSection("loginSmtp").Value,
                MailFromSmtp = mailConfig.GetSection("mailFromSmtp").Value,
                PasswordSmtp = mailConfig.GetSection("passwordSmtp").Value,
                PortSmtp = Convert.ToInt32(mailConfig.GetSection("portSmtp").Value),
                SslSmtp = Convert.ToBoolean(mailConfig.GetSection("sslSmtp").Value)
            };
            _emailReporter = new EmailReporter(mailConfiguration);
        }

        [TestMethod()]
        public void SendMailTest()
        {
            string toAdr = "";
            var mails = new List<string>
            {
                toAdr
            };
            var res = _emailReporter.SendMail("Test", "This is test message", mails);

            Assert.IsTrue(res);
        }
    }
}