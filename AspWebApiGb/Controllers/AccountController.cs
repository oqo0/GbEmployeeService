using AspWebApiGb.Models.Dto;
using AspWebApiGb.Services;
using AspWebApiGb.Utils;
using EmployeeServiceData;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AspWebApiGb.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class AccountController : Controller
{
    #region Services

    private readonly IAccountRepository _accountRepository;
    private readonly IValidator<AccountDto> _accountDtoValidator;
    private readonly IValidator<string> _passwordValidator;

    #endregion

    #region Constructors

    public AccountController(
        IAccountRepository accountRepository,
        IValidator<AccountDto> accountDtoValidator,
        IValidator<string> passwordValidator)
    {
        _accountRepository = accountRepository;
        _accountDtoValidator = accountDtoValidator;
        _passwordValidator = passwordValidator;
    }

    #endregion

    [HttpPost("auth/admin/create")]
    public ActionResult<int> CreateAccount([FromBody] AccountDto accountDto, string password)
    {
        var accountDtoValidationRes = _accountDtoValidator.Validate(accountDto);
        var passwordValidationRes = _passwordValidator.Validate(password);

        if (!accountDtoValidationRes.IsValid)
            return BadRequest(accountDtoValidationRes.ToDictionary());

        if (!passwordValidationRes.IsValid)
            return BadRequest(passwordValidationRes.ToDictionary());

        
        (string passwordSalt, string passwordHash) = PasswordUtils.CreatePasswordHash(password);

        Account account = new Account()
        {
            EMail = accountDto.EMail,
            FirstName = accountDto.FirstName,
            SecondName = accountDto.SecondName,
            LastName = accountDto.LastName,
            Blocked = accountDto.Blocked,
            PasswordSalt = passwordSalt,
            PasswordHash = passwordHash
        };

        return _accountRepository.Create(account);
    }
    
    [HttpPost("auth/admin/get_all_accounts")]
    public ActionResult<IList<Account>> GetAllAccounts()
    {
        return Ok(_accountRepository
            .GetAll()
            .Select(acc => new AccountDto()
            {
                AccountId = acc.AccountId,
                EMail = acc.EMail,
                FirstName = acc.FirstName,
                SecondName = acc.SecondName,
                LastName = acc.LastName,
                Blocked = acc.Blocked,
            })
            .ToList());
    }
    
    [HttpPost("auth/admin/update")]
    public ActionResult<bool> UpdateAccount([FromBody] AccountDto accountDto)
    {
        var accountDtoValidationRes = _accountDtoValidator.Validate(accountDto);

        if (!accountDtoValidationRes.IsValid)
            return BadRequest(accountDtoValidationRes.ToDictionary());
        
        Account account = new Account()
        {
            AccountId = accountDto.AccountId,
            EMail = accountDto.EMail,
            FirstName = accountDto.FirstName,
            SecondName = accountDto.SecondName,
            LastName = accountDto.LastName,
            Blocked = accountDto.Blocked
        };

        return Ok(_accountRepository.Update(account));
    }
    
    [HttpPost("auth/admin/delete")]
    public ActionResult<bool> DeleteAccount(int id)
    {
        return _accountRepository.Delete(id);
    }
}