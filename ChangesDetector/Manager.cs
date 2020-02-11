using ChangesDetector.model;
using Microsoft.Extensions.Configuration;
using System;
using ChangesDetector.module;
using System.Collections.Generic;
using ChangesDetector.model.state;
using System.Linq;

namespace ChangesDetector
{
    public class Manager
    {
        private readonly MailConfiguration _mailConfiguration;
        private readonly AppStateManager _appStateManager;
        private readonly IDownloader _downloader;
        private readonly IChangesChecker _changesChecker;

        public Manager()
        {
            Configure(out _mailConfiguration);

            _appStateManager = new AppStateManager();
            _downloader = new WebpageDownloader();
            _changesChecker = new ChangesChecker(_downloader,_mailConfiguration);

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

        public bool AddNewWebpageToReport(SavedWebpage webpage)
        {
            try
            {
                _appStateManager.AddSavedWebpage(webpage);
                _downloader.Download(new Uri(webpage.Url), webpage.Name, false);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
        public bool RemoveWebpage(int id)
        {
            if (_appStateManager.RemoveSavedWebpage(id))
                return true;
            else
                return false;
        }
        public void UpdateWebpage(int id)
        {
            _appStateManager.UpdateDate(id);
        }
        public PageChanges CheckIfWebpageHasChanged(int webpageId)
        {
            var webPages = _appStateManager.GetState();
            if (webPages != null)
            {
                var webPage = webPages.SavedWebpages.Where(w => w.Id == webpageId).SingleOrDefault();
                var diff = _changesChecker.Check(webPage, new Uri(webPage.Url));

                return new PageChanges
                {
                    Changes = diff,
                    HasChanged = true
                };
            }

            return new PageChanges
            {
                HasChanged = false,
                Changes = null
            };
        }
  
        public IEnumerable<SavedWebpage> GetWebpages()
        {
            var result = _appStateManager.GetState();
            return result.SavedWebpages;
        }


        public void Test()
        {
            //Uri url = new Uri("http://ponowemu.pl");
            //IDownloader downloader = new WebpageDownloader();
            //var result = downloader.Download(url, false);
            //var result2 = downloader.Download(new Uri("https://sikoraauxilium.com/"), false);
            //IDetector detector = new Detector();
            //detector.Detect(result, result2);
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
