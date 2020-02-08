using System;
using System.Collections.Generic;
using System.Text;

namespace ChangesDetector.model
{
    interface IDetector
    {
        public bool Detect();
        public void Notify();
    }
    class Detector : IDetector
    {
        /// <summary>
        /// Single method responsible for detecting
        /// changes between to complete versions of webpage
        /// </summary>
        /// <returns></returns>
        public bool Detect()
        {
            throw new NotImplementedException();
        }

        public void Notify()
        {
            throw new NotImplementedException();
        }
    }
}
