namespace SampleRestApps.Services;

public interface IDocumentService
{
    Task<LetterResponse> SendLetterAsync(
        string tenantId,
        string contractId,
        LetterRequest request,
        LetterType letterType,
        CancellationToken cancellationToken);
}