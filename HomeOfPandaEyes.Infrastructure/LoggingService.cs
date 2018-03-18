using System;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using System.Diagnostics;
using System.Runtime.Remoting.Messaging;

namespace PwC.SMART.Batch.Infrastructure
{
    public class LoggingService
    {
        public const string CategoryFatal = "CategoryFatal";
        public const string CategoryError = "CategoryError";
        public const string CategoryInfo = "CategoryInfo";
        public const string ThreadTitleKey = "[PwC.SMART.ThreadLogTitle]";

        private static readonly bool Verbos;

        static LoggingService()
        {
            Verbos = false;
            string cfgVerbos = ConfigurationManager.AppSettings["Verbos"];
            if (!string.IsNullOrWhiteSpace(cfgVerbos))
            {
                bool.TryParse(cfgVerbos, out Verbos);
            }
        }

        public static string ThreadTitle
        {
            get
            {
                var title = CallContext.GetData(ThreadTitleKey) as string;
                return title;
            }
            set { CallContext.SetData(ThreadTitleKey, value); }
        }

        public static void Info(string message, params object[] args)
        {
            try { message = ((args == null) ? message : string.Format(message, args)); }
            catch { }
            Logger.Write(new LogEntry
                             {
                                 Severity = TraceEventType.Information,
                                 Title = ThreadTitle,
                                 Message = message,
                                 Categories = { CategoryInfo }
                             });
        }

        public static void Verbose(string message, params object[] args)
        {
            if (Verbos)
            {
                try { message = ((args == null) ? message : string.Format(message, args)); }
                catch { }
                Logger.Write(new LogEntry
                                 {
                                     Severity = TraceEventType.Verbose,
                                     Title = ThreadTitle,
                                     Message = message,
                                     Categories = { CategoryInfo }
                                 });
            }
        }

        public static void Error(string message, params object[] args)
        {
            try { message = ((args == null) ? message : string.Format(message, args)); }
            catch { }
            Logger.Write(new LogEntry
                             {
                                 Severity = TraceEventType.Error,
                                 Title = ThreadTitle,
                                 Message = message,
                                 Categories = { CategoryError }
                             });
        }

        public static void Critical(string message, params object[] args)
        {
            try { message = ((args == null) ? message : string.Format(message, args)); }
            catch { }
            Logger.Write(new LogEntry
                             {
                                 Severity = TraceEventType.Critical,
                                 Title = ThreadTitle,
                                 Message = message,
                                 Categories = { CategoryFatal }
                             });
        }

        public static void Critical(Exception exception)
        {
            Logger.Write(new LogEntry
                             {
                                 Severity = TraceEventType.Critical,
                                 Title = ThreadTitle,
                                 Message = exception.Message + "\r\n" + exception.StackTrace,
                                 Categories = { CategoryFatal }
                             });
        }
    }
}