using Serilog;
using Serilog.Events;
using Serilog.Sinks.Elasticsearch;

namespace Core.Logging.Serilog
{
    /// <summary>
    /// Provides extensions for configuring the Serilog logger.
    /// </summary>
    public static class LoggerConfigurationExtensions
    {
        private const string OutputTemplate =
            "[{Timestamp:yyyy-MM-dd HH:mm:ss}] [{Level}] [{MachineName}] [{ThreadId}] [{CorrelationId}] {Message}{NewLine}{Exception}";

        /// <summary>
        /// Configures the minimum log level for the logger.
        /// </summary>
        /// <param name="loggerConfiguration">The logger configuration to modify.</param>
        /// <returns>The modified logger configuration.</returns>
        public static LoggerConfiguration WithMinimumLevel(this LoggerConfiguration loggerConfiguration)
        {
            return loggerConfiguration
                .MinimumLevel.Information()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information);
        }

        /// <summary>
        /// Adds enrichers to the logger configuration to include additional log context information.
        /// </summary>
        /// <param name="loggerConfiguration">The logger configuration to modify.</param>
        /// <returns>The modified logger configuration.</returns>
        public static LoggerConfiguration WithEnrichers(this LoggerConfiguration loggerConfiguration)
        {
            return loggerConfiguration
                .Enrich.FromLogContext()
                .Enrich.WithMachineName()
                .Enrich.WithThreadId();
        }

        /// <summary>
        /// Configures the logger to write log output to the console.
        /// </summary>
        /// <param name="loggerConfiguration">The logger configuration to modify.</param>
        /// <returns>The modified logger configuration.</returns>
        public static LoggerConfiguration WithConsole(this LoggerConfiguration loggerConfiguration)
        {
            return loggerConfiguration
                .WriteTo.Console(outputTemplate: OutputTemplate);
        }

        /// <summary>
        /// Configures the logger to write log output to a file.
        /// </summary>
        /// <param name="loggerConfiguration">The logger configuration to modify.</param>
        /// <param name="path">The path to the log file. Defaults to "logs/appLog.txt" if not specified.</param>
        /// <returns>The modified logger configuration.</returns>
        public static LoggerConfiguration WithFile(
            this LoggerConfiguration loggerConfiguration,
            string path = "logs/log-.txt")
        {
            return loggerConfiguration
                .WriteTo.File(
                path: path,
                rollingInterval: RollingInterval.Day,
                outputTemplate: OutputTemplate);
        }

        public static LoggerConfiguration WithLogstash(
            this LoggerConfiguration loggerConfiguration,
            Action<LogstashOptions> configureOptions)
        {
            LogstashOptions logstashOptions = new();
            configureOptions(logstashOptions);

            return loggerConfiguration
               .WriteTo.DurableHttpUsingFileSizeRolledBuffers(
               requestUri: logstashOptions.Url,
               bufferBaseFileName: logstashOptions.BufferBaseFileName,
               batchFormatter: logstashOptions.BatchFormatter,
               textFormatter: logstashOptions.TextFormatter);
        }

        public static LoggerConfiguration WithElasticSearch(
            this LoggerConfiguration loggerConfiguration,
            string elasticSearchUrl,
            Action<ElasticsearchSinkOptions>? configureOptions = null)
        {
            // Configure Serilog to log to Elasticsearch
            // string elasticUri = "https://localhost:9200"; // Change if your Elasticsearch is hosted
            ElasticsearchSinkOptions elasticsearchSinkOptions = new(new Uri(elasticSearchUrl))
            {
                AutoRegisterTemplate = true,
                IndexFormat = "logs-{0:yyyy.MM.dd}",
            };
            configureOptions?.Invoke(elasticsearchSinkOptions);

            return loggerConfiguration
                .WriteTo.Elasticsearch(elasticsearchSinkOptions);
                //.WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(elasticUri))
                //{
                //    AutoRegisterTemplate = true,
                //    IndexFormat = "logs-{0:yyyy.MM.dd}",
                //    ModifyConnectionSettings = x => x
                //    .BasicAuthentication("elastic", "changeme") // Set up basic authentication
                //    .ServerCertificateValidationCallback((sender, certificate, chain, sslPolicyErrors) => true) // Bypass SSL validation
                //});
        }
    }
}
