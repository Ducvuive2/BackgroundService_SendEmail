using Application.Interfaces;
using Domain.Entities;

namespace Application.Services
{
    public class EmployeeReportService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IExcelReportGenerator _excelReportGenerator;
        private readonly IEmailSender _emailSender;

        public EmployeeReportService(
            IEmployeeRepository employeeRepository,
            IExcelReportGenerator excelReportGenerator,
            IEmailSender emailSender)
        {
            _employeeRepository = employeeRepository;
            _excelReportGenerator = excelReportGenerator;
            _emailSender = emailSender;
        }

        public async Task GenerateAndSendEmployeeReportAsync()
        {
            // Get all employees ordered by job title
            IEnumerable<Employee> employees = await _employeeRepository.GetAllEmployeesOrderedByJobTitleAsync();
            
            // Generate Excel report
            byte[] reportData = _excelReportGenerator.GenerateEmployeeReport(employees);
            
            // Send email with Excel attachment
            await _emailSender.SendEmailWithAttachmentAsync(
                recipient: "duc2006200101@gmail.com",
                subject: "Adventure Works - Employee Report",
                body: "Please find attached the employee report sorted by job title.",
                attachment: reportData,
                attachmentName: $"EmployeeReport_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx");
        }
    }
}