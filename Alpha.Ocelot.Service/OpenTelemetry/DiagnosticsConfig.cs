using System.Diagnostics.Metrics;

namespace Alpha.Ocelot.OpenTelemetry;

public static class DiagnosticsConfig
{
    public const string ServiceName = "Alpha Ocelot Service";
    public static readonly Meter Meter = new(ServiceName);
    public static readonly Histogram<double> TimeSpend = Meter.CreateHistogram<double>("ocelot_time_spend");
    public static readonly Counter<long> TotalCalls = Meter.CreateCounter<long>("ocelot_total_calls");
}