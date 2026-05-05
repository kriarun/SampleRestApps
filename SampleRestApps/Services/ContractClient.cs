namespace SampleRestApps.Services;

public class ContractClient : IContractClient
{
    private readonly HttpClient _httpClient;

    public ContractClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task UploadContractAsync(
        string tenantId,
        string contractId,
        CancellationToken cancellationToken)
    {
        var response = await _httpClient.PostAsync(
            $"api/v1/tenants/{Uri.EscapeDataString(tenantId)}/contracts/{Uri.EscapeDataString(contractId)}/upload",
            content: null,
            cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);
            throw new ApiException(
                "Downstream API failed uploading contract.",
                response.StatusCode,
                responseBody);
        }
    }
}
