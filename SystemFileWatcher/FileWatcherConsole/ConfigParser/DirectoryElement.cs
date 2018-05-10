using System.Configuration;

namespace FileWatcherConsole.ConfigParser
{
    public class DirectoryElement : ConfigurationElement
    {
        [ConfigurationProperty("path", IsKey = true, IsRequired = true)]
        public string Path => (string)this["path"];
    }
}
