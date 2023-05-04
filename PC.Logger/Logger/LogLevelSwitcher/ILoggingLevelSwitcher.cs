using Serilog.Events;

namespace PC.Logger.Logger.LogLevelSwitcher
{
    /// <summary>
    /// Project logging levels switcher
    /// </summary>
    public interface ILoggingLevelSwitcher
    {
        /// <summary>
        /// Minimum global logging level
        /// </summary>
        LogEventLevel GlobalLogLevel { get; set; }
    }
}