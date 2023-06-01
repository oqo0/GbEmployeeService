using EmployeeServiceData;
using AspWebApiGb.Models.Dto;
using AspWebApiGb.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AspWebApiGb.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class DictionaryController : Controller
{
    #region Services

    private readonly IEmployeeTypeRepository _employeeTypeRepository;

    #endregion

    #region Constructors

    public DictionaryController(IEmployeeTypeRepository employeeTypeRepository)
    {
        _employeeTypeRepository = employeeTypeRepository;
    }

    #endregion

    [HttpPost("employee-types/create")]
    public ActionResult<int> CreateEmployeeType([FromQuery] string description)
    {
        return Ok(_employeeTypeRepository
            .Create(new EmployeeType()
            {
                Description = description
            })
        );
    }

    [HttpGet("employee-types/all")]
    public ActionResult<IList<EmployeeTypeDto>> GetAllEmployeeTypes()
    {
        return Ok(
            _employeeTypeRepository
                .GetAll()
                .Select(
                    x=> new EmployeeTypeDto()
                    {
                        Description = x.Description,
                        Id = x.Id
                    }
                ).ToList()
        );
    }
    
    [HttpPut("employee-types/update")]
    public ActionResult<bool> UpdateEmployeeType([FromQuery] int id, string description)
    {
        return Ok(
            _employeeTypeRepository.Update(
                    new EmployeeType()
                    {
                        Id = id,
                        Description = description
                    })
            );
    }
    
    [HttpDelete("employee-types/delete")]
    public ActionResult<bool> DeleteEmployeeType([FromQuery] int id)
    {
        return Ok(_employeeTypeRepository.Delete(id));
    }
}