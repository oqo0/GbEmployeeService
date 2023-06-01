using AspWebApiGbProto;
using Grpc.Core;

namespace AspWebApiGb.Services.GRPC;

public class DepartmentService : AspWebApiGbProto.DepartmentService.DepartmentServiceBase
{
    #region Services

    private readonly IDepartmentRepository _departmentRepository;

    #endregion

    #region Constructors

    public DepartmentService(IDepartmentRepository departmentRepository)
    {
        _departmentRepository = departmentRepository;
    }

    #endregion

    public override Task<CreateDepartmentResponse> CreateDepartment(CreateDepartmentRequest request, ServerCallContext context)
    {
        Guid createdDepartmentId = _departmentRepository.Create(new EmployeeServiceData.Department()
        {
            Description = request.Description
        });

        var response = new CreateDepartmentResponse();
        response.Id = createdDepartmentId.ToString();
        
        return Task.FromResult(response);
    }

    public override Task<GetAllDepartmentsResponse> GetAllDepartments(GetAllDepartmentsRequest request, ServerCallContext context)
    {
        var departments = _departmentRepository
            .GetAll()
            .Select(
                x => new Department()
                {
                    Description = x.Description,
                    Id = x.Id.ToString()
                }
            )
            .ToList();

        var response = new GetAllDepartmentsResponse();
        response.Departments.AddRange(departments.ToList());

        return Task.FromResult(response);
    }

    public override Task<DeleteDepartmentResponse> DeleteDepartment(DeleteDepartmentRequest request, ServerCallContext context)
    {
        _departmentRepository.Delete(new Guid(request.Id));
        
        return Task.FromResult(
            new DeleteDepartmentResponse()
        );
    }
}