using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace APEXAContracting.Common.Logging
{
    /// <summary>
    /// An <see cref="ILoggerProvider" /> that writes logs to a file
    /// </summary>
    [ProviderAlias("File")]
    public class FileLoggerProvider : BatchingLoggerProvider
    {
        private readonly string _path;
        private readonly string _fileName;
        private readonly int? _maxFileSize;
        private readonly int? _maxRetainedFiles;

        /// <summary>
        /// Creates an instance of the <see cref="FileLoggerProvider" /> 
        /// Dependency injection rely on codes in Startup.cs:
        /// services.Configure[FileLoggerOptions](Configuration.GetSection("Logging:File"));
        /// Setting in appsettings.json section "Logging:File".
        /// </summary>
        /// <param name="options">The options object controlling the logger</param>
        public FileLoggerProvider(IOptions<FileLoggerOptions> options) : base(options)
        {
            var loggerOptions = options.Value;
            _path = loggerOptions.LogDirectory;
            _fileName = loggerOptions.FileName;
            _maxFileSize = loggerOptions.FileSizeLimit;
            _maxRetainedFiles = loggerOptions.RetainedFileCountLimit;
        }

        /// <inheritdoc />
        protected override async Task WriteMessagesAsync(IEnumerable<LogMessage> messages, CancellationToken cancellationToken)
        {
            Directory.CreateDirectory(_path);

            foreach (var group in messages.GroupBy(GetGrouping))
            {
                var fullName = GetFullName(group.Key);
                var fileInfo = new FileInfo(fullName);
                if (_maxFileSize > 0 && fileInfo.Exists && fileInfo.Length > _maxFileSize)
                {
                    return;
                }

                using (var streamWriter = File.AppendText(fullName))
                {
                    foreach (var item in group)
                    {
                        await streamWriter.WriteAsync(item.Message);
                    }
                }
            }

            RollFiles();
        }

        private string GetFullName((int Year, int Month, int Day) group)
        {
            return Path.Combine(_path, $"{_fileName}{group.Year:0000}{group.Month:00}{group.Day:00}.txt");
        }

        private (int Year, int Month, int Day) GetGrouping(LogMessage message)
        {
            return (message.Timestamp.Year, message.Timestamp.Month, message.Timestamp.Day);
        }

        /// <summary>
        /// Deletes old log files, keeping a number of files defined by <see cref="FileLoggerOptions.RetainedFileCountLimit" />
        /// </summary>
        protected void RollFiles()
        {
            if (_maxRetainedFiles > 0)
            {
                var files = new DirectoryInfo(_path)
                    .GetFiles(_fileName + "*")
                    .OrderByDescending(f => f.Name)
                    .Skip(_maxRetainedFiles.Value);

                foreach (var item in files)
                {
                    item.Delete();
                }
            }
        }
    }

    /// <summary>
    /// Options for file logging.
    /// </summary>
    public class FileLoggerOptions : BatchingLoggerOptions
    {
        private int? _fileSizeLimit = 10 * 1024 * 1024;
        private int? _retainedFileCountLimit = 100;
        private string _fileName = "logs-";


        /// <summary>
        /// Gets or sets a strictly positive value representing the maximum log size in bytes or null for no limit.
        /// Once the log is full, no more messages will be appended.
        /// Defaults to <c>10MB</c>.
        /// </summary>
        public int? FileSizeLimit
        {
            get { return _fileSizeLimit; }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), $"{nameof(FileSizeLimit)} must be positive.");
                }
                _fileSizeLimit = value;
            }
        }

        /// <summary>
        /// Gets or sets a strictly positive value representing the maximum retained file count or null for no limit.
        /// Defaults to <c>100</c>.
        /// </summary>
        public int? RetainedFileCountLimit
        {
            get { return _retainedFileCountLimit; }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), $"{nameof(RetainedFileCountLimit)} must be positive.");
                }
                _retainedFileCountLimit = value;
            }
        }

        /// <summary>
        /// Gets or sets the filename prefix to use for log files.
        /// Defaults to <c>logs-</c>.
        /// </summary>
        public string FileName
        {
            get { return _fileName; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentException(nameof(value));
                }
                _fileName = value;
            }
        }

        /// <summary>
        /// The directory in which log files will be written, relative to the app process.
        /// Default to <c>Logs</c> where under current application's folder.
        /// It support full path such as "c:\Logs".
        /// </summary>
        /// <returns></returns>
        public string LogDirectory { get; set; } = "Logs";
        
    }
}
