using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HomeOfPandaEyes.Infrastructure.Logger
{
    public class DummyLogger : BaseLogger
    {
        protected override void WritePerformance(object message)
        {
        }

        protected override void WriteError(object message, string subFolder)
        {
        }

        protected override void WriteWarning(object message, string subFolder)
        {
        }

        protected override void WriteVerbose(object message, string subFolder)
        {
        }

        protected override void WriteInfo(object message, string subFolder)
        {
        }

        protected override void WriteUnknown(object message, string subFolder)
        {
        }
    }
}
