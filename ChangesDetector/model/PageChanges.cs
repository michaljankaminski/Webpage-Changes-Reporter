﻿using DiffPlex.DiffBuilder.Model;
using System.Collections.Generic;

namespace ChangesDetector.model
{
    public class PageChanges
    {
        public int WebsiteId { get; set; }
        public IDictionary<string, SideBySideDiffModel> Changes { get; set; }
        public bool HasChanged = false;
    }
}
