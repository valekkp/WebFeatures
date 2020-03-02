﻿namespace WebFeatures.Application.Interfaces.Logging
{
    public interface ILogger<T>
    {
        void LogInformation(string message, params object[] args);

        void LogWarning(string message, params object[] args);
    }
}