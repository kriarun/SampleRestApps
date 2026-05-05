namespace SampleRestApps.Models;

public record ActivationCodeResponse
{
    public string   Id             { get; init; } = string.Empty;
    public string   TenantId       { get; init; } = string.Empty;
    public string   ContractId     { get; init; } = string.Empty;
    public DateTime ValidTo        { get; init; }
    public string   MetaData       { get; init; } = string.Empty;
    public string   Purpose        { get; init; } = string.Empty;
    public DateTime CreatedAt      { get; init; }
    public string   CreatedBy      { get; init; } = string.Empty;
    public string   ActivationCode { get; init; } = string.Empty;
}