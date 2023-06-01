using AspWebApiGbProto;
using Grpc.Core;

namespace AspWebApiGb.Services.GRPC;

public class DictionaryService : DictionariesService.DictionariesServiceBase
{
    #region Services

    private readonly IEmployeeTypeRepository _employeeTypeRepository;

    #endregion

    #region Constructors

    public DictionaryService(IEmployeeTypeRepository employeeTypeRepository)
    {
        _employeeTypeRepository = employeeTypeRepository;
    }

    #endregion

    public override Task<CreateEmployeeTypeResponse> CreateEmployeeType(CreateEmployeeTypeRequest request, ServerCallContext context)
    {
        var createdEmployeeId = _employeeTypeRepository.Create(
            new EmployeeServiceData.EmployeeType()
            {
                Description = request.Description
            }
        );

        var response = new CreateEmployeeTypeResponse
        {
            Id = createdEmployeeId
        };

        return Task.FromResult(response);
    }
    
    public override Task<GetAllEmployeeTypesResponse> GetAllEmployeeTypes(GetAllEmployeeTypesRequest request, ServerCallContext context)
    {
        var employeeTypes = _employeeTypeRepository
            .GetAll()
            .Select(
                x => new EmployeeType()
                {
                    Description = x.Description,
                    Id = x.Id
                }
            ).ToList();

        var response = new GetAllEmployeeTypesResponse();
        response.EmployeeTypes.AddRange(employeeTypes);

        return Task.FromResult(response);
    }
    
    public override Task<DeleteEmployeeTypeResponse> DeleteEmployeeType(DeleteEmployeeTypeRequest request, ServerCallContext context)
    {
        _employeeTypeRepository.Delete(request.Id);

        return Task.FromResult(
            new DeleteEmployeeTypeResponse()
            );
    }
}