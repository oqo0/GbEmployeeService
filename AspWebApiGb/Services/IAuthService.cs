using AspWebApiGb.Models.Dto;
using AspWebApiGb.Models.Requests;

namespace AspWebApiGb.Services;

public interface IAuthService
{
    public AuthResponse? Login(AuthRequest authRequest);

    public SessionDto GetSession(string sessionToken);
}