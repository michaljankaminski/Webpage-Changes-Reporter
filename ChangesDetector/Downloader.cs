using ChangesDetector.model;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ChangesDetector
{
    interface IDownloader
    {
        public void Download(Uri url);
    }
    class WebpageDownloader : IDownloader
    {       
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
        private IEnumerable<HtmlNode> GetSitemap(HtmlDocument mainPage)
        {
            foreach (HtmlNode link in mainPage.DocumentNode.SelectNodes("//a[@href]"))
                if (ValidateLink(link))
                    yield return link;
        }
        public void Download(Uri url)
        {
            HtmlWeb webPage = new HtmlWeb();
            var htmlDocument = webPage.Load(url);

            foreach (var singleNode in GetSitemap(htmlDocument))
            {
                var href = singleNode.GetAttributeValue("href", String.Empty);
                Console.WriteLine(href);
            }
        }
    }
}
