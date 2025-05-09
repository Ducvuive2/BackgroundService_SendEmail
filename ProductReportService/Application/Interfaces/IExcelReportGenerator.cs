using Domain.Entities;

namespace Application.Interfaces
{
    public interface IExcelReportGenerator
    {
        byte[] GenerateEmployeeReport(IEnumerable<Employee> employees);
        byte[] GeneratePurchasingManagerReport(IEnumerable<PurchasingManager> purchasingManagers);
    }
} 