using System;
using System.IO;
using static LoggerFactory;

public class LoggerFactory
{
    public enum LogLevel
    {
        Info,
        Warning,
        Error
    }

    public interface ILogger
    {
        void Log(LogLevel level, string message);
        void Info(string message);
        void Warning(string message);
        void Error(string message);
    }

    private class ConsoleLogger : ILogger
    {
        public void Log(LogLevel level, string message)
        {
            Console.WriteLine($"[{level}] {message}");
        }

        public void Info(string message) => Log(LogLevel.Info, message);
        public void Warning(string message) => Log(LogLevel.Warning, message);
        public void Error(string message) => Log(LogLevel.Error, message);
    }

    private class FileLogger : ILogger
    {
        private readonly string _filePath;

        public FileLogger(string filePath)
        {
            _filePath = filePath;
        }

        public void Log(LogLevel level, string message)
        {
            string logEntry = $"[{DateTime.Now}] [{level}] {message}";
            File.AppendAllText(_filePath, logEntry + Environment.NewLine);
        }

        public void Info(string message) => Log(LogLevel.Info, message);
        public void Warning(string message) => Log(LogLevel.Warning, message);
        public void Error(string message) => Log(LogLevel.Error, message);
    }

    public static ILogger CreateLogger(string type, string? filePath = null)
    {
        return type.ToLower() switch
        {
            "console" => new ConsoleLogger(),
            "file" when filePath != null => new FileLogger(filePath),
            _ => throw new ArgumentException("Invalid logger type or missing file path for file logger.")
        };
    }
}

public class LoggerUser
{
    private readonly ILogger _logger;

    public LoggerUser(ILogger logger)
    {
        _logger = logger;
    }

    public void DoSomething()
    {
        _logger.Info("LoggerUser is doing something.");
    }
}
