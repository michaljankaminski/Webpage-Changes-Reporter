using System;

namespace ChangesDetector.model
{
    interface IDetector
    {
        public bool Detect();
        public void Notify();
    }
    class Detector : IDetector
    {
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
