using System.Collections.Generic;

namespace ChangesDetector.model
{
    public class StorageFile
    {
        public int Key { get; set; }
        public string Path { get; set; }
        public IList<string> FileNames { get; set; }

        public bool CheckIfFileNameExist(string filename)
        {
            foreach (var name in FileNames)
            {
                if (name == filename)
                    return true;
            }

            return false;
        }
    }
}
