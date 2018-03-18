using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace HomeOfPandaEyes.Infrastructure.Logger
{
    [ConfigurationCollection(typeof(LoggerProfileSettings))]
    public class LoggerProfileSettingsCollection : ConfigurationElementCollection
    {
        protected static ConfigurationPropertyCollection _properties;

        static LoggerProfileSettingsCollection()
        {
            _properties = new ConfigurationPropertyCollection();
        }

        public LoggerProfileSettingsCollection()
        {
        }

        protected override void BaseAdd(int index, ConfigurationElement element)
        {
            if (index == -1)
            {
                BaseAdd(element, false);
            }
            else
            {
                base.BaseAdd(index, element);
            }
        }

        public void Clear()
        {
            BaseClear();
        }

        public int IndexOf(LoggerProfileSettings setting)
        {
            return BaseIndexOf(setting);
        }

        public void Remove(LoggerProfileSettings setting)
        {
            if (BaseIndexOf(setting) >= 0)
            {
                BaseRemove(setting.Key);
            }
        }

        public void Remove(string name)
        {
            BaseRemove(name);
        }

        public void RemoveAt(int index)
        {
            BaseRemoveAt(index);
        }

        public new LoggerProfileSettings this[string name]
        {
            get { return (LoggerProfileSettings)BaseGet(name); }
        }

        public LoggerProfileSettings this[int index]
        {
            get { return (LoggerProfileSettings)BaseGet(index); }
            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }
                BaseAdd(index, value);
            }
        }

        protected override ConfigurationPropertyCollection Properties
        {
            get { return _properties; }
        }

        public void Add(LoggerProfileSettings setting)
        {
            BaseAdd(setting);
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new LoggerProfileSettings();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((LoggerProfileSettings)element).Key;
        }
    }
}
