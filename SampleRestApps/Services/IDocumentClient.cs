namespace SampleRestApps.Services;

public interface IDocumentClient
{
    Task<LetterResponse> SendOneLetterAsync(
        string tenantId,
        string contractId,
        LetterRequest request,
        CancellationToken cancellationToken);

    Task<LetterResponse> SendTwoLettersAsync(
        string tenantId,
        string contractId,
        LetterRequest request,
        CancellationToken cancellationToken);
}
