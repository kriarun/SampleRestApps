using System.Net;
using System.Net.Http.Json;

namespace SampleRestApps.Services;

public class DocumentClient : IDocumentClient
{
    private readonly HttpClient _httpClient;

    public DocumentClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public Task<LetterResponse> SendOneLetterAsync(
        string tenantId,
        string contractId,
        LetterRequest request,
        CancellationToken cancellationToken)
        => SendLetterAsync(tenantId, contractId, "one-letter", request, cancellationToken);

    public Task<LetterResponse> SendTwoLettersAsync(
        string tenantId,
        string contractId,
        LetterRequest request,
        CancellationToken cancellationToken)
        => SendLetterAsync(tenantId, contractId, "two-letters", request, cancellationToken);

    private async Task<LetterResponse> SendLetterAsync(
        string tenantId,
        string contractId,
        string endpoint,
        LetterRequest request,
        CancellationToken cancellationToken)
    {
        var response = await _httpClient.PostAsJsonAsync(
            $"api/v1/tenants/{Uri.EscapeDataString(tenantId)}/documents/{Uri.EscapeDataString(contractId)}/{endpoint}",
            request,
            cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);
            throw new ApiException(
                "Downstream API failed sending document letter.",
                response.StatusCode,
                responseBody);
        }

        var result = await response.Content.ReadFromJsonAsync<LetterResponse>(cancellationToken);
        return result ?? throw new ApiException(
            "Downstream API returned an empty letter response.",
            HttpStatusCode.InternalServerError);
    }
}
