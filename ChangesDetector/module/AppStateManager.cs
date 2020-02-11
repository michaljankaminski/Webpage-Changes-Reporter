using System;
using System.Collections.Generic;
using System.Linq;
using ChangesDetector.model.state;
using ChangesDetector.service;
using Newtonsoft.Json;

namespace ChangesDetector.module
{
    interface IAppStateManager
    {
        bool SaveCurrentState(AppStateConfiguration state = null);
        AppStateConfiguration GetState();
        void AddSavedWebpage(SavedWebpage webpage);
        bool RemoveSavedWebpage(int index);
        void UpdateDate(int id);
    }

    public class AppStateManager : IAppStateManager
    {
        private readonly IFileStorage _fileStorage;
        private AppStateConfiguration State { get; set; }

        public AppStateManager()
        {
            _fileStorage = new FileStorage();
            var stateContent = _fileStorage.GetAppStateFile();
            if (stateContent == string.Empty)
            {
                State = new AppStateConfiguration
                {
                    SavedWebpages = new List<SavedWebpage>()
                };
                SaveCurrentState();
            }
            else
            {
                ReadState(stateContent);
            }
        }
        public void UpdateDate(int id)
        {
            var item = State.SavedWebpages.Where(s => s.Id == id).FirstOrDefault();
            if (item != null)
                item.LastUpdate = DateTime.Now;

            SaveCurrentState();
        }
        public bool SaveCurrentState(AppStateConfiguration state = null)
        {
            if (state == null)
                state = State;
            var stateJson = JsonConvert.SerializeObject(state, Formatting.Indented);
            _fileStorage.SaveAppStateFile(stateJson.ToString());
            State = state;
            return true;
        }

        private bool ReadState(string content)
        {
            try
            {
                State = JsonConvert.DeserializeObject<AppStateConfiguration>(content);
                SetIdInStates();
                return true;
            }
            catch (Exception e)
            {
                Logger.Instance.Log(e.ToString());
                throw;
            }

            return false;
        }

        public void AddSavedWebpage(SavedWebpage webpage)
        {
            State.SavedWebpages.Add(webpage);
            SaveCurrentState();
        } 

        public bool RemoveSavedWebpage(int index)
        {
            try
            {
                var item = State.SavedWebpages.Where(s => s.Id == index).SingleOrDefault();
                if (item != null)
                {
                    State.SavedWebpages.Remove(item);
                    SaveCurrentState();
                }


                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public AppStateConfiguration GetState()
        {
            return State;
        }

        private void SetIdInStates()
        {
            foreach (var storage in _fileStorage.GetStorages())
            {
                foreach (var wp in State.SavedWebpages)
                {
                    if (storage.Name == wp.Name)
                    {
                        wp.Id = storage.Key;
                        break;
                    }
                }   
            }
        }
    }
}
