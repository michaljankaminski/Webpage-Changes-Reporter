using System;
using System.Collections.Generic;
using System.Text;

namespace ChangesDetector.model
{
    class Webpage
    {
        public string WebpageUrl { get; set; }
        public IEnumerable<Header> Headers { get; set; }
        public IEnumerable<WebpageComponent> Components { get; set; }
    }
}
