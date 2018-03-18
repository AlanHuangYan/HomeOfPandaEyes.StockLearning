using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Reflection;
using System.Runtime.Remoting.Messaging;

namespace HomeOfPandaEyes.Infrastructure.Logger
{
    public class LoggingService
    {
        public const string BATCH_DEFAULT_LOGGER = "_sys_default";
        public const string BATCH_LOGGING_SERVICE_SECTION = "loggingConfig";
        public const string ThreadLoggerConfigKey = "[PwC.SMART.ThreadLoggerConfig]";

        private static LoggerProfileSettingsCollection _profiles;
        private static DummyLogger _dummyLogger;

        static LoggingService()
        {
            _dummyLogger = new DummyLogger();
            var section = (LoggerProfileSection)ConfigurationManager.GetSection(BATCH_LOGGING_SERVICE_SECTION);
            if (section != null)
            {
                _profiles = section.Settings;
            }
        }

        public static string LoggerConfig
        {
            get
            {
                var config = CallContext.GetData(ThreadLoggerConfigKey) as string;
                return config;
            }
            set
            {
                CallContext.SetData(ThreadLoggerConfigKey, value);
            }
        }

        

        public static ILogger GetLogger(string configName=null)
        {
            if (string.IsNullOrWhiteSpace(configName))
            {
                configName = LoggerConfig;
                if (string.IsNullOrWhiteSpace(configName))
                {
                    if (_profiles != null && _profiles.Count > 0)
                    {
                        foreach (LoggerProfileSettings profile in _profiles)
                        {
                            if (profile.Default)
                            {
                                configName = profile.Name;
                                break;
                            }
                        }
                    }
                }
                if (string.IsNullOrWhiteSpace(configName))
                {
                    //throw new ArgumentNullException("configName");
                    return _dummyLogger;
                }
            }
            if (_profiles != null && _profiles.Count > 0)
            {
                var profile = _profiles[configName];
                if (profile != null)
                {
                    string loggerName = profile.Logger;
                    if (string.IsNullOrWhiteSpace(loggerName))
                    {
                        loggerName = BATCH_DEFAULT_LOGGER;
                    }
                    if (profile.Enable)
                    {
                        ILogger Logger = profile.Container.GetExportedValueOrDefault<ILogger>(loggerName);
                        if (Logger != null)
                        {
                            if (!Logger.IsInitialized)
                            {
                                Logger.Initialize(profile);
                            }
                            return Logger;
                        }
                    }
                    else
                    {
                        return _dummyLogger;
                    }
                }
            }
            return _dummyLogger;
        }

        public static string Title
        {
            get
            {
                ILogger logger = GetLogger();
                if (logger != null)
                {
                    return logger.Title;
                }
                return string.Empty;
            }
            set
            {
                ILogger logger = GetLogger();
                if (logger != null)
                {
                    logger.Title = value;
                }
            }
        }

        public static string Info(string subFolder, string message, params object[] args)
        {
            try { message = ((args == null) ? message : string.Format(message, args)); }
            catch { }

            ILogger logger = GetLogger();
            if (logger != null)
            {
                logger.Info(message, subFolder);
            }

            return message;
        }

        public static string Verbose(string subFolder, string message, params object[] args)
        {
            try { message = ((args == null) ? message : string.Format(message, args)); }
            catch { }

            ILogger logger = GetLogger();
            if (logger != null)
            {
                logger.Verbose(message, subFolder);
            }

            return message;
        }

        public static string Warning(string subFolder, string message, params object[] args)
        {
            try { message = ((args == null) ? message : string.Format(message, args)); }
            catch { }

            ILogger logger = GetLogger();
            if (logger != null)
            {
                logger.Warning(message, subFolder);
            }

            return message;
        }

        public static string Warning(string subFolder, Exception message)
        {
            ILogger logger = GetLogger();
            if (logger != null)
            {
                logger.Warning(message, subFolder);
            }
            return message.Message;
        }

        public static string Error(string subFolder, string message, params object[] args)
        {
            try { message = ((args == null) ? message : string.Format(message, args)); }
            catch { }

            ILogger logger = GetLogger();
            if (logger != null)
            {
                logger.Error(message, subFolder);
            }

            return message;
        }

        public static string Error(string subFolder, Exception message)
        {
            ILogger logger = GetLogger();
            if (logger != null)
            {
                logger.Error(message, subFolder);
            }
            return message.Message;
        }

        public static string Performance(object message, params object[] args)
        {
            ILogger logger = GetLogger();
            if (logger != null)
            {
                logger.Performance(message);
            }
            if (message != null)
            {
                return message.ToString();
            }
            return string.Empty;
        }
    }
}
