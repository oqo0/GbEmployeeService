using EmployeeServiceData;
using AspWebApiGb.Models.Dto;
using AspWebApiGb.Services;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AspWebApiGb.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class EmployeeController : Controller
{
    #region Services

    private readonly IEmployeeRepository _employeeRepository;
    private readonly AbstractValidator<EmployeeDto> _employeeDtoValidator;

    #endregion

    #region Constructors

    public EmployeeController(IEmployeeRepository employeeRepository, AbstractValidator<EmployeeDto> employeeDtoValidator)
    {
        _employeeRepository = employeeRepository;
        _employeeDtoValidator = employeeDtoValidator;
    }

    #endregion

    [HttpPost("employees/create")]
    public ActionResult<int> CreateEmployee([FromQuery] EmployeeDto employeeDto)
    {
        ValidationResult validationResult = _employeeDtoValidator.Validate(employeeDto);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.ToDictionary());
        
        return Ok(
            _employeeRepository.Create(new Employee()
            {
                DepartmentId = employeeDto.DepartmentId,
                EmployeeTypeId = employeeDto.EmployeeTypeId,
                FirstName = employeeDto.FirstName,
                Patronymic = employeeDto.Patronymic,
                Salary = employeeDto.Salary,
                Surname = employeeDto.Surname
            })
        );
    }
    
    [HttpGet("employees/all")]
    public ActionResult<IList<EmployeeDto>> GetAllEmployees()
    {
        return Ok(_employeeRepository
            .GetAll()
            .Select(
                x=> new EmployeeDto()
                {
                    Id = x.Id,
                    DepartmentId = x.DepartmentId,
                    EmployeeTypeId = x.EmployeeTypeId,
                    FirstName = x.FirstName,
                    Patronymic = x.Patronymic,
                    Salary = x.Salary,
                    Surname = x.Surname
                }
            )
            .ToList()
        );
    }
    
    [HttpPut("employees/update")]
    public ActionResult<bool> UpdateEmployee([FromQuery] EmployeeDto employeeDto)
    {
        return Ok(
            _employeeRepository.Update(
                new Employee()
                {
                    DepartmentId = employeeDto.DepartmentId,
                    EmployeeTypeId = employeeDto.EmployeeTypeId,
                    FirstName = employeeDto.FirstName,
                    Patronymic = employeeDto.Patronymic,
                    Salary = employeeDto.Salary,
                    Surname = employeeDto.Surname
                })
        );
    }
    
    [HttpDelete("employees/delete")]
    public ActionResult<bool> DeleteEmployee([FromQuery] int id)
    {
        return Ok(_employeeRepository.Delete(id));
    }
}