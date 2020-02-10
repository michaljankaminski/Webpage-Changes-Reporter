using ChangesDetector.model;
using Microsoft.Extensions.Configuration;
using System;
using ChangesDetector.module;
using System.Collections.Generic;

namespace ChangesDetector
{
    public class Manager
    {
        private readonly MailConfiguration _mailConfiguration;
       
        public Manager()
        {
            Configure(out _mailConfiguration);
        }

        public void Configure(out MailConfiguration mailConfiguration)
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

        public bool AddNewWebpageToReport(Uri url)
        {
            IDownloader downloader = new WebpageDownloader();
            downloader.Download(url, false);

            return true;
        }

        public bool CheckIfWebpageHasChanged()
        {
            throw new NotImplementedException();
        }
        
        public IEnumerable<Webpage> GetWebpages()
        {
            return new List<Webpage>();
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
