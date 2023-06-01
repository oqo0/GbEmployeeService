using Microsoft.EntityFrameworkCore;

namespace EmployeeServiceData;

public class EmployeeServiceDbContext : DbContext
{
    public DbSet<EmployeeType> EmployeeTypes { get; set; }

    public DbSet<Department> Departments { get; set; }
    
    public DbSet<Employee> Employees { get; set; }
    
    public DbSet<Account> Accounts { get; set; }
    
    public DbSet<AccountSession> AccountSessions { get; set; }

    public EmployeeServiceDbContext(DbContextOptions<EmployeeServiceDbContext> options) : base(options) { }
}