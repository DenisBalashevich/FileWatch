using System.Collections.Generic;

namespace FileWatcherConsole.ConfigParser
{
    public class FileMoverModel
    {
        public List<string> Directories { get; set; }

        public List<RuleModel> Rules { get; set; }
    }
}
