using System.Diagnostics.Metrics;

namespace Alpha.Gateway.OpenTelemetry;

public static class DiagnosticsConfig
{
    public const string ServiceName = "Alpha Gateway Service";
    public static readonly Meter Meter = new(ServiceName);
    public static readonly Histogram<double> TimeSpend = Meter.CreateHistogram<double>("gateway_time_spend");
    public static readonly Counter<long> TotalCalls = Meter.CreateCounter<long>("gateway_total_calls");
}