using AspWebApiGb.Models.Dto;
using AspWebApiGb.Services;
using EmployeeServiceData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AspWebApiGb.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class DepartmentController : Controller
{
    #region Services

    private readonly IDepartmentRepository _departmentRepository;
    private readonly ILogger<DepartmentController> _logger;

    #endregion

    #region Constructors

    public DepartmentController(IDepartmentRepository departmentRepository, ILogger<DepartmentController> logger)
    {
        _departmentRepository = departmentRepository;
        _logger = logger;
    }

    #endregion

    [HttpPost("departments/create")]
    public ActionResult<int> CreateDepartment([FromQuery] string description)
    {
        return Ok(
            _departmentRepository.Create(
                new Department()
                {
                    Description = description
                })
        );
    }

    [HttpGet("departments/all")]
    public ActionResult<IList<DepartmentDto>> GetAllDepartments()
    {
        return Ok(_departmentRepository
            .GetAll()
            .Select(
                x=> new DepartmentDto()
                {
                    Description = x.Description, Id = x.Id
                }
            )
            .ToList()
        );
    }
    
    [HttpPut("departments/update")]
    public ActionResult<bool> UpdateDepartment([FromQuery] Guid id, string description)
    {
        return Ok(
            _departmentRepository.Update(
                new Department()
                {
                    Id = id,
                    Description = description
                })
        );
    }
    
    [HttpDelete("departments/delete")]
    public ActionResult<bool> DeleteDepartment([FromQuery] Guid id)
    {
        return Ok(_departmentRepository.Delete(id));
    }
}