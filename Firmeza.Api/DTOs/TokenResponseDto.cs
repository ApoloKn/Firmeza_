namespace Firmeza.Api.DTOs;

/// <summary>
/// DTO for JWT token response
/// </summary>
public class TokenResponseDto
{
    public string Token { get; set; } = string.Empty;
    
    public DateTime Expiration { get; set; }
    
    public string Username { get; set; } = string.Empty;
    
    public List<string> Roles { get; set; } = new();
}
