namespace PuntosColombia.MissingNumbers.Infrastructure.Framework.Instrumentation.Logging
{
    using Serilog;
    using System;
    using System.Diagnostics;

    public class LoggerService : ILoggerService
    {
        private readonly ILogger log;

        public LoggerService(ILogger log)
        {
            this.log = log.ForContext("Priority", 1)
                .ForContext("Title", "Integration Services Ordenes")
                .ForContext("MachineName", CurrentMachineName)
                .ForContext("AppDomainName", Environment.UserDomainName)
                .ForContext("ProcessID", Process.GetCurrentProcess().Id)
                .ForContext("ProcessName", Process.GetCurrentProcess().ProcessName);
        }

        //static readonly ILogger log = Serilog.Log.ForContext<LoggerService>();

        public string CurrentUser
        {
            get
            {
                return Environment.UserDomainName + "\\" + Environment.UserName;
            }
        }

        public string CurrentMachineName
        {
            get
            {
                return Environment.MachineName;
            }
        }

        public void LogException(Exception ex)
        {
            log.Error(ex, ex.Message);
        }


        public void LogDebug(string message)
        {
            log.Debug(message);
        }

        public void LogInformation(string message)
        {
            log.Information(message);
        }

        public void LogWarning(string message)
        {
            log.Warning(message);
        }
    }
}
