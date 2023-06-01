using EmployeeServiceData;

namespace AspWebApiGb.Services.Impl;

public class EmployeeRepository : IEmployeeRepository
{
    #region Services

    private EmployeeServiceDbContext _context;

    #endregion

    #region Constructor

    public EmployeeRepository(EmployeeServiceDbContext context)
    {
        _context = context;
    }

    #endregion
    
    public int Create(Employee data)
    {
        _context.Add(data);
        _context.SaveChanges();

        return data.Id;
    }

    public IList<Employee> GetAll()
    {
        return _context.Employees.ToList();
    }

    public Employee GetById(int id)
    {
        return _context.Employees.FirstOrDefault(emp => emp.Id == id);
    }
    
    public bool Update(Employee data)
    {
        Employee employee = GetById(data.Id);

        if (employee == null)
        {
            return false;
        }

        employee.DepartmentId = data.DepartmentId;
        employee.EmployeeTypeId = data.EmployeeTypeId;
        employee.FirstName = data.FirstName;
        employee.Surname = data.Surname;
        employee.Patronymic = data.Patronymic;
        employee.Salary = data.Salary;
        
        int amountOfChanges = _context.SaveChanges();

        return amountOfChanges != 0;
    }

    public bool Delete(int id)
    {
        Employee employee = GetById(id);

        if (employee == null)
        {
            return false;
        }

        _context.Remove(employee);
        _context.SaveChanges();
        
        return true;
    }
}