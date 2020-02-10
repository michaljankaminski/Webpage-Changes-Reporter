using System.Collections.Generic;

namespace ChangesDetector.model
{
    public class StorageFile
    {
        public int Key { get; set; }
        public string Path { get; set; }
        public IList<string> FileNames { get; set; }
    }
}
