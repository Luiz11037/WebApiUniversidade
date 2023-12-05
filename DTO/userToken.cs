using System.Data;

namespace apiUniversidade.DTO;

public class UserToken{
    public bool Authenticated { get; set; }
    public DateTime Expiration { get; set; }
    public string? Token { get; set; }
    public string? Message { get; set; }
}