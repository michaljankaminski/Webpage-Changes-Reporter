using ChangesDetector.model;
using ChangesDetector.model.state;
using ChangesDetector.service;
using DiffPlex.DiffBuilder.Model;
using System;
using System.Collections.Generic;

namespace ChangesDetector.module
{
    interface IChangesChecker
    {
        IDictionary<string, SideBySideDiffModel> Check(SavedWebpage webpage, Uri url);
    }
    class ChangesChecker : IChangesChecker
    {
        private readonly IDownloader _downloader;
        private readonly IDetector _detector;
        private readonly IFileStorage _fileStorage;

        public ChangesChecker(IDownloader downloader, MailConfiguration mailConfiguration)
        {
            _downloader = downloader;
            _detector = new Detector(mailConfiguration);
            _fileStorage = new FileStorage();
        }

        private Webpage ConvertToWebpage(SavedWebpage webpage)
        {
            List<WebpageComponent> webpageComponents = new List<WebpageComponent>();
            var files = _fileStorage.GetStorageFileByKey(webpage.Id);

            foreach (var file in files.FileNames)
            {
                webpageComponents.Add(new WebpageComponent
                {
                    AbsolutePath = file,
                    SourceCode = _fileStorage.GetFileContent(files.Key, file)
                });
            }

            return new Webpage
            {
                WebpageName = webpage.Name,
                WebpageUrl = webpage.Url,
                Components = webpageComponents,
            };
        }

        public IDictionary<string, SideBySideDiffModel> Check(SavedWebpage webpage, Uri url)
        {
            var tempWebpage = _downloader.Download(url, String.Empty, true);
            var originalWebpage = ConvertToWebpage(webpage);

            var diff = _detector.Detect(originalWebpage, tempWebpage);

            return diff;
        }
    }
}
