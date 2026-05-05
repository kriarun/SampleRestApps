namespace SampleRestApps.Services;

public interface IContractService
{
    Task UploadContractAsync(
        string tenantId,
        string contractId,
        CancellationToken cancellationToken);
}