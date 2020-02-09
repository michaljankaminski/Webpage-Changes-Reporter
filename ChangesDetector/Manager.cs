using ChangesDetector.model;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace ChangesDetector
{
    public class Manager
    {
        private readonly MailConfiguration _mailConfiguration;
        public Manager()
        {
            Config(out _mailConfiguration);
        }
        public void Config(out MailConfiguration mailConfiguration)
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("configs/appsettings.json")
                .Build();
            var mailConfig = configuration.GetSection("mail");

            mailConfiguration = new MailConfiguration()
            {
                HostSmtp = mailConfig.GetSection("hostSmtp").Value,
                LoginSmtp = mailConfig.GetSection("loginSmtp").Value,
                MailFromSmtp = mailConfig.GetSection("mailFromSmtp").Value,
                PasswordSmtp = mailConfig.GetSection("passwordSmtp").Value,
                PortSmtp = Convert.ToInt32(mailConfig.GetSection("portSmtp").Value),
                SslSmtp = Convert.ToBoolean(mailConfig.GetSection("sslSmtp").Value)
            };

        }
        public void Test()
        {
            Uri url = new Uri("http://ponowemu.pl");
            IDownloader downloader = new WebpageDownloader();
            var result = downloader.Download(url, false);
            var result2 = downloader.Download(new Uri("https://sikoraauxilium.com/"), false);
            IDetector detector = new Detector();
            detector.Detect(result, result2);


        }
        public void Test2()
        {
            for (int i = 0; i <= 10; i++)
            {
                Console.Write(".");
            }
        }
    }
}
