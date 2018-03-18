using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;

namespace HomeOfPandaEyes.Infrastructure.Logger
{
    public class DefaultLoggerConfigurationSource : IConfigurationSource
    {
        private const string GENERAL = "general";
        private const string INFO = "info";
        private const string VERBOSE = "verbose";
        private const string WARNING = "warning";
        private const string ERROR = "error";
        private const string PERFORMANCE = "performance";
        private const string CONSOLE = "console";

        private string _baseFolder = null;

        public DefaultLoggerConfigurationSource(string baseFolder)
        {
            if (string.IsNullOrWhiteSpace(baseFolder))
            {
                throw new ArgumentNullException("baseFolder");
            }
            _baseFolder = baseFolder;
        }

        private string InfoFileName
        {
            get
            {
                return System.IO.Path.Combine(_baseFolder, "info\\info.log");
            }
        }

        private string VerboseFileName
        {
            get
            {
                return System.IO.Path.Combine(_baseFolder, "verbose\\verbose.log");
            }
        }

        private string WarningFileName
        {
            get
            {
                return System.IO.Path.Combine(_baseFolder, "warning\\warning.log");
            }
        }

        private string ErrorFileName
        {
            get
            {
                return System.IO.Path.Combine(_baseFolder, "error\\error.log");
            }
        }

        private string PerformanceFileName
        {
            get
            {
                return System.IO.Path.Combine(_baseFolder, "performance\\performance.log");
            }
        }

        public System.Configuration.ConfigurationSection GetSection(string sectionName)
        {
            if (sectionName.Equals("loggingConfiguration", StringComparison.CurrentCulture))
            {
                var loggingSettings = new LoggingSettings();
                loggingSettings.DefaultCategory = GENERAL;
                loggingSettings.TracingEnabled = true;
                loggingSettings.LogWarningWhenNoCategoriesMatch = true;

                var msgTemplate = GetMessageTemplate();
                var performanceTemplate = GetPerformanceTemplate();

                var msgFormatter = GetMessageFormatter(msgTemplate);
                loggingSettings.Formatters.Add(msgFormatter);

                var performanceFormatter = GetPerformanceFormatter(performanceTemplate);
                loggingSettings.Formatters.Add(performanceFormatter);

                var infoListenerRef = new TraceListenerReferenceData(INFO);
                var verboseListenerRef = new TraceListenerReferenceData(VERBOSE);
                var warningListenerRef = new TraceListenerReferenceData(WARNING);
                var errorListenerRef = new TraceListenerReferenceData(ERROR);
                var performanceListenerRef = new TraceListenerReferenceData(PERFORMANCE);
                var consoleListenerRef = new TraceListenerReferenceData(CONSOLE);

                loggingSettings.TraceListeners.Add(GetInfoListener());
                loggingSettings.TraceListeners.Add(GetVerboseListener());
                loggingSettings.TraceListeners.Add(GetWarningListener());
                loggingSettings.TraceListeners.Add(GetErrorListener());
                loggingSettings.TraceListeners.Add(GetPerformanceListener());
                loggingSettings.TraceListeners.Add(GetConsoleListener());

                loggingSettings.TraceSources.Add(GetGeneralCategory(infoListenerRef, verboseListenerRef, consoleListenerRef));
                loggingSettings.TraceSources.Add(GetInfoCategory(infoListenerRef, consoleListenerRef));
                loggingSettings.TraceSources.Add(GetVerboseCategory(verboseListenerRef, consoleListenerRef));
                loggingSettings.TraceSources.Add(GetWarningCategory(warningListenerRef, consoleListenerRef));
                loggingSettings.TraceSources.Add(GetErrorCategory(errorListenerRef, consoleListenerRef));
                loggingSettings.TraceSources.Add(GetPerformanceCategory(performanceListenerRef));

                // Special Sources 
                loggingSettings.SpecialTraceSources.AllEventsTraceSource.Name = "AllEvents";
                loggingSettings.SpecialTraceSources.AllEventsTraceSource.DefaultLevel = SourceLevels.All;

                loggingSettings.SpecialTraceSources.NotProcessedTraceSource.Name = "NotProcessed";
                loggingSettings.SpecialTraceSources.NotProcessedTraceSource.DefaultLevel = SourceLevels.All;

                loggingSettings.SpecialTraceSources.ErrorsTraceSource.Name = "LoggingErrorsAndWarnings";
                loggingSettings.SpecialTraceSources.ErrorsTraceSource.DefaultLevel = SourceLevels.All;
                loggingSettings.SpecialTraceSources.ErrorsTraceSource.TraceListeners.Clear();
                loggingSettings.SpecialTraceSources.ErrorsTraceSource.TraceListeners.Add(errorListenerRef);

                return loggingSettings;
            }
            return null;
        }

        private static TraceSourceData GetPerformanceCategory(params TraceListenerReferenceData[] listenerRefs)
        {
            return GetCategory(PERFORMANCE, SourceLevels.ActivityTracing, listenerRefs);
        }

        private static TraceSourceData GetErrorCategory(params TraceListenerReferenceData[] listenerRefs)
        {
            return GetCategory(ERROR, SourceLevels.Error, listenerRefs);
        }

        private static TraceSourceData GetWarningCategory(params TraceListenerReferenceData[] listenerRefs)
        {
            return GetCategory(WARNING, SourceLevels.Warning, listenerRefs);
        }

        private static TraceSourceData GetVerboseCategory(params TraceListenerReferenceData[] listenerRefs)
        {
            return GetCategory(VERBOSE, SourceLevels.Verbose, listenerRefs);
        }

        private static TraceSourceData GetInfoCategory(params TraceListenerReferenceData[] listenerRefs)
        {
            return GetCategory(INFO, SourceLevels.Information, listenerRefs);
        }

        private static TraceSourceData GetGeneralCategory(params TraceListenerReferenceData[] listenerRefs)
        {
            return GetCategory(GENERAL, SourceLevels.All, listenerRefs);
        }

        private static TraceSourceData GetCategory(string name,SourceLevels level = SourceLevels.All, params TraceListenerReferenceData[] listenerRefs)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException("name");
            }
            var category = new TraceSourceData();
            if (listenerRefs != null && listenerRefs.Length > 0)
            {
                foreach (TraceListenerReferenceData listenerRef in listenerRefs)
                {
                    category.TraceListeners.Add(listenerRef);
                }
            }
            category.Name = name;
            category.DefaultLevel = level;
            return category;
        }

        private static CustomTraceListenerData GetConsoleListener()
        {
            var consoleListener = new CustomTraceListenerData();
            consoleListener.Name = CONSOLE;
            consoleListener.Type = typeof(ConsoleTraceListener);
            consoleListener.ListenerDataType = typeof(CustomTraceListenerData);
            consoleListener.Filter = SourceLevels.All;
            consoleListener.TraceOutputOptions = TraceOptions.None;
            consoleListener.Formatter = "MsgFormatter";
            return consoleListener;
        }

        private RollingFlatFileTraceListenerData GetPerformanceListener()
        {
            var performanceListener = new RollingFlatFileTraceListenerData();
            performanceListener.Name = PERFORMANCE;
            performanceListener.FileName = PerformanceFileName;
            performanceListener.RollSizeKB = 1024;
            performanceListener.RollInterval = RollInterval.Day;
            performanceListener.RollFileExistsBehavior = RollFileExistsBehavior.Increment;
            performanceListener.Type = typeof(RollingFlatFileTraceListener);
            performanceListener.ListenerDataType = typeof(RollingFlatFileTraceListenerData);
            performanceListener.Filter = SourceLevels.ActivityTracing;
            performanceListener.TraceOutputOptions = TraceOptions.None;
            performanceListener.Formatter = "PerformanceFormatter";
            performanceListener.Header = "---------------Header---------------";
            performanceListener.Footer = "-----------------Footer----------------";
            return performanceListener;
        }

        private RollingFlatFileTraceListenerData GetErrorListener()
        {
            var errorListener = new RollingFlatFileTraceListenerData();
            errorListener.Name = ERROR;
            errorListener.FileName = ErrorFileName;
            errorListener.RollSizeKB = 1024;
            errorListener.RollInterval = RollInterval.Day;
            errorListener.RollFileExistsBehavior = RollFileExistsBehavior.Increment;
            errorListener.Type = typeof(RollingFlatFileTraceListener);
            errorListener.ListenerDataType = typeof(RollingFlatFileTraceListenerData);
            errorListener.Filter = SourceLevels.Error;
            errorListener.TraceOutputOptions = TraceOptions.None;
            errorListener.Formatter = "MsgFormatter";
            errorListener.Header = "---------------Header---------------";
            errorListener.Footer = "-----------------Footer----------------";
            return errorListener;
        }

        private RollingFlatFileTraceListenerData GetWarningListener()
        {
            var warningListener = new RollingFlatFileTraceListenerData();
            warningListener.Name = WARNING;
            warningListener.FileName = WarningFileName;
            warningListener.RollSizeKB = 1024;
            warningListener.RollInterval = RollInterval.Day;
            warningListener.RollFileExistsBehavior = RollFileExistsBehavior.Increment;
            warningListener.Type = typeof(RollingFlatFileTraceListener);
            warningListener.ListenerDataType = typeof(RollingFlatFileTraceListenerData);
            warningListener.Filter = SourceLevels.Warning;
            warningListener.TraceOutputOptions = TraceOptions.None;
            warningListener.Formatter = "MsgFormatter";
            warningListener.Header = "---------------Header---------------";
            warningListener.Footer = "-----------------Footer----------------";
            return warningListener;
        }

        private RollingFlatFileTraceListenerData GetVerboseListener()
        {
            var verboseListener = new RollingFlatFileTraceListenerData();
            verboseListener.Name = VERBOSE;
            verboseListener.FileName = VerboseFileName;
            verboseListener.RollSizeKB = 1024;
            verboseListener.RollInterval = RollInterval.Day;
            verboseListener.RollFileExistsBehavior = RollFileExistsBehavior.Increment;
            verboseListener.Type = typeof(RollingFlatFileTraceListener);
            verboseListener.ListenerDataType = typeof(RollingFlatFileTraceListenerData);
            verboseListener.Filter = SourceLevels.Verbose;
            verboseListener.TraceOutputOptions = TraceOptions.None;
            verboseListener.Formatter = "MsgFormatter";
            verboseListener.Header = "---------------Header---------------";
            verboseListener.Footer = "-----------------Footer----------------";
            return verboseListener;
        }

        private RollingFlatFileTraceListenerData GetInfoListener()
        {
            var infoListener = new RollingFlatFileTraceListenerData();
            infoListener.Name = INFO;
            infoListener.FileName = InfoFileName;
            infoListener.RollSizeKB = 1024;
            infoListener.RollInterval = RollInterval.Day;
            infoListener.RollFileExistsBehavior = RollFileExistsBehavior.Increment;
            infoListener.Type = typeof(RollingFlatFileTraceListener);
            infoListener.ListenerDataType = typeof(RollingFlatFileTraceListenerData);
            infoListener.Filter = SourceLevels.Information;
            infoListener.TraceOutputOptions = TraceOptions.None;
            infoListener.Formatter = "MsgFormatter";
            infoListener.Header = "---------------Header---------------";
            infoListener.Footer = "-----------------Footer----------------";
            return infoListener;
        }

        private static TextFormatterData GetPerformanceFormatter(string performanceTemplate)
        {
            var performanceFormatter = new TextFormatterData();
            performanceFormatter.Name = "PerformanceFormatter";
            performanceFormatter.Type = typeof(TextFormatter);
            performanceFormatter.Template = performanceTemplate;
            return performanceFormatter;
        }

        private static TextFormatterData GetMessageFormatter(string msgTemplate)
        {
            var msgFormatter = new TextFormatterData();
            msgFormatter.Name = "MsgFormatter";
            msgFormatter.Type = typeof(TextFormatter);
            msgFormatter.Template = msgTemplate;
            return msgFormatter;
        }

        private static string GetPerformanceTemplate()
        {
            var performanceTemplate = new StringBuilder();
            performanceTemplate.AppendLine("{message}");
            return performanceTemplate.ToString();
        }

        private static string GetMessageTemplate()
        {
            var msgTemplate = new StringBuilder();
            msgTemplate.AppendLine("Timestamp: {timestamp(local)}");
            msgTemplate.AppendLine("Severity: {severity}");
            //msgTemplate.AppendLine("Category: {category}");
            msgTemplate.AppendLine("Title:{title}");
            msgTemplate.AppendLine("Message: {message}");
            //msgTemplate.AppendLine("Priority: {priority}");
            //msgTemplate.AppendLine("EventId: {eventid}");
            //msgTemplate.AppendLine("Machine: {machine}");
            //msgTemplate.AppendLine("Application Domain: {appDomain}");
            //msgTemplate.AppendLine("Process    {processId}");
            //msgTemplate.AppendLine("Process Name: {processName}");
            //msgTemplate.AppendLine("Win32 Thread    {win32ThreadId}");
            //msgTemplate.AppendLine("Thread Name: {threadName}");
            //msgTemplate.AppendLine("Extended Properties: {dictionary({key} - {value})}");
            return msgTemplate.ToString();
        }

        public void Add(string sectionName, System.Configuration.ConfigurationSection configurationSection)
        {
        }

        public void AddSectionChangeHandler(string sectionName, ConfigurationChangedEventHandler handler)
        {
        }

        public void Remove(string sectionName)
        {
        }

        public void RemoveSectionChangeHandler(string sectionName, ConfigurationChangedEventHandler handler)
        {
        }

        public event EventHandler<ConfigurationSourceChangedEventArgs> SourceChanged;

        public void Dispose()
        {
        }
    }
}
