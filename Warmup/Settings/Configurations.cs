namespace csharp_scalar.Warmup.Settings
{
    public record Configurations(TelemetrySettings Telemetry, string Messaging);
    public record TelemetrySettings(string Exporter);
}