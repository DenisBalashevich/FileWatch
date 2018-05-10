using System;

namespace FileWatcherConsole.EventArguments
{
    public class CreatedFileEventArgs<TModel> : EventArgs
    {
        public TModel CreatedItem { get; set; }
    }
}
