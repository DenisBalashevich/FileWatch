using System.Collections.Generic;
using System.Globalization;

namespace FileWatcherConsole.ConfigParser
{
    public class ConfigurationService
    {
        public static FileMoverModel SetConfigurationSettings(FileSystemWatcherConfigurationSection config)
        {
            var directories = new List<string>(config.Directories.Count);
            var rules = new List<RuleModel>();

            foreach (DirectoryElement directory in config.Directories)
            {
                directories.Add(directory.Path);
            }

            foreach (RuleElement rule in config.Rules)
            {
                rules.Add(new RuleModel
                {
                    FileNamePattern = rule.FileNamePattern,
                    AppointmentFolder = rule.AppointmentFolder,
                    IsDateAdd = rule.IsDateAdd,
                    IsOrderNumberAdd = rule.IsOrderNumberAdd
                });
            }

            CultureInfo.DefaultThreadCurrentCulture = config.Culture;
            CultureInfo.DefaultThreadCurrentUICulture = config.Culture;

            return new FileMoverModel
            {
                Directories = directories,
                Rules = rules
            };
        }
    }
}
