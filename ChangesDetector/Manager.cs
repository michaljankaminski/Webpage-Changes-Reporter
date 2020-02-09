using ChangesDetector.model;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChangesDetector
{
    public class Manager
    {
        public void Test()
        {
            Uri url = new Uri("http://ponowemu.pl");
            IDownloader downloader = new WebpageDownloader();
            var result = downloader.Download(url);
            var result2 = downloader.Download(new Uri("https://sikoraauxilium.com/"));
            IDetector detector = new Detector();
            detector.Detect(result, result2);


        }
    }
}
