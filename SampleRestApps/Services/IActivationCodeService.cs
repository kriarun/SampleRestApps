using SampleRestApps.Models;

namespace SampleRestApps.Services;

public interface IActivationCodeService
{
    Task<ActivationCodeResponse> SendActivationCodesAsync(
        string tenantId,
        string contractId,
        ActivationCodeRequest request,
        CancellationToken cancellationToken);

    Task DeleteActivationCodesAsync(
        string tenantId,
        string contractId,
        CancellationToken cancellationToken);
}