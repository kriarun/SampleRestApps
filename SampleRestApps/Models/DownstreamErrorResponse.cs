namespace SampleRestApps.Models;

public record DownstreamErrorResponse
{
    public string Message   { get; init; } = string.Empty;
    public string Code      { get; init; } = string.Empty;
    public string Subsystem { get; init; } = string.Empty;
}