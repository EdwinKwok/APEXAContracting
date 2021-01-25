using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text;
using System.Linq;

namespace APEXAContracting.Common.Logging
{
    /// <summary>
    ///  Logging in file system.
    ///  Reference: https://andrewlock.net/creating-a-rolling-file-logging-provider-for-asp-net-core-2-0/
    ///  Reference: https://github.com/andrewlock/NetEscapades.Extensions.Logging/tree/master/src/NetEscapades.Extensions.Logging.RollingFile/Internal
    /// </summary>
    public abstract class BatchingLoggerProvider : ILoggerProvider
    {
        private readonly List<LogMessage> _currentBatch = new List<LogMessage>();
        private readonly TimeSpan _interval;
        private readonly int? _queueSize;
        private readonly int? _batchSize;

        private BlockingCollection<LogMessage> _messageQueue;
        private Task _outputTask;
        private CancellationTokenSource _cancellationTokenSource;
        /// <summary>
        ///  Default is <c>None</c>, means no logging.
        /// </summary>
        private LogLevel[] _logLevels = new LogLevel[] { LogLevel.None };
        private bool _isEnabled = true;

        protected BatchingLoggerProvider(IOptions<BatchingLoggerOptions> options)
        {
            // NOTE: Only IsEnabled is monitored

            var loggerOptions = options.Value;
            if (loggerOptions.BatchSize <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(loggerOptions.BatchSize), $"{nameof(loggerOptions.BatchSize)} must be a positive number.");
            }
            if (loggerOptions.FlushPeriod <= TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException(nameof(loggerOptions.FlushPeriod), $"{nameof(loggerOptions.FlushPeriod)} must be longer than zero.");
            }

            _interval = loggerOptions.FlushPeriod;
            _batchSize = loggerOptions.BatchSize;
            _queueSize = loggerOptions.BackgroundQueueSize;
            _isEnabled = loggerOptions.IsEnabled;

            if (!string.IsNullOrEmpty(loggerOptions.LogLevels))
            {
                try
                {
                    _logLevels = loggerOptions.LogLevels.Split(",".ToArray(), StringSplitOptions.RemoveEmptyEntries).Where(c => Enum.IsDefined(typeof(LogLevel), c))
                                 .Select(c => (LogLevel)Enum.Parse(typeof(LogLevel), c)).ToArray();
                }
                catch
                {
                    _logLevels = new LogLevel[] { LogLevel.None };
                }
             }

            Start();
        }

        protected abstract Task WriteMessagesAsync(IEnumerable<LogMessage> messages, CancellationToken token);

        private async Task ProcessLogQueue(object state)
        {
            while (!_cancellationTokenSource.IsCancellationRequested)
            {
                var limit = _batchSize ?? int.MaxValue;

                while (limit > 0 && _messageQueue.TryTake(out var message))
                {
                    _currentBatch.Add(message);
                    limit--;
                }

                if (_currentBatch.Count > 0)
                {
                    try
                    {
                        await WriteMessagesAsync(_currentBatch, _cancellationTokenSource.Token);
                    }
                    catch
                    {
                        // ignored
                    }

                    _currentBatch.Clear();
                }

                await IntervalAsync(_interval, _cancellationTokenSource.Token);
            }
        }

        protected virtual Task IntervalAsync(TimeSpan interval, CancellationToken cancellationToken)
        {
            return Task.Delay(interval, cancellationToken);
        }

        internal void AddMessage(DateTimeOffset timestamp, string message)
        {
            if (!_messageQueue.IsAddingCompleted)
            {
                try
                {
                    _messageQueue.Add(new LogMessage { Message = message, Timestamp = timestamp }, _cancellationTokenSource.Token);
                }
                catch
                {
                    //cancellation token canceled or CompleteAdding called
                }
            }
        }

        private void Start()
        {
            _messageQueue = _queueSize == null ?
                new BlockingCollection<LogMessage>(new ConcurrentQueue<LogMessage>()) :
                new BlockingCollection<LogMessage>(new ConcurrentQueue<LogMessage>(), _queueSize.Value);

            _cancellationTokenSource = new CancellationTokenSource();
            _outputTask = Task.Factory.StartNew<Task>(
                ProcessLogQueue,
                null,
                TaskCreationOptions.LongRunning);
        }

        private void Stop()
        {
            _cancellationTokenSource.Cancel();
            _messageQueue.CompleteAdding();

            try
            {
                _outputTask.Wait(_interval);
            }
            catch (TaskCanceledException)
            {
            }
            catch (AggregateException ex) when (ex.InnerExceptions.Count == 1 && ex.InnerExceptions[0] is TaskCanceledException)
            {
            }
        }

        public void Dispose()
        {
            Stop();
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new BatchingLogger(this, categoryName);
        }

        /// <summary>
        ///  Expose the custom log levels which are going to be log.
        /// </summary>
        public LogLevel[] LogLevels {
            get { return _logLevels; }
            set { _logLevels = value; }
        }

        /// <summary>
        ///  Enable log or disable log feature. Default value = true.
        /// </summary>
        public bool IsEnabled {
            get { return _isEnabled; }
            set { _isEnabled = value; }
        }
    }
    
    public class BatchingLoggerOptions
    {
        private int? _batchSize = 32;
        private int? _backgroundQueueSize;
        private TimeSpan _flushPeriod = TimeSpan.FromSeconds(1);

        /// <summary>
        /// Gets or sets the period after which logs will be flushed to the store.
        /// </summary>
        public TimeSpan FlushPeriod
        {
            get { return _flushPeriod; }
            set
            {
                if (value <= TimeSpan.Zero)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), $"{nameof(FlushPeriod)} must be positive.");
                }
                _flushPeriod = value;
            }
        }

        /// <summary>
        /// Gets or sets the maximum size of the background log message queue or null for no limit.
        /// After maximum queue size is reached log event sink would start blocking.
        /// Defaults to <c>null</c>.
        /// </summary>
        public int? BackgroundQueueSize
        {
            get { return _backgroundQueueSize; }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), $"{nameof(BackgroundQueueSize)} must be non-negative.");
                }
                _backgroundQueueSize = value;
            }
        }

        /// <summary>
        /// Gets or sets a maximum number of events to include in a single batch or null for no limit.
        /// </summary>
        public int? BatchSize
        {
            get { return _batchSize; }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), $"{nameof(BatchSize)} must be positive.");
                }
                _batchSize = value;
            }
        }

        private bool _isEnabled = true;
        /// <summary>
        /// Gets or sets value indicating if logger accepts and queues writes. Default value = true.
        /// </summary>
        public bool IsEnabled { get { return _isEnabled; } set { _isEnabled = value; } }

        private string _logLevels = "None";
        /// <summary>
        ///  System is able to custom define which level of system state is going to be log.
        ///  
        ///  String contains LogLevel definitions and splitor is ","
        ///  Format: Trace,Information,Warning,Error,Critical,None
        ///  
        ///  Default value is <c>None</c>
        ///  
        ///  Note: "None" cannot be combine with other log level setting.
        ///  If set value = "None", means file logger is disabled. Same as set IsEnabled = false.
        ///  
        /// Summary:
        ///     Logs that contain the most detailed messages. These messages may contain sensitive
        ///     application data. These messages are disabled by default and should never be
        ///     enabled in a production environment.
        ///  Trace = 0,
        ///
        /// Summary:
        ///     Logs that are used for interactive investigation during development. These logs
        ///     should primarily contain information useful for debugging and have no long-term
        ///     value.
        /// Debug = 1,
        ///
        /// Summary:
        ///     Logs that track the general flow of the application. These logs should have long-term
        ///     value.
        /// Information = 2,
        ///
        /// Summary:
        ///     Logs that highlight an abnormal or unexpected event in the application flow,
        ///     but do not otherwise cause the application execution to stop.
        /// Warning = 3,
        ///
        /// Summary:
        ///     Logs that highlight when the current flow of execution is stopped due to a failure.
        ///     These should indicate a failure in the current activity, not an application-wide
        ///     failure.
        /// Error = 4,
        ///
        /// Summary:
        ///     Logs that describe an unrecoverable application or system crash, or a catastrophic
        ///    failure that requires immediate attention.
        /// Critical = 5,
        ///
        /// Summary:
        ///     Not used for writing log messages. Specifies that a logging category should not
        ///     write any messages.
        /// None = 6
        /// </summary>
        public string LogLevels { get { return _logLevels; } set { _logLevels = value; } } 
    }

    public struct LogMessage
    {
        public DateTimeOffset Timestamp { get; set; }
        public string Message { get; set; }
    }

    public class BatchingLogger : ILogger
    {
        private readonly BatchingLoggerProvider _provider;
        private readonly string _category;
        
        public BatchingLogger(BatchingLoggerProvider loggerProvider, string categoryName)
        {
            _provider = loggerProvider;
            _category = categoryName;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        /// <summary>
        ///  Define enable logging current log or not. 
        ///  Depend on settings in appsettings.json.
        /// </summary>
        /// <param name="logLevel"></param>
        /// <returns></returns>
        public bool IsEnabled(LogLevel logLevel)
        {
            if (!_provider.IsEnabled || logLevel == LogLevel.None || !_provider.LogLevels.Contains(logLevel))
            {
                return false;
            }

            return true;
        }

        public void Log<TState>(DateTimeOffset timestamp, LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }

            var builder = new StringBuilder();
            builder.Append(timestamp.ToString("yyyy-MM-dd HH:mm:ss.fff zzz"));
            builder.Append(" [");
            builder.Append(logLevel.ToString());
            builder.Append("] ");
            builder.Append(_category);
            builder.Append(": ");
            builder.AppendLine(formatter(state, exception));

            if (exception != null)
            {
                builder.AppendLine(exception.ToString());
            }

            _provider.AddMessage(timestamp, builder.ToString());
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            Log(DateTimeOffset.Now, logLevel, eventId, state, exception, formatter);
        }
    }
}

