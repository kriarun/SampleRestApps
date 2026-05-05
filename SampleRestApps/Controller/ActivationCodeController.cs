using Microsoft.AspNetCore.Mvc;
using SampleRestApps.Models;
using SampleRestApps.Services;

namespace SampleRestApps.Controller;

[ApiController]
[Route("api/v1/tenants/{tenantId}/contracts/{contractId}/activation-codes")]
public class ActivationCodeController : ControllerBase
{
    private readonly IActivationCodeService _activationCodeService;
    private readonly ILogger<ActivationCodeController> _logger;

    public ActivationCodeController(
        IActivationCodeService activationCodeService,
        ILogger<ActivationCodeController> logger)
    {
        _activationCodeService = activationCodeService;
        _logger = logger;
    }

    [HttpPost]
    [ProducesResponseType(typeof(ActivationCodeResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status502BadGateway)]
    public async Task<IActionResult> SendActivationCodes(
        [FromRoute] string tenantId,
        [FromRoute] string contractId,
        [FromBody] ActivationCodeRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await _activationCodeService.SendActivationCodesAsync(
                tenantId, contractId, request, cancellationToken);

            return Ok(result);
        }
        catch (ApiException ex)
        {
            _logger.LogError(ex,
                "Downstream failed sending activation codes. TenantId={TenantId}, ContractId={ContractId}, Status={StatusCode}",
                tenantId, contractId, (int)ex.StatusCode);

            return StatusCode(502, new ProblemDetails { Detail = "Downstream API unavailable." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Unexpected error sending activation codes. TenantId={TenantId}, ContractId={ContractId}",
                tenantId, contractId);

            return StatusCode(500, new ProblemDetails { Detail = "An unexpected error occurred." });
        }
    }

    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status502BadGateway)]
    public async Task<IActionResult> DeleteActivationCodes(
        [FromRoute] string tenantId,
        [FromRoute] string contractId,
        CancellationToken cancellationToken)
    {
        try
        {
            await _activationCodeService.DeleteActivationCodesAsync(
                tenantId, contractId, cancellationToken);

            return NoContent();
        }
        catch (ApiException ex)
        {
            _logger.LogError(ex,
                "Downstream failed deleting activation codes. TenantId={TenantId}, ContractId={ContractId}, Status={StatusCode}",
                tenantId, contractId, (int)ex.StatusCode);

            return StatusCode(502, new ProblemDetails { Detail = "Downstream API unavailable." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Unexpected error deleting activation codes. TenantId={TenantId}, ContractId={ContractId}",
                tenantId, contractId);

            return StatusCode(500, new ProblemDetails { Detail = "An unexpected error occurred." });
        }
    }
}
