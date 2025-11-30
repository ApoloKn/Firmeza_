namespace Firmeza.Api.Services;

/// <summary>
/// Interface for email service operations
/// </summary>
public interface IEmailService
{
    /// <summary>
    /// Send a receipt email for a completed sale
    /// </summary>
    /// <param name="saleId">The ID of the sale</param>
    /// <returns>True if email sent successfully, false otherwise</returns>
    Task<bool> SendReceiptEmailAsync(int saleId);
}
