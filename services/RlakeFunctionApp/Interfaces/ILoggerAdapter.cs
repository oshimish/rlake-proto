using System;

namespace RlakeFunctionApp.Interfaces
{
    public interface ILoggerAdapter<TType>
    {
        void LogError(Exception exception, string message);
        void LogError(string message);
        void LogInformation(string message);
    }
}
