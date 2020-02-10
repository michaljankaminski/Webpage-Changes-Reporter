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
        public IEnumerable<DiffPaneModel> Detect(Webpage localCopy, Webpage remoteVersion);
        public void Notify();
    }
    class Detector : IDetector
    {
        private readonly IDiffer _differ;
        private readonly IInlineDiffBuilder _inlineDiffBuilder;
        public Detector()
        {
            _differ = new Differ();
            _inlineDiffBuilder = new InlineDiffBuilder(_differ);
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
        private DiffPaneModel CompareSources(string localCopy, string remoteCopy)
        {
            var diff = _inlineDiffBuilder.BuildDiffModel(localCopy, remoteCopy);

            return diff;
            //foreach (var line in diff.Lines)
            //{
            //    switch (line.Type)
            //    {
            //        case ChangeType.Inserted:
            //            Console.ForegroundColor = ConsoleColor.Green;
            //            Console.Write("+++ ");
            //            break;
            //        case ChangeType.Deleted:
            //            Console.ForegroundColor = ConsoleColor.Red;
            //            Console.Write("--- ");
            //            break;
            //        default:
            //            Console.ForegroundColor = ConsoleColor.Yellow;
            //            Console.Write("  ");
            //            break;
            //    }
            //    Console.WriteLine(line.Text);
            //}
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
        public IEnumerable<DiffPaneModel> Detect(Webpage localCopy, Webpage remoteVersion)
        {
            //CompareSitemaps(localCopy.Sitemap, remoteVersion.Sitemap);
            var diff = CompareSources(localCopy.Components.First().SourceCode, remoteVersion.Components.Last().SourceCode);
            List<DiffPaneModel> diffPaneModels = new List<DiffPaneModel>();

            foreach (var component in localCopy.Components)
            {
                var remoteComponent = remoteVersion.Components.Where(c => c.AbsolutePath == component.AbsolutePath).FirstOrDefault();
                if (remoteComponent != null)
                {
                    var difference = CompareSources(component.SourceCode, remoteComponent.SourceCode);
                    if (difference.Lines.Where(l => l.Type != ChangeType.Unchanged).Count() > 0)
                        diffPaneModels.Add(difference);
                }
            }


            return diffPaneModels;
        }

        public void Notify()
        {
            throw new NotImplementedException();
        }
    }
}
