namespace SampleRestApps.Services;

public class DocumentService : IDocumentService
{
    private readonly IDocumentClient _client; // NSwag generated
    private readonly ILogger<DocumentService> _logger;

    public DocumentService(IDocumentClient client, ILogger<DocumentService> logger)
    {
        _client = client;
        _logger = logger;
    }

    public async Task<LetterResponse> SendLetterAsync(
        string tenantId,
        string contractId,
        LetterRequest request,
        LetterType letterType,
        CancellationToken cancellationToken)
    {
        try
        {
            return letterType switch
            {
                LetterType.OneLetter  => await _client.SendOneLetterAsync(tenantId, contractId, request, cancellationToken),
                LetterType.TwoLetters => await _client.SendTwoLettersAsync(tenantId, contractId, request, cancellationToken),
                _                     => throw new ArgumentOutOfRangeException(nameof(letterType))
            };
        }
        catch (ApiException ex)
        {
            var error = ex.TryDeserializeError();

            _logger.LogError(ex,
                "Downstream rejected {LetterType}. TenantId={TenantId}, ContractId={ContractId}, Code={Code}, Subsystem={Subsystem}",
                letterType, tenantId, contractId, error?.Code, error?.Subsystem);

            throw;
        }
    }
}