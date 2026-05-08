using SampleRestApps.Models;

namespace SampleRestApps.Services;

public class ActivationCodeService : IActivationCodeService
{
    private readonly IActivationCodeClient _client; // NSwag generated
    private readonly ILogger<ActivationCodeService> _logger;

    public ActivationCodeService(IActivationCodeClient client, ILogger<ActivationCodeService> logger)
    {
        _client = client;
        _logger = logger;
    }

    public async Task<ActivationCodeResponse> SendActivationCodesAsync(
        string tenantId,
        string contractId,
        ActivationCodeRequest request,
        CancellationToken cancellationToken)
    {
        return await _client.SendActivationCodesAsync(tenantId, contractId, request, cancellationToken);
    }

    public async Task DeleteActivationCodesAsync(
        string tenantId,
        string contractId,
        CancellationToken cancellationToken)
    {
        await _client.DeleteActivationCodesAsync(tenantId, contractId, cancellationToken);
    }
}