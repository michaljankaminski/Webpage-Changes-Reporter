using System;
using System.Collections.Generic;
using System.Text;

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
