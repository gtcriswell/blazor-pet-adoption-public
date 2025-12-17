using Client.Services;

namespace Client.Middleware
{
    public interface IUnhandledExceptionSender
    {
        event EventHandler<Exception> UnhandledExceptionThrown;
    }

    public class UnhandledExceptionSender : ILogger, IUnhandledExceptionSender
    {

        public event EventHandler<Exception> UnhandledExceptionThrown;

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state,
            Exception exception, Func<TState, Exception, string> formatter)
        {
            if (exception != null)
            {
                UnhandledExceptionThrown?.Invoke(this, exception);
            }
        }
    }

    public class UnhandledExceptionProvider : ILoggerProvider
    {
        UnhandledExceptionSender _unhandledExceptionSender;
        private readonly ILogService _logService;

        public UnhandledExceptionProvider(ILogService logService)
        {
            _logService = logService;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new UnhandledExceptionLogger(this._logService, _unhandledExceptionSender);
        }

        public void Dispose()
        {
        }

        public class UnhandledExceptionLogger : ILogger
        {
            private readonly ILogService _logService;
            private readonly UnhandledExceptionSender _unhandeledExceptionSender;

            public UnhandledExceptionLogger(ILogService logService, UnhandledExceptionSender unhandledExceptionSender)
            {
                _unhandeledExceptionSender = unhandledExceptionSender;
                _logService = logService;
            }

            public bool IsEnabled(LogLevel logLevel)
            {
                return true;
            }

            public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
            {
                if (exception != null)
                {
                    if (logLevel >= LogLevel.Warning)
                    {
                        _logService.LogError(new DTO.Client.ClientErrorDto() { Message = exception.Message, StackTrace = exception.StackTrace, OccurredAt = DateTime.UtcNow });
                    }
                }
            }

            public IDisposable BeginScope<TState>(TState state)
            {
                return new NoopDisposable();
            }

            private class NoopDisposable : IDisposable
            {
                public void Dispose()
                {
                }
            }
        }
    }
}
