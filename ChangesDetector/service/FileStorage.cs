using ChangesDetector.model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace ChangesDetector.service
{
    public interface IFileStorage
    {
        IList<StorageFile> GetStorages();
        bool AddNewStorage(string path);
        bool RemoveStorage(int key);
        int GetNumberOfStorages();
        int CreateNewStorage(string pageName);
        string GetFileContent(int key, string componentName);
        void CreateFileWithContent(int key, string componentName, string content);
        string CleanFileName(string fileName);
        string GetAppStateFile();
        bool SaveAppStateFile(string content);
    }
    public class FileStorage : IFileStorage
    {
        private string AppDataPath { get; set; }
        private string PagesPath { get; set; }
        private string AppStatePath { get; set; }
        private List<StorageFile> StoredFiles { get; set; } = new List<StorageFile>();
        private int CurrentIndex { get; set; } = 0;

        public FileStorage()
        {
            AppDataPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "PoNowemu",
                "ChangesDetector");
            AppStatePath = AppDataPath + "\\AppState.json";
            PagesPath = Path.Combine(AppDataPath, "Webpages\\");
            Directory.CreateDirectory(PagesPath);
            AddLocalFiles();
        }

        public IList<StorageFile> GetStorages()
        {
            return StoredFiles;
        }

        public bool AddNewStorage(string path)
        {
            try
            {
                Directory.CreateDirectory(path);
                StoredFiles.Add(new StorageFile
                {
                    Key = CurrentIndex,
                    Path = path,
                    FileNames = new List<string>()
                });

                CurrentIndex++;
                return true;
            }
            catch (FileNotFoundException ex)
            {
                Logger.Instance.Log(ex.ToString());
                return false;
            }
            catch (Exception ex)
            {
                Logger.Instance.Log(ex.ToString());
                return false;
            }
        }

        public bool RemoveStorage(int key)
        {
            try
            {
                if (StoredFiles.Where(x => x.Key == key).Count() == 0)
                {
                    throw new KeyNotFoundException();
                }
                var storage = GetStorageFileByKey(key);
                StoredFiles.RemoveAll(x => x.Key == key);
                Directory.Delete(storage.Path, true);
                return true;
            }
            catch (KeyNotFoundException e)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int GetNumberOfStorages()
        {
            return CurrentIndex;
        }

        public string GetFileContent(int key, string componentName)
        {
            var storage = GetStorageFileByKey(key);
            if (storage == null)
            {
                throw new KeyNotFoundException();
            }

            var comp = storage.CheckIfFileNameExist(componentName);
            if (comp == null)
            {
                throw new Exception("Such component does not exist");
            }

            var content = File.ReadAllText(StoredFiles[key].Path + "\\componentName");
            return content;
        }

        public int CreateNewStorage(string pageName)
        {
            int fileKey = CurrentIndex;
            AddNewStorage(Path.Combine(PagesPath, pageName));
            return fileKey;
        }

        public void CreateFileWithContent(int key, string componentName, string content)
        {
            var path = GetStorageFileByKey(key);
            File.WriteAllText(path.Path + "\\" + componentName, content);
        }

        public string CleanFileName(string filename)
        {
            Regex pattern = new Regex(@"[:\/.]");
            return pattern.Replace(filename, "_");
        }

        public string GetAppStateFile()
        {
            if (!File.Exists(AppStatePath))
            {
                var str = File.Create(AppStatePath);
                str.Dispose();
                return string.Empty;
            }
            return File.ReadAllText(AppStatePath);
        }

        public bool SaveAppStateFile(string content)
        {
            try
            {
                File.WriteAllText(AppStatePath, content);
                return true;
            }
            catch (Exception e)
            {
                throw;
            }

        }

        private void AddLocalFiles()
        {
            foreach (var file in Directory.GetDirectories(PagesPath))
            {
                AddNewStorage(file);
            }
        }

        private StorageFile GetStorageFileByKey(int key)
        {
            foreach (var storage in StoredFiles)
            {
                if (storage.Key == key)
                    return storage;
            }

            return null;
        }
    }
}
