using System.Net;
using System.Net.Mail;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Firmeza.Data.Data;

namespace Firmeza.Api.Services;

/// <summary>
/// Email service implementation for sending receipts
/// </summary>
public class EmailService : IEmailService
{
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _configuration;
    private readonly ILogger<EmailService> _logger;

    public EmailService(
        ApplicationDbContext context,
        IConfiguration configuration,
        ILogger<EmailService> logger)
    {
        _context = context;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<bool> SendReceiptEmailAsync(int saleId)
    {
        try
        {
            // Fetch the sale with all related data
            var sale = await _context.Sales
                .Include(s => s.Customer)
                .Include(s => s.SaleDetails)
                    .ThenInclude(sd => sd.Product)
                .FirstOrDefaultAsync(s => s.Id == saleId);

            if (sale == null)
            {
                _logger.LogError($"Sale with ID {saleId} not found");
                return false;
            }

            if (sale.Customer?.Email == null)
            {
                _logger.LogError($"Customer email not found for sale {saleId}");
                return false;
            }

            // Generate email content
            var emailBody = GenerateReceiptHtml(sale);
            var subject = $"Receipt for Order #{sale.Id} - Firmeza";

            // Send email
            await SendEmailAsync(sale.Customer.Email, subject, emailBody);

            _logger.LogInformation($"Receipt email sent successfully for sale {saleId}");
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error sending receipt email for sale {saleId}");
            return false;
        }
    }

    private string GenerateReceiptHtml(Data.Models.Sale sale)
    {
        var sb = new StringBuilder();
        
        sb.AppendLine("<!DOCTYPE html>");
        sb.AppendLine("<html>");
        sb.AppendLine("<head>");
        sb.AppendLine("<style>");
        sb.AppendLine("body { font-family: Arial, sans-serif; line-height: 1.6; color: #333; max-width: 600px; margin: 0 auto; padding: 20px; }");
        sb.AppendLine(".header { background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); color: white; padding: 30px; text-align: center; border-radius: 10px 10px 0 0; }");
        sb.AppendLine(".content { background: #f9f9f9; padding: 30px; border-radius: 0 0 10px 10px; }");
        sb.AppendLine("table { width: 100%; border-collapse: collapse; margin: 20px 0; background: white; }");
        sb.AppendLine("th, td { padding: 12px; text-align: left; border-bottom: 1px solid #ddd; }");
        sb.AppendLine("th { background-color: #667eea; color: white; }");
        sb.AppendLine(".total-row { font-weight: bold; font-size: 1.2em; background-color: #f0f0f0; }");
        sb.AppendLine(".footer { text-align: center; margin-top: 30px; color: #777; font-size: 0.9em; }");
        sb.AppendLine("</style>");
        sb.AppendLine("</head>");
        sb.AppendLine("<body>");
        
        // Header
        sb.AppendLine("<div class='header'>");
        sb.AppendLine("<h1>🧾 Receipt</h1>");
        sb.AppendLine("<p>Thank you for your purchase!</p>");
        sb.AppendLine("</div>");
        
        // Content
        sb.AppendLine("<div class='content'>");
        sb.AppendLine($"<p><strong>Order Number:</strong> #{sale.Id}</p>");
        sb.AppendLine($"<p><strong>Date:</strong> {sale.SaleDate:MMMM dd, yyyy}</p>");
        sb.AppendLine($"<p><strong>Customer:</strong> {sale.Customer?.Name}</p>");
        sb.AppendLine($"<p><strong>Payment Method:</strong> {sale.PaymentMethod ?? "N/A"}</p>");
        
        // Items table
        sb.AppendLine("<h2>Order Items</h2>");
        sb.AppendLine("<table>");
        sb.AppendLine("<tr><th>Item</th><th>Quantity</th><th>Price</th><th>Subtotal</th></tr>");
        
        foreach (var detail in sale.SaleDetails)
        {
            sb.AppendLine("<tr>");
            sb.AppendLine($"<td>{detail.Product?.Name ?? "Unknown Product"}</td>");
            sb.AppendLine($"<td>{detail.Quantity}</td>");
            sb.AppendLine($"<td>${detail.UnitPrice:F2}</td>");
            sb.AppendLine($"<td>${detail.SubTotal:F2}</td>");
            sb.AppendLine("</tr>");
        }
        
        sb.AppendLine("</table>");
        
        // Totals
        sb.AppendLine("<table>");
        sb.AppendLine($"<tr><td>Subtotal:</td><td>${sale.SubTotal:F2}</td></tr>");
        sb.AppendLine($"<tr><td>Tax:</td><td>${sale.Tax:F2}</td></tr>");
        sb.AppendLine($"<tr><td>Discount:</td><td>-${sale.Discount:F2}</td></tr>");
        sb.AppendLine($"<tr class='total-row'><td>Total:</td><td>${sale.Total:F2}</td></tr>");
        sb.AppendLine("</table>");
        
        if (!string.IsNullOrEmpty(sale.Notes))
        {
            sb.AppendLine($"<p><strong>Notes:</strong> {sale.Notes}</p>");
        }
        
        sb.AppendLine("</div>");
        
        // Footer
        sb.AppendLine("<div class='footer'>");
        sb.AppendLine("<p>Thank you for shopping with Firmeza!</p>");
        sb.AppendLine("<p>If you have any questions, please contact us.</p>");
        sb.AppendLine("</div>");
        
        sb.AppendLine("</body>");
        sb.AppendLine("</html>");
        
        return sb.ToString();
    }

    private async Task SendEmailAsync(string toEmail, string subject, string htmlBody)
    {
        var smtpServer = _configuration["Email:SmtpServer"] ?? "smtp.gmail.com";
        var smtpPort = int.Parse(_configuration["Email:SmtpPort"] ?? "587");
        var senderEmail = _configuration["Email:SenderEmail"] ?? "noreply@firmeza.com";
        var senderName = _configuration["Email:SenderName"] ?? "Firmeza Store";
        var username = _configuration["Email:Username"];
        var password = _configuration["Email:Password"];

        using var message = new MailMessage();
        message.From = new MailAddress(senderEmail, senderName);
        message.To.Add(toEmail);
        message.Subject = subject;
        message.Body = htmlBody;
        message.IsBodyHtml = true;

        using var smtpClient = new SmtpClient(smtpServer, smtpPort);
        smtpClient.EnableSsl = true;
        smtpClient.UseDefaultCredentials = false;
        
        // Only set credentials if username and password are provided
        if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
        {
            smtpClient.Credentials = new NetworkCredential(username, password);
        }

        await smtpClient.SendMailAsync(message);
    }
}
