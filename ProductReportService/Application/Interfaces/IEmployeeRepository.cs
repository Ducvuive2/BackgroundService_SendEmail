using Domain.Entities;

namespace Application.Interfaces
{
    public interface IEmployeeRepository
    {
        Task<IEnumerable<Employee>> GetAllEmployeesOrderedByJobTitleAsync();
    }
} 