using FileWatcherConsole.ConfigParser;
using FileWatcherConsole.EventArguments;
using FileWatcherConsole.Logging;
using FileWatcherConsole.Resources;
using System;
using System.Configuration;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

namespace FileWatcherConsole
{
    class Program
    {
        private static FilesMoverService _fileMoverService;

        static void Main(string[] args)
        {
            var config = ConfigurationManager.GetSection("fileWatcher") as FileSystemWatcherConfigurationSection;
            FileMoverModel fileMoverModel;

            if (config != null)
            {
                fileMoverModel = ConfigurationService.SetConfigurationSettings(config);
            }
            else
            {
                Console.Write(Strings.ConfigNotFound);
                return;
            }

            Console.WriteLine(config.Culture.DisplayName);

            ILogger logger = new Logger();
            _fileMoverService = new FilesMoverService(fileMoverModel.Rules, config.Rules.DefaultDirectory, logger);
            FileWatcherService watcher = new FileWatcherService(fileMoverModel.Directories, logger);

            watcher.Created += OnCreated;
            watcher.Renamed += OnRenamed;

            Console.CancelKeyPress += cancelHandler;
            while (true)
            {
                var cki = Console.ReadKey(true);
                if (cki.Key == ConsoleKey.X) break;
            }
        }

        private static void OnCreated(object sender, CreatedFileEventArgs<FileModel> args)
        {
            _fileMoverService.Move(args.CreatedItem);
        }

        private static void OnRenamed(object sender, RenamedFileEventArgs<FileModel> args)
        {
            _fileMoverService.Move(args.RenamedItem);
        }

        protected static void cancelHandler(object sender, ConsoleCancelEventArgs args)
        {
            args.Cancel = true;
        }
    }
}
