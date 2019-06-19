using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.EventLog;
using System.Diagnostics;

namespace ViaVarejo.ClearSale.FileCopy.Infra
{
    public static class Log
    {

        public static ILogger LogActive()
        {
            ILoggerFactory loggerFactory = new LoggerFactory()
            .AddConsole()
            .AddDebug();
            ILogger logger = loggerFactory.CreateLogger<Program>();
            return logger;
        }

        public static void  PutEventLog(string eventlog, string aplication, string type )
        {

            var appLog = new System.Diagnostics.EventLog(type);
            appLog.Source = aplication;

            appLog.WriteEntry(eventlog);
        }

    }

}

