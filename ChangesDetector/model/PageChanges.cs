using DiffPlex.DiffBuilder.Model;

namespace ChangesDetector.model
{
    public class PageChanges
    {
        public DiffPaneModel Changes { get; set; }
        public bool HasChanged = false;
    }
}
