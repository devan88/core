using Serilog.Formatting;
using Serilog.Formatting.Elasticsearch;
using Serilog.Sinks.Http;
using Serilog.Sinks.Http.BatchFormatters;

namespace Core.Logging.Serilog
{
    public sealed record LogstashOptions
    {
        public string Url { get; set; }

        public string BufferBaseFileName { get; set; } = "logs/Buffer";

        public IBatchFormatter BatchFormatter { get; set; } = new ArrayBatchFormatter();

        public ITextFormatter TextFormatter { get; set; } = new ElasticsearchJsonFormatter();
    }
}
