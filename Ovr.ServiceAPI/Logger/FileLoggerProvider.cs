namespace GoNetPos.ServiceApi.Logger
{
    public class FileLoggerProvider : ILoggerProvider
    {
        private string logDirectory;
        private LogLevel logLevel;

        public FileLoggerProvider(string directory, LogLevel level)
        {
            logDirectory = directory;
            logLevel = level;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new FileLogger(logDirectory, logLevel, categoryName);
        }

        public void Dispose()
        {
        }

        public static ILogger CreateLoggerInstance()
        {
            using var loggerFactory = LoggerFactory.Create(loggingBuilder => loggingBuilder
            .SetMinimumLevel(LogLevel.Trace)
            .AddConsole());

            return loggerFactory.CreateLogger("DbInit");
        }

        public static ILoggerFactory CreateLoggerFactory()
        {
            using var loggerFactory = LoggerFactory.Create(loggingBuilder => loggingBuilder
            .SetMinimumLevel(LogLevel.Trace)
            .AddConsole());

            return loggerFactory;
        }
    }
}
