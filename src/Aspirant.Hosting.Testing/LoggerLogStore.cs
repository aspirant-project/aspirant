using System.Collections.Concurrent;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Aspirant.Hosting.Testing;

/// <summary>
/// Stores logs from <see cref="ILogger"/> instances created from <see cref="StoredLogsLoggerProvider"/>.
/// </summary>
public class LoggerLogStore(IHostEnvironment hostEnvironment)
{
    private readonly ConcurrentDictionary<string, List<(DateTimeOffset TimeStamp, string Category, LogLevel Level, string Message, Exception? Exception)>> _store = [];

    /// <summary>
    /// Adds a log entry to the store.
    /// </summary>
    /// <param name="category">The log category.</param>
    /// <param name="level">The log level.</param>
    /// <param name="message">The log message.</param>
    /// <param name="exception">The exception associated with the log entry.</param>
    public void AddLog(string category, LogLevel level, string message, Exception? exception)
    {
        _store.GetOrAdd(category, _ => []).Add((DateTimeOffset.Now, category, level, message, exception));
    }

    /// <summary>
    /// Gets a snapshot of the logs in the store at this moment.
    /// </summary>
    /// <returns>A snapshot of logs stored at this moment.</returns>
    public IReadOnlyDictionary<string, IList<(DateTimeOffset TimeStamp, string Category, LogLevel Level, string Message, Exception? Exception)>> GetLogs()
    {
        return _store.ToDictionary(entry => entry.Key, entry => (IList<(DateTimeOffset, string, LogLevel, string, Exception?)>)entry.Value);
    }

    /// <summary>
    /// Ensures no errors have been logged.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown if errors have been logged.</exception>
    public void EnsureNoErrors()
    {
        var logs = GetLogs();

        var errors = logs.SelectMany(kvp => kvp.Value).Where(log => log.Level == LogLevel.Error || log.Level == LogLevel.Critical).ToList();
        //Where(category => category.Value.Any(log => log.Level == LogLevel.Error || log.Level == LogLevel.Critical)).ToList();
        if (errors.Count > 0)
        {
            var appName = hostEnvironment.ApplicationName;
            throw new InvalidOperationException(
                $"AppHost '{appName}' logged errors: {Environment.NewLine}" +
                string.Join(Environment.NewLine, errors.Select(log => $"[{log.Category}] {log.Message}")));
        }
    }
}
