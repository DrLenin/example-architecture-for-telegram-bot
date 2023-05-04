using Newtonsoft.Json;
using PC.Logger.Logger.Converters;
using Serilog.Events;

namespace PC.Logger.Logger.Http.Models
{
    /// <summary>
    /// Logging endpoint request params
    /// </summary>
    public class LoggingEndpointParams
    {
        /// <summary>
        /// Min loglevel for whole application
        /// </summary>
        [JsonConverter(typeof(CustomStringEnumConverter))]
        public LogEventLevel Loglevel { get; set; }
    }
}