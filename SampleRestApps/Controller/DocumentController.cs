namespace SampleRestApps.Controller;
[ApiExceptionFilter]
[ApiController]
[Route("api/v1/tenants/{tenantId}/documents/{contractId}")]
public class DocumentController : ControllerBase
{
    private readonly IDocumentService _documentService;
    private readonly ILogger<DocumentController> _logger;

    public DocumentController(IDocumentService documentService, ILogger<DocumentController> logger)
    {
        _documentService = documentService;
        _logger = logger;
    }

    [HttpPost("one-letter")]
    [ProducesResponseType(typeof(LetterResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status502BadGateway)]
    public async Task<IActionResult> SendOneLetter(
        [FromRoute] string tenantId,
        [FromRoute] string contractId,
        [FromBody] LetterRequest request,
        CancellationToken cancellationToken)
        => await SendLetter(tenantId, contractId, request, LetterType.OneLetter, cancellationToken);

    [HttpPost("two-letters")]
    [ProducesResponseType(typeof(LetterResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status502BadGateway)]
    public async Task<IActionResult> SendTwoLetters(
        [FromRoute] string tenantId,
        [FromRoute] string contractId,
        [FromBody] LetterRequest request,
        CancellationToken cancellationToken)
        => await SendLetter(tenantId, contractId, request, LetterType.TwoLetters, cancellationToken);

    private async Task<IActionResult> SendLetter(
        string tenantId,
        string contractId,
        LetterRequest request,
        LetterType letterType,
        CancellationToken cancellationToken)
    {
       
            var result = await _documentService.SendLetterAsync(
                tenantId, contractId, request, letterType, cancellationToken);

            return Ok(result);
      
       
    }
}