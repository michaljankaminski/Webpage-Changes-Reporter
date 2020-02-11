using DiffPlex;
using DiffPlex.DiffBuilder;
using DiffPlex.DiffBuilder.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using ChangesDetector.model;

namespace ChangesDetector.module
{
    interface IDetector
    {
        public IDictionary<string, SideBySideDiffModel> Detect(Webpage localCopy, Webpage remoteVersion);
    }
    class Detector : IDetector
    {
        private readonly IEmailReporter _emailReporter;
        private readonly IDiffer _differ;
        private readonly ISideBySideDiffBuilder _sideBySideDiffBuilder;
        public Detector(MailConfiguration mailConfiguration)
        {
            _emailReporter = new EmailReporter(mailConfiguration);
            _differ = new Differ();
            _sideBySideDiffBuilder = new SideBySideDiffBuilder(_differ);
        }
        /// <summary>
        /// First step is comparing the sitemaps
        /// overall number of sites, names, etc.
        /// </summary>
        private void CompareSitemaps<T>(IEnumerable<T> localCopy, IEnumerable<T> remoteCopy)
            where T : IComparable
        {
            var result = localCopy
                .Concat(remoteCopy)
                .Except(localCopy.Intersect(remoteCopy))
                .Select(a => new
                {
                    Value = a,
                    List = localCopy.Any(c => c.Equals(a)) ? "Local copy" : "Remote version"
                });

            foreach (var d in result)
            {
                Console.WriteLine("Item: '{0}' found on: '{1}'", d.Value, d.List);
            }
        }
        /// <summary>
        /// Method for comparing sources between local copy
        /// and remote version of single page from sitemap 
        /// </summary>
        /// <param name="localCopy"></param>
        /// <param name="remoteCopy"></param>
        private SideBySideDiffModel CompareSources(string localCopy, string remoteCopy)
        {
            var diff = _sideBySideDiffBuilder.BuildDiffModel(localCopy, remoteCopy);

            return diff;
        }
        /// <summary>
        /// Method for comparing the http headers obtained
        /// after http requests. 
        /// </summary>
        private void CompareHeaders()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Single method responsible for detecting
        /// changes between to complete versions of webpage
        /// </summary>
        /// <returns></returns>
        public IDictionary<string, SideBySideDiffModel> Detect(Webpage localCopy, Webpage remoteVersion)
        {
            //CompareSitemaps(localCopy.Sitemap, remoteVersion.Sitemap);
            var diff = CompareSources(localCopy.Components.First().SourceCode, remoteVersion.Components.Last().SourceCode);
            IDictionary<string, SideBySideDiffModel> diffPaneModels = new Dictionary<string, SideBySideDiffModel>();

            foreach (var component in localCopy.Components)
            {
                var remoteComponent = remoteVersion.Components.Where(c => c.AbsolutePath.Equals(component.AbsolutePath)).FirstOrDefault();
                if (remoteComponent != null)
                {
                    var difference = CompareSources(component.SourceCode, remoteComponent.SourceCode);
                    var diff2 = difference.NewText.Lines.Where(l => l.Type != ChangeType.Unchanged);
                    if (difference.NewText.Lines.Where(l => l.Type != ChangeType.Unchanged).Count() > 0)
                    {
                        diffPaneModels.Add(new KeyValuePair<string, SideBySideDiffModel>
                        (
                           component.AbsolutePath,
                             difference
                        ));
                        Notify(localCopy.WebpageName, localCopy.WebpageUrl);
                    }

                }
            }
            return diffPaneModels;
        }

        private void Notify(string name, string url)
        {
            string title = "Changes found: " + name;
            string body = String.Format("Witaj <br/> Wykryto zmiany na stronie {0}, <br /> Adres url: {1} <br /> Wygenerowano: {2}", name, url, DateTime.Now);
            List<string> rec = new List<string> { "m.ferfet@ponowemu.pl" };
            _emailReporter.SendMail(title, body, rec);
        }
    }
}
