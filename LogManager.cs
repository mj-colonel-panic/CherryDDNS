using System.Diagnostics;

namespace CherryDDNS
{
    public static class LogManager
    {
        private static EventLog eventLog = new EventLog("Application", Environment.MachineName, ".NET Runtime");

        public static void Startup()
        {
            string message = $"CherryDDNS Service Started.\r\n" +
                $"The Below Config was loaded from {Config.FilePath}\r\n" +
                $"---------------------------------\r\n" +
                $"{Config.Instance.ToMaskedString()}";
            eventLog.WriteEntry(message, EventLogEntryType.Information, 1000);
        }

        public static void ARecordUpdated(Record record)
        {
            string message = $"CherryDDNS Service Updated the A Record for {record.HostName}.{record.DomainName}.";
            eventLog.WriteEntry(message, EventLogEntryType.Information, 1000);
        }

        public static void Cancelled()
        {
            string message = $"CherryDDNS Service was interrupted.";
            eventLog.WriteEntry(message, EventLogEntryType.Information, 1000);
        }

        public static void InvalidConfig()
        {
            string message = $"CherryDDNS Config is Invalid.\r\n" +
            $"Check the contents of {Config.FilePath}\r\n" +
            $"It should contain a JSON Object formatted as below\r\n" +
            $"The CherryDDNS Service has been terminated as it cannot run without this configuration.\r\n" +
            $"---------------------------------\r\n" +
            $"{Config.Model}";
            eventLog.WriteEntry(message, EventLogEntryType.Error, 1000);
        }

        public static void Error(Exception ex)
        {
            string message = $"CherryDDNS Service encountered an error:\r\n" +
                $"Message:\t{ex.Message}\r\n" +
                $"Source:\t{ex.Source}\r\n" +
                $"StackTrace:\t{ex.StackTrace}";
            eventLog.WriteEntry(message, EventLogEntryType.Error, 1000);
        }
    }
}
