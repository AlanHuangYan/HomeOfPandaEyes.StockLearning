using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel;
using System.IO;

namespace HomeOfPandaEyes.Infrastructure.Logger
{
    public class LoggerProfileSettings : ConfigurationElement
    {
        protected static readonly ConfigurationProperty _propName;
        protected static readonly ConfigurationProperty _propFolder;
        protected static readonly ConfigurationProperty _propAssembly;
        protected static readonly ConfigurationProperty _propLogger;
        protected static readonly ConfigurationProperty _propEnable;
        protected static readonly ConfigurationProperty _propInfo;
        protected static readonly ConfigurationProperty _propVerbose;
        protected static readonly ConfigurationProperty _propWarning;
        protected static readonly ConfigurationProperty _propError;
        protected static readonly ConfigurationProperty _propPerformance;
        protected static readonly ConfigurationProperty _propDefault;
        protected static ConfigurationPropertyCollection _LoggerProfileProperties;

        private CompositionContainer _container;

        static LoggerProfileSettings()
        {
            _propName =
                new ConfigurationProperty("name",
                                          typeof (string),
                                          null,
                                          null,
                                          new StringValidator(1),
                                          ConfigurationPropertyOptions.IsKey | ConfigurationPropertyOptions.IsRequired,
                                          string.Empty);
            _propFolder =
                new ConfigurationProperty("folder",
                                          typeof (string),
                                          null,
                                          null,
                                          null,
                                          ConfigurationPropertyOptions.None,
                                          string.Empty);
            _propAssembly =
                new ConfigurationProperty("assembly",
                                          typeof (string),
                                          null,
                                          null,
                                          null,
                                          ConfigurationPropertyOptions.None,
                                          string.Empty);

            _propLogger =
                            new ConfigurationProperty("logger",
                                                      typeof(string),
                                                      null,
                                                      null,
                                                      null,
                                                      ConfigurationPropertyOptions.None,
                                                      string.Empty);

            _propEnable =
                new ConfigurationProperty("enable",
                                          typeof(bool)
                                          ,true
                                          ,new BooleanConverter()
                                          ,null
                                          ,ConfigurationPropertyOptions.None
                                          ,string.Empty);

            _propInfo =
                new ConfigurationProperty("info",
                                          typeof(bool)
                                          ,true
                                          ,new BooleanConverter()
                                          ,null
                                          ,ConfigurationPropertyOptions.None
                                          ,string.Empty);

            _propVerbose =
                new ConfigurationProperty("verbose",
                                          typeof(bool)
                                          ,false
                                          ,new BooleanConverter()
                                          ,null
                                          ,ConfigurationPropertyOptions.None
                                          ,string.Empty);

            _propWarning =
                new ConfigurationProperty("warning",
                                          typeof(bool)
                                          ,true
                                          ,new BooleanConverter()
                                          ,null
                                          ,ConfigurationPropertyOptions.None
                                          ,string.Empty);

            _propError =
                new ConfigurationProperty("error",
                                          typeof(bool)
                                          ,true
                                          ,new BooleanConverter()
                                          ,null
                                          ,ConfigurationPropertyOptions.None
                                          ,string.Empty);

            _propPerformance =
                new ConfigurationProperty("performance",
                                          typeof(bool)
                                          ,false
                                          ,new BooleanConverter()
                                          ,null
                                          ,ConfigurationPropertyOptions.None
                                          ,string.Empty);

            _propDefault =
                new ConfigurationProperty("default",
                                          typeof(bool)
                                          , false
                                          , new BooleanConverter()
                                          , null
                                          , ConfigurationPropertyOptions.None
                                          , string.Empty);
            
            _LoggerProfileProperties = new ConfigurationPropertyCollection();
            _LoggerProfileProperties.Add(_propName);
            _LoggerProfileProperties.Add(_propFolder);
            _LoggerProfileProperties.Add(_propAssembly);
            _LoggerProfileProperties.Add(_propLogger);
            _LoggerProfileProperties.Add(_propEnable);
            _LoggerProfileProperties.Add(_propInfo);
            _LoggerProfileProperties.Add(_propVerbose);
            _LoggerProfileProperties.Add(_propWarning);
            _LoggerProfileProperties.Add(_propError);
            _LoggerProfileProperties.Add(_propPerformance);
            _LoggerProfileProperties.Add(_propDefault);
        }

        public LoggerProfileSettings()
        {
        }

        public virtual string Key
        {
            get { return Name; }
        }

        [ConfigurationProperty("name",
            Options = ConfigurationPropertyOptions.IsKey | ConfigurationPropertyOptions.IsRequired, DefaultValue = "")]
        public virtual string Name
        {
            get { return (string)base[_propName]; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Name");
                }
                base[_propName] = value;
            }
        }

        [ConfigurationProperty("folder", DefaultValue = "", Options = ConfigurationPropertyOptions.None)]
        public virtual string Folder
        {
            get 
            {
                string folder = (string)base[_propFolder];
                if (string.IsNullOrWhiteSpace(folder))
                {
                    folder = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
                    folder = Path.Combine(folder, Name);
                }
                return folder;
            }
            set { base[_propFolder] = value; }
        }

        [ConfigurationProperty("assembly", DefaultValue = "", Options = ConfigurationPropertyOptions.None)]
        public virtual string Assembly
        {
            get { return (string)base[_propAssembly]; }
            set { base[_propAssembly] = value; }
        }

        [ConfigurationProperty("logger", DefaultValue = "", Options = ConfigurationPropertyOptions.None)]
        public virtual string Logger
        {
            get { return (string)base[_propLogger]; }
            set { base[_propLogger] = value; }
        }

        [ConfigurationProperty("enable", DefaultValue = true, Options = ConfigurationPropertyOptions.None)]
        public virtual bool Enable
        {
            get { return (bool)base[_propEnable]; }
            set { base[_propEnable] = value; }
        }

        [ConfigurationProperty("info", DefaultValue = true, Options = ConfigurationPropertyOptions.None)]
        public virtual bool Info
        {
            get { return (bool)base[_propInfo]; }
            set { base[_propInfo] = value; }
        }

        [ConfigurationProperty("verbose", DefaultValue = false, Options = ConfigurationPropertyOptions.None)]
        public virtual bool Verbose
        {
            get { return (bool)base[_propVerbose]; }
            set { base[_propVerbose] = value; }
        }

        [ConfigurationProperty("warning", DefaultValue = true, Options = ConfigurationPropertyOptions.None)]
        public virtual bool Warning
        {
            get { return (bool)base[_propWarning]; }
            set { base[_propWarning] = value; }
        }

        [ConfigurationProperty("error", DefaultValue = true, Options = ConfigurationPropertyOptions.None)]
        public virtual bool Error
        {
            get { return (bool)base[_propError]; }
            set { base[_propError] = value; }
        }

        [ConfigurationProperty("performance", DefaultValue = false, Options = ConfigurationPropertyOptions.None)]
        public virtual bool Performance
        {
            get { return (bool)base[_propPerformance]; }
            set { base[_propPerformance] = value; }
        }

        [ConfigurationProperty("default", DefaultValue = false, Options = ConfigurationPropertyOptions.None)]
        public virtual bool Default
        {
            get { return (bool)base[_propDefault]; }
            set { base[_propDefault] = value; }
        }

        public virtual CompositionContainer Container
        {
            get
            {
                if (_container == null)
                {
                    System.Reflection.Assembly assembly = typeof(ILogger).Assembly;
                    if (!string.IsNullOrWhiteSpace(Assembly))
                    {
                        try
                        {
                            assembly = System.Reflection.Assembly.Load(Assembly);
                        }
                        catch
                        {
                            assembly = null;
                        }
                    }
                    if (assembly != null)
                    {
                        _container = new CompositionContainer(new AssemblyCatalog(assembly));
                    }
                }
                return _container;
            }
        }



    }
}
