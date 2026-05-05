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
        try
        {
            return await _client.SendActivationCodesAsync(tenantId, contractId, request, cancellationToken);
        }
        catch (ApiException ex)
        {
            var error = ex.TryDeserializeError();

            _logger.LogError(ex,
                "Downstream rejected activation code send. TenantId={TenantId}, ContractId={ContractId}, Code={Code}, Subsystem={Subsystem}",
                tenantId, contractId, error?.Code, error?.Subsystem);

            throw;
        }
    }

    public async Task DeleteActivationCodesAsync(
        string tenantId,
        string contractId,
        CancellationToken cancellationToken)
    {
        try
        {
            await _client.DeleteActivationCodesAsync(tenantId, contractId, cancellationToken);
        }
        catch (ApiException ex)
        {
            var error = ex.TryDeserializeError();

            _logger.LogError(ex,
                "Downstream rejected activation code delete. TenantId={TenantId}, ContractId={ContractId}, Code={Code}, Subsystem={Subsystem}",
                tenantId, contractId, error?.Code, error?.Subsystem);

            throw;
        }
    }
}