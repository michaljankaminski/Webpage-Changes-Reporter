using ChangesDetector.model;
using System;

namespace ChangesDetector
{
    interface IDownloader
    {
        public Webpage Download(Uri url);
    }
    class WebpageDownloader : IDownloader
    {
        public Webpage Download(Uri url)
        {
            throw new NotImplementedException();
        }
    }
}
