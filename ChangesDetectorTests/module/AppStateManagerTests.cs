using ChangesDetector.model.state;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using ChangesDetector.module;

namespace ChangesDetectorTests.module
{
    [TestClass()]
    public class AppStateManagerTests
    {
        private readonly AppStateManager _appStateManager;

        public AppStateManagerTests()
        {
            _appStateManager = new AppStateManager();
        }

        [TestMethod()]
        public void SaveCurrentStateTest()
        {
            var page1 = new SavedWebpage
            {
                Url = "www.wp.pl",
                LastUpdate = DateTime.Parse("2001-01-13T23:00:00")
            };

            var page2 = new SavedWebpage
            {
                Url = "wwww.interia.pl",
                UpdateFrequency = 30
            };

            var state = new AppStateConfiguration();
            state.SavedWebpages = new List<SavedWebpage>
            {
                page1,
                page2
            };

            _appStateManager.SaveCurrentState(state);
            Assert.AreEqual(_appStateManager.GetState().SavedWebpages.Count, 2);
        }
    }
}