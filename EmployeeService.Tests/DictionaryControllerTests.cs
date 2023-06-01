using System.Collections.Generic;
using AspWebApiGb.Controllers;
using AspWebApiGb.Services;
using EmployeeServiceData;
using Moq;

namespace EmployeeService.Tests;

public class DictionaryControllerTests
{
    private readonly Mock<IEmployeeTypeRepository> _mockEmployeeTypeRepository;
    private readonly DictionaryController _dictionaryController;

    public DictionaryControllerTests()
    {
        _mockEmployeeTypeRepository = new Mock<IEmployeeTypeRepository>();
        
        _dictionaryController = new DictionaryController(_mockEmployeeTypeRepository.Object);
    }
    
    [Theory]
    [InlineData("UserName")]
    [InlineData("Another user")]
    public void CreateEmployeeTypeTest(string description)
    {
        _mockEmployeeTypeRepository.Setup(
            repo =>
                repo.Create(It.IsAny<EmployeeType>()
                ))        
            .Verifiable();

        var result = _dictionaryController.CreateEmployeeType(description);
        
        _mockEmployeeTypeRepository.Verify(
            repo => repo.Create(It.IsAny<EmployeeType>()),
            Times.Once()
            );
    }

    [Fact]
    public void GetAllEmployeeTypeTest()
    {
        _mockEmployeeTypeRepository.Setup(repo => repo.GetAll())
            .Returns(new List<EmployeeType>());
        
        var result = _dictionaryController.GetAllEmployeeTypes();
        
        //Assert.IsAssignableFrom<ActionResult<IList<EmployeeTypeDto>>>(result);
        
        _mockEmployeeTypeRepository.Verify(
            repo => repo.GetAll(),
            Times.Once());
    }
    
    [Theory]
    [InlineData(1)]
    public void DeleteEmployeeTypeTest(int id)
    {
        _mockEmployeeTypeRepository.Setup(repo =>
            repo.Delete(1)).Verifiable();
        
        var result = _dictionaryController.DeleteEmployeeType(id);
        
        Assert.IsAssignableFrom<bool>(result);
    }
}