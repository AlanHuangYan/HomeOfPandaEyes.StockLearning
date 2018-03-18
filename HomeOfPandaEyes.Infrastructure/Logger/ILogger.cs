using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace HomeOfPandaEyes.Infrastructure.Logger
{
    public interface ILogger
    {
        bool IsInitialized { get; }
        string Title { get; set; }
        LoggerProfileSettings ProfileSetting { get; }
        void Initialize(LoggerProfileSettings setting);
        void Info(object message, string subFolder = null);
        void Verbose(object message, string subFolder = null);
        void Warning(object message, string subFolder = null);
        void Error(object message, string subFolder = null);
        void Performance(object message);
    }
}
