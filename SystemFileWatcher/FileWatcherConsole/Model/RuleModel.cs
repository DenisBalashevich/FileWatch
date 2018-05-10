namespace FileWatcherConsole
{
    public class RuleModel
    {
        public string FileNamePattern { get; set; }

        public string AppointmentFolder { get; set; }

        public bool IsOrderNumberAdd { get; set; }

        public bool IsDateAdd { get; set; }
 
        public int OrderNumber { get; set; }
    }
}
