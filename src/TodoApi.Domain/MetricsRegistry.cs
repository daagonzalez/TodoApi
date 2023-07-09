using Prometheus;

namespace TodoApi.Domain;

public class MetricsRegistry
{
    public static readonly Counter TodoEntryCreated = Metrics.CreateCounter(
        "todo_entry_created",
        "Total number of Todo entries created."
    );

    public static readonly Histogram DatabaseReadDuration = Metrics.CreateHistogram(
        "webapi_database_read_duration_seconds",
        "Duration of the database read operation",
        new HistogramConfiguration
        {
            Buckets = Histogram.LinearBuckets(start: 0, width: 1, count: 10), // Adjust bucket configuration as per your needs
            LabelNames = new[] { "operation" }
        }
    );
}