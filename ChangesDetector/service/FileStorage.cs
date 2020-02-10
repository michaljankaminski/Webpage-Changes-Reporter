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
        StorageFile GetStorageFileByKey(int key);
        bool AddNewStorage(string path);
        bool RemoveStorage(int key);
        int GetNumberOfStorages();
        int CreateNewStorage(string pageName);
        string GetFileContent(int key, string componentName);
        void CreateFileWithContent(int key, string componentName, string content);
        string CleanFileName(string fileName);
        string GetAppStateFile();
        bool SaveAppStateFile(string content);
        string CreateTempFolder();
        void CleanTempFolder();
    }

    public class FileStorage : IFileStorage
    {
        private string AppDataPath { get; set; }
        private string PagesPath { get; set; }
        private string AppStatePath { get; set; }
        private string TempPath { get; set; }
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
            TempPath = Path.Combine(AppDataPath, "Temp\\");
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
                throw;
            }
            catch (Exception ex)
            {
                throw;
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

            if (!storage.CheckIfFileNameExist(componentName))
            {
                throw new Exception("Such component does not exist");
            }

            var content = File.ReadAllText(StoredFiles[key].Path + "\\" + componentName);
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
            path.FileNames.Add(componentName);
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

        public string CreateTempFolder()
        {
            var guid = new Guid();
            var newPath = TempPath + guid.ToString() + "\\";
            Directory.CreateDirectory(newPath);
            return newPath;
        }

        public void CleanTempFolder()
        {
            DirectoryInfo di = new DirectoryInfo(TempPath);

            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }
            foreach (DirectoryInfo dir in di.GetDirectories())
            {
                dir.Delete(true);
            }
        }

        private void AddLocalFiles()
        {
            foreach (var storage in Directory.GetDirectories(PagesPath))
            {
                AddNewStorage(storage);
                DirectoryInfo di = new DirectoryInfo(storage);

                foreach (FileInfo file in di.GetFiles())
                {
                    StoredFiles[CurrentIndex - 1].FileNames.Add(file.Name);
                }
            }
        }

        public StorageFile GetStorageFileByKey(int key)
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
