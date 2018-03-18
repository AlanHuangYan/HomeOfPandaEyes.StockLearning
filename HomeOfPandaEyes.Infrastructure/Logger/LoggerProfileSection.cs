using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace HomeOfPandaEyes.Infrastructure.Logger
{
    public class LoggerProfileSection : ConfigurationSection
    {
        protected static readonly ConfigurationProperty _propSettings;
        protected static ConfigurationPropertyCollection _properties;

        static LoggerProfileSection()
        {
            _propSettings =
                new ConfigurationProperty(null,
                                          typeof (LoggerProfileSettingsCollection),
                                          null,
                                          ConfigurationPropertyOptions.IsDefaultCollection);
            _properties = new ConfigurationPropertyCollection();
            _properties.Clear();
            _properties.Add(_propSettings);
        }

        public LoggerProfileSection()
        {
        }

        protected override object GetRuntimeObject()
        {
            SetReadOnly();
            return this;
        }

        [ConfigurationProperty("", Options = ConfigurationPropertyOptions.IsDefaultCollection)]
        public LoggerProfileSettingsCollection Settings
        {
            get { return (LoggerProfileSettingsCollection)base[_propSettings]; }
        }

        protected override ConfigurationPropertyCollection Properties
        {
            get { return _properties; }
        }
    }
}
