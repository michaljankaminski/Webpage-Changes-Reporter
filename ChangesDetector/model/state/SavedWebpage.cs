﻿using System;

namespace ChangesDetector.model.state
{
    public class SavedWebpage
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public DateTime LastUpdate { get; set; }
        public int UpdateFrequency { get; set; }
        public bool Active { get; set; }
        public int LastKnownKey { get; set; }
        public string FilePath { get; set; }
    }
}
