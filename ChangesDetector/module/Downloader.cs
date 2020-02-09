using ChangesDetector.model;
using ChangesDetector.service;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ChangesDetector.module
{
    interface IDownloader
    {
        public Webpage Download(Uri url, bool remote);
    }
    class WebpageDownloader : IDownloader
    {
        private readonly HtmlWeb _webBrowser;
        private readonly IFileStorage _fileStorage;

        public WebpageDownloader()
        {
            _webBrowser = new HtmlWeb();
            _fileStorage = new FileStorage();
        }
        /// <summary>
        /// Method used for validating the url, whether it should
        /// be classified as proper sitemap element
        /// </summary>
        /// <remarks>
        /// Extend this checker 
        /// </remarks>
        /// <param name="link">Single node - candidate for sitemap</param>
        /// <returns>T/F</returns>
        private bool ValidateLink(HtmlNode link)
        {
            string pattern = @"[A-z0-9]*.(html|htm|php)";
            if (link.Attributes["href"].Value != Target._blank.ToString()
                && Regex.IsMatch(link.Attributes["href"].Value, pattern))
                return true;
            else
                return false;
        }
        private IEnumerable<string> GetSiteMap(HtmlDocument mainPage)
        {
            IList<HtmlNode> siteMap = new List<HtmlNode>();

            foreach (HtmlNode link in mainPage.DocumentNode.SelectNodes("//a[@href]"))
                if (ValidateLink(link))
                    siteMap.Add(link);

            return siteMap.Select(n => n.Attributes["href"].Value).Distinct();
        }
        private IEnumerable<WebpageComponent> GetWebpageComponents(Uri baseUrl, IEnumerable<string> siteMap)
        {
            foreach (var singleLink in siteMap)
            {
                var subpageUri = new Uri(baseUrl, singleLink);
                var subpageDocument = _webBrowser.Load(subpageUri);

                yield return new WebpageComponent
                {
                    SourceCode = subpageDocument.Text,
                    AbsolutePath = subpageUri
                };
            }
        }

        private void SaveWebpage(Webpage webpage)
        {
            foreach (var component in webpage.Components)
                _fileStorage
                   .CreateFileWithContent(_fileStorage.CleanFileName(component.AbsolutePath.OriginalString), component.SourceCode);
        }

        public Webpage Download(Uri url, bool remote = true)
        {
            var htmlDocument = _webBrowser.Load(url);
            var siteMap = GetSiteMap(htmlDocument);

            IList<WebpageComponent> components = new List<WebpageComponent>
            {
                new WebpageComponent
                {
                    AbsolutePath = url,
                    SourceCode = htmlDocument.Text
                }
            };

            foreach (var component in GetWebpageComponents(url, siteMap))
                components.Add(component);

            var wp = new Webpage
            {
                Components = components,
                WebpageUrl = url.AbsoluteUri,
                Sitemap = siteMap
            };

            if (!remote)
                SaveWebpage(wp);

            return wp;
        }
    }
}
