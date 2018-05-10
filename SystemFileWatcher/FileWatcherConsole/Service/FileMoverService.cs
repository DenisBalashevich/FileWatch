using FileWatcherConsole.Logging;
using FileWatcherConsole.Resources;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace FileWatcherConsole
{
    public class FilesMoverService
    {
        private readonly ILogger _logger;
        private readonly List<RuleModel> _rules;
        private readonly string _defaultFolder;

        public FilesMoverService(IEnumerable<RuleModel> rules, string defaultFolder, ILogger logger)
        {
            _rules = rules.ToList();
            _defaultFolder = defaultFolder;
            _logger = logger;
        }

        public void Move(FileModel item)
        {
            string fromPath = item.FullPath;
            foreach (RuleModel rule in _rules)
            {
                var match = Regex.Match(item.Name, rule.FileNamePattern);

                if (match.Success && match.Length == item.Name.Length)
                {
                    rule.OrderNumber++;
                    string targetPath = ConfigurationPath(item, rule);
                    _logger.Log(Strings.RuleMatch);
                    MoveFile(fromPath, targetPath);
                    return;
                }
            }

            string path = Path.Combine(_defaultFolder, item.Name);
            _logger.Log(Strings.RuleNotMatch);
            MoveFile(fromPath, path);
        }

        private void MoveFile(string startPath, string targetPath)
        {
            string dir = Path.GetDirectoryName(targetPath);
            Directory.CreateDirectory(dir);

            try
            {
                if (File.Exists(targetPath))
                {
                    File.Delete(targetPath);
                }

                File.Move(startPath, targetPath);
                _logger.Log(string.Format(Strings.FileMoved, startPath, targetPath));
            }
            catch (FileNotFoundException)
            {
                _logger.Log(Strings.CannotFindFile);
            }
            catch (IOException)
            {
                _logger.Log(Strings.FileBlocked);
            }
        }

        private string ConfigurationPath(FileModel file, RuleModel rule)
        {
            string filename = Path.GetFileNameWithoutExtension(file.Name);
            StringBuilder path = new StringBuilder();
            path.Append(Path.Combine(rule.AppointmentFolder, filename ?? throw new InvalidOperationException()));

            if (rule.IsDateAdd)
            {
                AppendDate(path);
            }

            if (rule.IsOrderNumberAdd)
            {
                path.Append($"_{rule.OrderNumber}");
            }

            path.Append(Path.GetExtension(file.Name));
            return path.ToString();
        }

        private void AppendDate(StringBuilder destination)
        {
            var dateTimeFormat = CultureInfo.CurrentCulture.DateTimeFormat;
            destination.Append($"{destination}_{DateTime.Now.ToLocalTime().ToString(dateTimeFormat.ShortDatePattern)}");
        }
    }
}
