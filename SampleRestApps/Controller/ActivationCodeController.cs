using Microsoft.AspNetCore.Mvc;
using SampleRestApps.Models;
using SampleRestApps.Services;

namespace SampleRestApps.Controller;

[ApiExceptionFilter]
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
        var result = await _activationCodeService.SendActivationCodesAsync(
            tenantId, contractId, request, cancellationToken);

        return Ok(result);
    }

    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status502BadGateway)]
    public async Task<IActionResult> DeleteActivationCodes(
        [FromRoute] string tenantId,
        [FromRoute] string contractId,
        CancellationToken cancellationToken)
    {
        await _activationCodeService.DeleteActivationCodesAsync(
            tenantId, contractId, cancellationToken);

        return NoContent();
    }
}