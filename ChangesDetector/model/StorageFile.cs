using System.Collections.Generic;

namespace ChangesDetector.model
{
    public class StorageFile
    {
        public int Key { get; set; }
        private string _path;

        public string Path
        {
            get { return _path; }
            set
            {
                _path = value;
                Name = System.IO.Path.GetFileName(_path);
            }
        }

        public string Name { get; set; }
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
