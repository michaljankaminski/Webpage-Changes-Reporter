using System.Collections.Generic;

namespace ChangesDetector.model
{
    class Webpage
    {
        public string WebpageUrl { get; set; }
        public IEnumerable<Header> Headers { get; set; }
        public IEnumerable<string> Sitemap { get; set; }
        public IList<WebpageComponent> Components { get; set; }
    }
}
