namespace SampleRestApps.Models;

public record LetterResponse
{
    public bool   IsPrinted { get; init; }
    public string Message   { get; init; } = string.Empty;
}
