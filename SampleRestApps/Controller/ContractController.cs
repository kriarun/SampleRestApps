namespace SampleRestApps.Controller;
[ApiController]
[Route("api/v1/tenants/{tenantId}/contracts/{contractId}")]
public class ContractController : ControllerBase
{
    private readonly IContractService _contractService;
    private readonly ILogger<ContractController> _logger;

    public ContractController(IContractService contractService, ILogger<ContractController> logger)
    {
        _contractService = contractService;
        _logger = logger;
    }

    [HttpPost("upload")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status502BadGateway)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UploadContract(
        [FromRoute] string tenantId,
        [FromRoute] string contractId,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "UploadContract called. TenantId={TenantId}, ContractId={ContractId}",
            tenantId, contractId);

        try
        {
            await _contractService.UploadContractAsync(tenantId, contractId, cancellationToken);
            return Ok();
        }
        catch (ApiException ex)
        {
            _logger.LogError(ex,
                "Downstream failed uploading contract. TenantId={TenantId}, ContractId={ContractId}, Status={StatusCode}",
                tenantId, contractId, (int)ex.StatusCode);

            return StatusCode(502, new ProblemDetails { Detail = "Downstream API unavailable." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Unexpected error uploading contract. TenantId={TenantId}, ContractId={ContractId}",
                tenantId, contractId);

            return StatusCode(500, new ProblemDetails { Detail = "An unexpected error occurred." });
        }
    }
}