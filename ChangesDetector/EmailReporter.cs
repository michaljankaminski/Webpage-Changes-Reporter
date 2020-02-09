using System;
using ChangesDetector.interfaces;

namespace ChangesDetector
{
    interface IEmailReporter: IReporter
    {

    }
    class EmailReporter : IEmailReporter
    {
        public bool Report()
        {
            throw new NotImplementedException();
        }
    }
}
