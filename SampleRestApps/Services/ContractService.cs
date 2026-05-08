namespace SampleRestApps.Services;

public class ContractService : IContractService
{
    private readonly IContractClient _client; // NSwag generated
    private readonly ILogger<ContractService> _logger;

    public ContractService(IContractClient client, ILogger<ContractService> logger)
    {
        _client = client;
        _logger = logger;
    }

    public async Task UploadContractAsync(
        string tenantId,
        string contractId,
        CancellationToken cancellationToken)
    {
        await _client.UploadContractAsync(tenantId, contractId, cancellationToken);
    }
}