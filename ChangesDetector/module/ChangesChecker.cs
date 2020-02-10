using System;
using System.Collections.Generic;
using System.Text;

namespace ChangesDetector.module
{
    interface IChangesChecker
    {
        
    }
    class ChangesChecker : IChangesChecker
    {
        private readonly IDownloader _downloader;
        private readonly IDetector _detector;

        public ChangesChecker(IDownloader downloader)
        {
            _downloader = downloader;
            _detector = new Detector();
        }
    }
}
