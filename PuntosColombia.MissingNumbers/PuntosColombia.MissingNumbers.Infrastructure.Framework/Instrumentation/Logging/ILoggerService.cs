namespace PuntosColombia.MissingNumbers.Infrastructure.Framework.Instrumentation.Logging
{
    using System;

    public interface ILoggerService
    {
        void LogException(Exception ex);

        void LogDebug(string message);

        void LogInformation(string message);

        void LogWarning(string message);
    }
}
