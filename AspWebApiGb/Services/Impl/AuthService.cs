using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AspWebApiGb.Models;
using AspWebApiGb.Models.Dto;
using AspWebApiGb.Models.Requests;
using AspWebApiGb.Utils;
using EmployeeServiceData;
using Microsoft.IdentityModel.Tokens;

namespace AspWebApiGb.Services.Impl;

public class AuthService : IAuthService
{
    internal const string SecretKey = "Fm93!0ajfZ,cm9DEj9aSk1@jf$j9fqPjR9@#";
    
    private readonly IServiceScopeFactory _serviceScopeFactory;
    
    private readonly Dictionary<string, SessionDto> _sessions = new Dictionary<string, SessionDto>();

    public AuthService(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }
    
    public SessionDto? GetSession(string sessionToken)
    {
        SessionDto sessionDto;

        lock (_sessions)
        {
            _sessions.TryGetValue(sessionToken, out sessionDto);
        }

        bool sessionWasFound = (sessionDto != null);
        
        if (!sessionWasFound)
        {
            using IServiceScope serviceScope = _serviceScopeFactory.CreateScope();
            EmployeeServiceDbContext serviceDbContext = serviceScope
                .ServiceProvider
                .GetRequiredService<EmployeeServiceDbContext>();

            AccountSession? session = serviceDbContext
                .AccountSessions
                .FirstOrDefault(
                    item => item.SessionToken == sessionToken
                );

            if (session == null)
                return null;

            Account account = serviceDbContext.Accounts.FirstOrDefault(
                account => account.AccountId == session.AccountId
                );

            sessionDto = GetSessionDto(account, session);

            lock (_sessions)
            {
                _sessions[sessionToken] = sessionDto;
            }
            
            return sessionDto;
        }

        return sessionDto;
    }
    
    public AuthResponse Login(AuthRequest authRequest)
    {
        using IServiceScope serviceScope = _serviceScopeFactory.CreateScope();
        EmployeeServiceDbContext serviceDbContext = serviceScope
            .ServiceProvider
            .GetRequiredService<EmployeeServiceDbContext>();

        Account account = FindAccountByLogin(serviceDbContext, authRequest.Login);

        bool accountExists = (account != null);
        if (!accountExists)
        {
            return new AuthResponse()
            {
                Status = AuthStatus.UserNotFound
            };
        }

        bool passwordIsCorrect = PasswordUtils.VerifyPassword(
                authRequest.Password,
                account.PasswordSalt,
                account.PasswordHash
                );
        if (!passwordIsCorrect)
        {
            return new AuthResponse()
            {
                Status = AuthStatus.InvalidPassword
            };
        }

        AccountSession session = new AccountSession()
        {
            AccountId = account.AccountId,
            SessionToken = CreateSessionToken(account),
            TimeCreated = DateTime.Now,
            TimeLastRequest = DateTime.Now,
            IsClosed = false
        };

        serviceDbContext.AccountSessions.Add(session);
        serviceDbContext.SaveChanges();

        SessionDto sessionDto = GetSessionDto(account, session);

        lock (_sessions)
        {
            _sessions[sessionDto.SessionToken] = sessionDto;
        }
        
        return new AuthResponse()
        {
            Session = sessionDto,
            Status = AuthStatus.Success
        };
    }

    private Account FindAccountByLogin(EmployeeServiceDbContext context, string login)
    {
        return context
            .Accounts
            .FirstOrDefault(account => account.EMail == login);
    }

    private string CreateSessionToken(Account account)
    {
        JwtSecurityTokenHandler jwtSecurityTokenHandler = new ();
        
        byte[] key = Encoding.ASCII.GetBytes(SecretKey);

        SecurityTokenDescriptor securityTokenDescriptor = new ()
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, account.AccountId.ToString()),
                new Claim(ClaimTypes.Email, account.EMail)
            }),
            Expires = DateTime.UtcNow.AddDays(1),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
        };

        SecurityToken securityToken = jwtSecurityTokenHandler.CreateToken(securityTokenDescriptor);

        return jwtSecurityTokenHandler.WriteToken(securityToken);
    }

    private SessionDto GetSessionDto(Account account, AccountSession accountSession)
    {
        return new SessionDto()
        {
            SessionId = accountSession.AccountId,
            SessionToken = accountSession.SessionToken,
            Account = new AccountDto()
            {
                AccountId = account.AccountId,
                EMail = account.EMail,
                FirstName = account.FirstName,
                Blocked = account.Blocked,
                SecondName = account.SecondName
            }
        };
    }
}