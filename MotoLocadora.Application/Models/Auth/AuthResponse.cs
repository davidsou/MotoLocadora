namespace MotoLocadora.Application.Models.Auth;

public class AuthResponse
{
    public string Token { get; set; } = string.Empty;
    public DateTime Expires { get; set; }
    public string RefreshToken { get; set; } = string.Empty;
    public IEnumerable<string> Roles { get; set; } = new List<string>();
}
