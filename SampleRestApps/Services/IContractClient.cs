namespace SampleRestApps.Services;

public interface IContractClient
{
    Task UploadContractAsync(
        string tenantId,
        string contractId,
        CancellationToken cancellationToken);
}
