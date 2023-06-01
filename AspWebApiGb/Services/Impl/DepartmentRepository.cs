using EmployeeServiceData;

namespace AspWebApiGb.Services.Impl;

public class DepartmentRepository : IDepartmentRepository
{
    #region Services

    private readonly EmployeeServiceDbContext _context;

    #endregion

    #region Constructor

    public DepartmentRepository(EmployeeServiceDbContext context)
    {
        _context = context;
    }

    #endregion
    
    public Guid Create(Department data)
    {
        _context.Add(data);
        _context.SaveChanges();

        return data.Id;
    }

    public IList<Department> GetAll()
    {
        return _context.Departments.ToList();
    }

    public Department GetById(Guid id)
    {
        return _context.Departments.FirstOrDefault(dep => dep.Id == id);
    }
    
    public bool Update(Department data)
    {
        Department department = GetById(data.Id);

        if (department == null)
            return false;

        department.Description = data.Description;
        int amountOfChanges = _context.SaveChanges();

        return amountOfChanges != 0;
    }

    public bool Delete(Guid id)
    {
        Department department = GetById(id);

        if (department == null)
            return false;

        _context.Remove(department);
        _context.SaveChanges();

        return true;
    }
}