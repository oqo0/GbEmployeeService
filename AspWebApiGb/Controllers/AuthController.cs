using System.Net.Http.Headers;
using AspWebApiGb.Models;
using AspWebApiGb.Models.Dto;
using AspWebApiGb.Models.Requests;
using AspWebApiGb.Services;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace AspWebApiGb.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class AuthController : Controller
{
    #region Services

    private readonly IAuthService _authService;
    private readonly IValidator<AuthRequest> _authRequestValidator;

    #endregion

    #region Constructor

    public AuthController(IAuthService authService, IValidator<AuthRequest> authRequestValidator)
    {
        _authService = authService;
        _authRequestValidator = authRequestValidator;
    }

    #endregion

    [AllowAnonymous]
    [HttpPost("auth/login")]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IList<ValidationFailure>), StatusCodes.Status400BadRequest)]
    public IActionResult Login([FromBody] AuthRequest authRequest)
    {
        var validationResult = _authRequestValidator.Validate(authRequest);

        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.ToDictionary());
        }
        
        var response = _authService.Login(authRequest);

        if (response.Status == AuthStatus.Success)
        {
            Response.Headers.Add("X-Session-Token", response.Session.SessionToken);
        }
        
        return Ok(response);
    }
    
    [ProducesResponseType(typeof(SessionDto), StatusCodes.Status200OK)]
    [HttpGet("auth/get_session")]
    public IActionResult GetSession()
    {
        var authHeader = Request.Headers[HeaderNames.Authorization];

        if (AuthenticationHeaderValue.TryParse(authHeader, out var headerValue))
        {
            var scheme = headerValue.Scheme;
            var sessionToken = headerValue.Parameter;

            if (string.IsNullOrEmpty(sessionToken))
                return Unauthorized();

            SessionDto sessionDto = _authService.GetSession(sessionToken);

            if (sessionDto == null)
                return Unauthorized();

            return Ok(sessionDto);
        }
        
        return Unauthorized();
    }
}