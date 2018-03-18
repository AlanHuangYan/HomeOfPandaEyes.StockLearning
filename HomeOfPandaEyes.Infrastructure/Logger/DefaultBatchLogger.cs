using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using System.Collections;
using System.Runtime.Remoting.Messaging;

namespace HomeOfPandaEyes.Infrastructure.Logger
{
    [Export(LoggingService.BATCH_DEFAULT_LOGGER, typeof(ILogger))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class DefaultLogger : BaseLogger
    {
        public const string ThreadLoggerTitleKey = "[PwC.SMART.ThreadLoggerTitle]";

        private Hashtable _writers;

        public DefaultLogger()
        {
            _writers = Hashtable.Synchronized(new Hashtable());
        }

        private string GetBaseFolder(string subFolder)
        {
            if (subFolder == null)
            {
                return ProfileSetting.Folder;
            }
            else
            {
                return System.IO.Path.Combine(ProfileSetting.Folder, subFolder);
            }
        }

        private LogWriter GetLogWriter(string subFolder)
        {
            string baseFolder = GetBaseFolder(subFolder);
            lock (_writers.SyncRoot)
            {
                if (_writers.ContainsKey(baseFolder))
                {
                    return (LogWriter)_writers[baseFolder];
                }
                else
                {
                    var factory = new LogWriterFactory(new DefaultLoggerConfigurationSource(baseFolder));
                    LogWriter writer = factory.Create();
                    _writers.Add(baseFolder, writer);
                    return writer;
                }
            }
        }

        private string GetMessageString(object message)
        {
            if (message == null)
            {
                return string.Empty;
            }
            else if (typeof(Exception) == message.GetType())
            {
                return GetExceptionMessage((Exception)message);
            }
            else
            {
                return message.ToString();
            }
        }

        private string GetExceptionMessage(Exception ex)
        {
            if (ex == null)
            {
                return string.Empty;
            }
            else
            {
                StringBuilder msgBuffer = new StringBuilder();
                msgBuffer.AppendLine(ex.Message);
                msgBuffer.AppendLine(ex.StackTrace);
                msgBuffer.AppendLine("-------------------");
                if (ex.InnerException != null)
                {
                    msgBuffer.AppendLine(GetMessageString(ex.InnerException));
                }
                return msgBuffer.ToString();
            }
        }

        public override string Title
        {
            get
            {
                var title = CallContext.GetData(ThreadLoggerTitleKey) as string;
                return title;
            }
            set
            {
                CallContext.SetData(ThreadLoggerTitleKey, value);
            }
        }

        protected override void WriteInfo(object message, string subFolder)
        {
            LogEntry entry = new LogEntry();
            entry.Categories.Add(CATEGORY_INFO);
            entry.Severity = TraceEventType.Information;
            entry.Title = Title;
            entry.Message = GetMessageString(message);
            LogWriter writer = GetLogWriter(subFolder);
            writer.Write(entry);
        }

        protected override void WriteVerbose(object message, string subFolder)
        {
            LogEntry entry = new LogEntry();
            entry.Categories.Add(CATEGORY_VERBOSE);
            entry.Severity = TraceEventType.Verbose;
            entry.Title = Title;
            entry.Message = GetMessageString(message);
            LogWriter writer = GetLogWriter(subFolder);
            writer.Write(entry);
        }

        protected override void WriteWarning(object message, string subFolder)
        {
            LogEntry entry = new LogEntry();
            entry.Categories.Add(CATEGORY_WARNING);
            entry.Severity = TraceEventType.Warning;
            entry.Title = Title;
            entry.Message = GetMessageString(message);
            LogWriter writer = GetLogWriter(subFolder);
            writer.Write(entry);
        }

        protected override void WriteError(object message, string subFolder)
        {
            LogEntry entry = new LogEntry();
            entry.Categories.Add(CATEGORY_ERROR);
            entry.Severity = TraceEventType.Error;
            entry.Title = Title;
            entry.Message = GetMessageString(message);
            LogWriter writer = GetLogWriter(subFolder);
            writer.Write(entry);
        }

        protected override void WritePerformance(object message)
        {
            LogEntry entry = new LogEntry();
            entry.Categories.Add(CATEGORY_PERFORMANCE);
            entry.Severity = TraceEventType.Information;
            entry.Title = Title;
            entry.Message = GetMessageString(message);
            LogWriter writer = GetLogWriter(null);
            writer.Write(entry);
        }

        protected override void WriteUnknown(object message, string subFolder)
        {
            WriteVerbose(message, subFolder);
        }
        

    }
}
