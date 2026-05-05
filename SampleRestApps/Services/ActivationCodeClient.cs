using System.Net;
using System.Net.Http.Json;
using SampleRestApps.Models;

namespace SampleRestApps.Services;

public class ActivationCodeClient : IActivationCodeClient
{
    private readonly HttpClient _httpClient;

    public ActivationCodeClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ActivationCodeResponse> SendActivationCodesAsync(
        string tenantId,
        string contractId,
        ActivationCodeRequest request,
        CancellationToken cancellationToken)
    {
        var response = await _httpClient.PostAsJsonAsync(
            BuildActivationCodePath(tenantId, contractId),
            request,
            cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            throw await CreateApiExceptionAsync(
                "Downstream API failed sending activation codes.",
                response,
                cancellationToken);
        }

        var result = await response.Content.ReadFromJsonAsync<ActivationCodeResponse>(cancellationToken);
        return result ?? throw new ApiException(
            "Downstream API returned an empty activation code response.",
            HttpStatusCode.InternalServerError);
    }

    public async Task DeleteActivationCodesAsync(
        string tenantId,
        string contractId,
        CancellationToken cancellationToken)
    {
        var response = await _httpClient.DeleteAsync(
            BuildActivationCodePath(tenantId, contractId),
            cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            throw await CreateApiExceptionAsync(
                "Downstream API failed deleting activation codes.",
                response,
                cancellationToken);
        }
    }

    private static string BuildActivationCodePath(string tenantId, string contractId)
        => $"api/v1/tenants/{Uri.EscapeDataString(tenantId)}/contracts/{Uri.EscapeDataString(contractId)}/activation-codes";

    private static async Task<ApiException> CreateApiExceptionAsync(
        string message,
        HttpResponseMessage response,
        CancellationToken cancellationToken)
    {
        var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);
        return new ApiException(message, response.StatusCode, responseBody);
    }
}
