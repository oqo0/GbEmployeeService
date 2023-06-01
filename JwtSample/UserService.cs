using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace JwtSample;

internal class UserService
{
    private const string SecretKey = "fn03j9r0Dn89hmFj49hfFj89h3$hjf34fg34g";
    
    private IDictionary<string, string> _users = new Dictionary<string, string>()
    {
        { "user1", "pass1" },
        { "user2", "pass1123" },
        { "user3", "pas46s1125" },
        { "user4", "123456" },
        { "user5", "1234567" }
    };

    public string Login(string login, string password)
    {
        if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
            return string.Empty;

        int count = 0;
        foreach (var user in _users)
        {
            bool loginIsCorrect = string.CompareOrdinal(user.Key, login) == 0 &&
                                string.CompareOrdinal(user.Value, password) == 0;
            
            if (loginIsCorrect)
            {
                return GenerateJwtToken(count, login);
            }

            count++;
        }

        return string.Empty;
    }

    private string GenerateJwtToken(int id, string userName)
    {
        JwtSecurityTokenHandler jwtSecurityTokenHandler = new ();
        
        byte[] key = Encoding.ASCII.GetBytes(SecretKey);

        SecurityTokenDescriptor securityTokenDescriptor = new ()
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, userName),
                new Claim(ClaimTypes.NameIdentifier, id.ToString())
            }),
            Expires = DateTime.UtcNow.AddMinutes(15),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
        };

        SecurityToken securityToken = jwtSecurityTokenHandler.CreateToken(securityTokenDescriptor);

        return jwtSecurityTokenHandler.WriteToken(securityToken);
    }
}