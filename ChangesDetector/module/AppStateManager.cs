using System;
using System.Collections.Generic;
using ChangesDetector.model.state;
using ChangesDetector.service;
using Newtonsoft.Json;

namespace ChangesDetector.module
{
    interface IAppStateManager
    {
        bool SaveCurrentState(AppStateConfiguration state = null);
        AppStateConfiguration GetState();
    }

    public class AppStateManager:IAppStateManager
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

        public bool SaveCurrentState(AppStateConfiguration state = null)
        {
            if (state == null)
                state = State;
            var stateJson = JsonConvert.SerializeObject(state,Formatting.Indented);
            _fileStorage.SaveAppStateFile(stateJson.ToString());
            State = state;
            return true;
        }

        private bool ReadState(string content)
        {
            try
            {
                State = JsonConvert.DeserializeObject<AppStateConfiguration>(content);
                return true;
            }
            catch (Exception e)
            {
                Logger.Instance.Log(e.ToString());
                throw;
            }

            return false;
        }

        public AppStateConfiguration GetState()
        {
            return State;
        }
    }
}
