using DiffPlex.DiffBuilder.Model;
using System.Collections.Generic;

namespace ChangesDetector.model
{
    public class PageChanges
    {
        public IEnumerable<DiffPaneModel> Changes { get; set; }
        public bool HasChanged = false;
    }
}
