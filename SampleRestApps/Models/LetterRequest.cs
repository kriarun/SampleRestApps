using System.ComponentModel.DataAnnotations;

namespace SampleRestApps.Models;

public record LetterRequest
{
    [Required]
    public List<string> ActivationCodes { get; init; } = new();
}