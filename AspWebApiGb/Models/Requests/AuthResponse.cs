using AspWebApiGb.Models.Dto;

namespace AspWebApiGb.Models.Requests;

public class AuthResponse
{
    public AuthStatus Status { get; set; }
    public SessionDto Session { get; set; }
}