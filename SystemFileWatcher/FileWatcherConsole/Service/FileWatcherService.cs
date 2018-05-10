using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using FileWatcherConsole.Logging;
using FileWatcherConsole.EventArguments;
using FileWatcherConsole.Resources;

namespace FileWatcherConsole
{
    public class FileWatcherService
    {
        private readonly List<FileSystemWatcher> _fileSystemWatchers;
        private readonly ILogger _logger;

        public FileWatcherService(IEnumerable<string> directories, ILogger logger)
        {
            _fileSystemWatchers = directories.Select(CreateWatcher).ToList();
            _logger = logger;
        }

        public event EventHandler<CreatedFileEventArgs<FileModel>> Created;

        public event EventHandler<RenamedFileEventArgs<FileModel>> Renamed;

        private void OnCreated(FileModel file)
        {
            Created?.Invoke(this, new CreatedFileEventArgs<FileModel> { CreatedItem = file });
        }

        private void OnRenamed(FileModel file)
        {
            Renamed?.Invoke(this, new RenamedFileEventArgs<FileModel> { RenamedItem = file });
        }

        private FileSystemWatcher CreateWatcher(string path)
        {
            var fileSystemWatcher = new FileSystemWatcher(path)
                                {
                                    NotifyFilter = NotifyFilters.FileName,
                                    EnableRaisingEvents = true
                                };

            fileSystemWatcher.Created += (sender, fileSystemEventArgs) =>
            {
                _logger.Log(string.Format(Strings.FileFound, fileSystemEventArgs.Name));
                OnCreated(new FileModel { FullPath = fileSystemEventArgs.FullPath, Name = fileSystemEventArgs.Name });
            };

            fileSystemWatcher.Renamed += (sender, fileSystemEventArgs) =>
            {
                _logger.Log(string.Format(Strings.FileRenamed, fileSystemEventArgs.Name));
                OnRenamed(new FileModel { FullPath = fileSystemEventArgs.FullPath, Name = fileSystemEventArgs.Name });
            };

            return fileSystemWatcher;
        }
    }
}
