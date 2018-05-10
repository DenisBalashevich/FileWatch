using System.Configuration;

namespace FileWatcherConsole.ConfigParser
{
    public class RuleElement : ConfigurationElement
    {
        [ConfigurationProperty("fileNamePattern", IsKey = true, IsRequired = true)]
        public string FileNamePattern => (string)this["fileNamePattern"];

        [ConfigurationProperty("appointmentFolder", IsRequired = true)]
        public string AppointmentFolder => (string)this["appointmentFolder"];

        [ConfigurationProperty("isOrderNumberAdd", IsRequired = false, DefaultValue = false)]
        public bool IsOrderNumberAdd => (bool)this["isOrderNumberAdd"];

        [ConfigurationProperty("isDateAdd", IsRequired = false, DefaultValue = false)]
        public bool IsDateAdd => (bool)this["isDateAdd"];
    }
}
