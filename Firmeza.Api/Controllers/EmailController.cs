using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Firmeza.Api.Services;

namespace Firmeza.Api.Controllers;

/// <summary>
/// Controller for email operations
/// </summary>
[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "Customer")]
public class EmailController : ControllerBase
{
    private readonly IEmailService _emailService;
    private readonly ILogger<EmailController> _logger;

    public EmailController(IEmailService emailService, ILogger<EmailController> logger)
    {
        _emailService = emailService;
        _logger = logger;
    }

    /// <summary>
    /// Send receipt email for a completed sale
    /// </summary>
    /// <param name="saleId">The ID of the sale</param>
    /// <returns>Success message</returns>
    [HttpPost("send-receipt/{saleId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> SendReceipt(int saleId)
    {
        try
        {
            var result = await _emailService.SendReceiptEmailAsync(saleId);
            
            if (!result)
            {
                return NotFound(new { message = "Sale not found or customer email not available" });
            }

            return Ok(new { message = "Receipt email sent successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error sending receipt for sale {saleId}");
            return StatusCode(500, new { message = "An error occurred while sending the receipt" });
        }
    }
}
