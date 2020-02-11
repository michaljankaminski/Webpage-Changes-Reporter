using System.Collections.Generic;

namespace ChangesDetector.model
{
    public class Webpage
    {
        public string WebpageName { get; set; }
        public string WebpageUrl { get; set; }
        public IEnumerable<Header> Headers { get; set; }
        public IEnumerable<string> Sitemap { get; set; }
        public IList<WebpageComponent> Components { get; set; }
    }
}
