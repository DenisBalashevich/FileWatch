using System;

namespace FileWatcherConsole.EventArguments
{
    public class RenamedFileEventArgs<TModel> : EventArgs
    {
        public TModel RenamedItem { get; set; }
    }
}
