using DiffPlex;
using DiffPlex.DiffBuilder;
using DiffPlex.DiffBuilder.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChangesDetector.model
{
    interface IDetector
    {
        public bool Detect(Webpage localCopy, Webpage remoteVersion);
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
        private void CompareSitemaps()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Method for comparing sources between local copy
        /// and remote version of single page from sitemap 
        /// </summary>
        /// <param name="localCopy"></param>
        /// <param name="remoteCopy"></param>
        private void CompareSources(string localCopy, string remoteCopy)
        {
            var diff = _inlineDiffBuilder.BuildDiffModel(localCopy, remoteCopy);

            foreach (var line in diff.Lines)
            {
                switch (line.Type)
                {
                    case ChangeType.Inserted:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("+++ ");
                        break;
                    case ChangeType.Deleted:
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write("--- ");
                        break;
                    default:
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write("  ");
                        break;
                }

                Console.WriteLine(line.Text);
            }
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
        public bool Detect(Webpage localCopy, Webpage remoteVersion)
        {
            CompareSources(localCopy.Components.First().SourceCode, remoteVersion.Components.Last().SourceCode);

            return true;
        }

        public void Notify()
        {
            throw new NotImplementedException();
        }
    }
}
