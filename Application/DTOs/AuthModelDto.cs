using System.Text.Json.Serialization;

public class AuthModel
{
    public string? Message { set; get; }
    public bool IsAuthenticated { set; get; }
    public string? UserName { set; get; }
    public string? Email { set; get; }
    public string? Role { set; get; }
    public string? Token { set; get; }
    [JsonIgnore]
    public string RefreshToken { set; get; }
    public DateTime RefreshTokenExpiration{ set; get; }
}