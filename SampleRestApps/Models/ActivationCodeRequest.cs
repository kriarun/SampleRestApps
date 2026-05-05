using System.ComponentModel.DataAnnotations;

namespace SampleRestApps.Models;

public record ActivationCodeRequest
{
    [Required]
    public DateTime ValidTo  { get; init; }

    [Required]
    public string MetaData   { get; init; } = string.Empty;

    [Required]
    public string Purpose    { get; init; } = string.Empty;

    [Required]
    public int Length        { get; init; }
}
