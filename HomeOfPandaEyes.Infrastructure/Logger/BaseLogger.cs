using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace HomeOfPandaEyes.Infrastructure.Logger
{
    public abstract class BaseLogger : ILogger
    {
        public const string CATEGORY_INFO = "info";
        public const string CATEGORY_VERBOSE = "verbose";
        public const string CATEGORY_WARNING = "warning";
        public const string CATEGORY_ERROR = "error";
        public const string CATEGORY_CRITICAL = "critical";
        public const string CATEGORY_PERFORMANCE = "performance";
        public const string CATEGORY_UNKNOWN = "unknown";

        private bool _isInitialized = false;
        private string _title;

        private LoggerProfileSettings _profileSetting;

        public virtual bool IsInitialized
        {
            get { return _isInitialized; }
        }

        public virtual LoggerProfileSettings ProfileSetting
        {
            get 
            {
                if (_profileSetting == null)
                {
                    throw new ArgumentNullException("profile setting");
                }
                return _profileSetting; 
            }
        }

        public virtual void Initialize(LoggerProfileSettings setting)
        {
            if (!_isInitialized)
            {
                bool throwExp = false;
                try
                {
                    ImplementInitialization(setting);
                }
                catch
                {
                    throwExp = true;
                    throw;
                }
                finally
                {
                    if (!throwExp)
                    {
                        _isInitialized = true;
                    }
                }
            }
        }

        protected virtual void ImplementInitialization(LoggerProfileSettings setting)
        {
            _profileSetting = setting;
        }

        public virtual string Title
        {
            get
            {
                return _title;
            }
            set
            {
                _title = value;
            }
        }

        protected virtual void Write(object message, string category, string subFolder = null)
        {
            if (IsInitialized)
            {
                bool processed = false;
                if (string.Compare(category, CATEGORY_INFO, true) == 0)
                {
                    if (ProfileSetting.Info)
                    {
                        WriteInfo(message, subFolder);
                    }
                    processed = true;
                }
                else if (string.Compare(category, CATEGORY_VERBOSE, true) == 0)
                {
                    if (ProfileSetting.Verbose)
                    {
                        WriteVerbose(message, subFolder);
                    }
                    processed = true;
                }
                else if (string.Compare(category, CATEGORY_WARNING, true) == 0)
                {
                    if (ProfileSetting.Warning)
                    {
                        WriteWarning(message, subFolder);
                    }
                    processed = true;
                }
                else if (string.Compare(category, CATEGORY_ERROR, true) == 0)
                {
                    if (ProfileSetting.Error)
                    {
                        WriteError(message, subFolder);
                    }
                    processed = true;
                }
                else if (string.Compare(category, CATEGORY_PERFORMANCE, true) == 0)
                {
                    if (ProfileSetting.Performance)
                    {
                        WritePerformance(message);
                    }
                    processed = true;
                }

                if (!processed)
                {
                    
                }
            }
        }

        protected abstract void WritePerformance(object message);
        protected abstract void WriteError(object message, string subFolder);
        protected abstract void WriteWarning(object message, string subFolder);
        protected abstract void WriteVerbose(object message, string subFolder);
        protected abstract void WriteInfo(object message, string subFolder);
        protected abstract void WriteUnknown(object message, string subFolder);

        public virtual void Info(object message, string subFolder = null)
        {
            Write(message, CATEGORY_INFO, subFolder);
        }

        public virtual void Verbose(object message, string subFolder = null)
        {
            Write(message, CATEGORY_VERBOSE, subFolder);
        }

        public virtual void Error(object message, string subFolder = null)
        {
            Write(message, CATEGORY_ERROR, subFolder);
        }

        public virtual void Warning(object message, string subFolder = null)
        {
            Write(message, CATEGORY_WARNING, subFolder);
        }

        public virtual void Critical(object message, string subFolder = null)
        {
            Write(message, CATEGORY_CRITICAL, subFolder);
        }

        public virtual void Performance(object message)
        {
            Write(message, CATEGORY_PERFORMANCE, null);
        }
    }
}
