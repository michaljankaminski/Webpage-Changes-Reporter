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
        IList<StorageFile> GetFiles();
        bool AddNewFile(string path, bool copy = true);
        bool RemoveFile(int key);
        int GetNumberOfFiles();
        string GetFileContent(int key);
        int CreateFileWithContent(string fileName, string content);
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
            PagesPath = Path.Combine(AppDataPath, "Webpages/");
            Directory.CreateDirectory(PagesPath);
            AddLocalFiles();
        }

        public IList<StorageFile> GetFiles()
        {
            return StoredFiles;
        }

        public bool AddNewFile(string path, bool copy = true)
        {
            try
            {
                if (!File.Exists(path))
                    throw new FileNotFoundException();
                if (copy)
                    File.Copy(path, Path.Combine(PagesPath, Path.GetFileName(path)));
                StoredFiles.Add(new StorageFile
                {
                    Key = CurrentIndex,
                    Path = path
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

        public bool RemoveFile(int key)
        {
            try
            {
                if (StoredFiles.Where(x => x.Key == key).Count() == 0)
                {
                    throw new KeyNotFoundException();
                }
                StoredFiles.RemoveAll(x => x.Key == key);
                return true;
            }
            catch (KeyNotFoundException)
            {
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public int GetNumberOfFiles()
        {
            return CurrentIndex;
        }

        public string GetFileContent(int key)
        {
            if (StoredFiles.Where(x => x.Key == key).Count() == 0)
            {
                throw new KeyNotFoundException();
            }

            var content = File.ReadAllText(StoredFiles[key].Path);
            return content;
        }

        public int CreateFileWithContent(string fileName, string content)
        {
            int fileKey = CurrentIndex;
            File.WriteAllText(PagesPath + fileName, content);
            AddNewFile(PagesPath + fileName, false);
            return fileKey;
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
            foreach (var file in Directory.GetFiles(PagesPath))
            {
                AddNewFile(file, false);
            }
        }
    }
}
