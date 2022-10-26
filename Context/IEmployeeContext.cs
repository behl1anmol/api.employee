using api.employee.Entity;
using Microsoft.EntityFrameworkCore;

namespace api.employee.Context;

public interface IEmployeeContext
{
    public DbSet<Employee> Employees { get; set; }
}
