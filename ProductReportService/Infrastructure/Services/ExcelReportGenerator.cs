using Application.Interfaces;
using Domain.Entities;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;

namespace Infrastructure.Services
{
    public class ExcelReportGenerator : IExcelReportGenerator
    {
        public ExcelReportGenerator()
        {
            // Set EPPlus license context
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        }

        public byte[] GenerateEmployeeReport(IEnumerable<Employee> employees)
        {
            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Employees");

            // Set up header row
            worksheet.Cells[1, 1].Value = "Business Entity ID";
            worksheet.Cells[1, 2].Value = "National ID Number";
            worksheet.Cells[1, 3].Value = "Login ID";
            worksheet.Cells[1, 4].Value = "Job Title";
            worksheet.Cells[1, 5].Value = "Birth Date";
            worksheet.Cells[1, 6].Value = "Marital Status";
            worksheet.Cells[1, 7].Value = "Gender";
            worksheet.Cells[1, 8].Value = "Hire Date";
            worksheet.Cells[1, 9].Value = "Salaried";
            worksheet.Cells[1, 10].Value = "Vacation Hours";
            worksheet.Cells[1, 11].Value = "Sick Leave Hours";
            worksheet.Cells[1, 12].Value = "Current";
            worksheet.Cells[1, 13].Value = "Modified Date";

            // Style the header
            using (var range = worksheet.Cells[1, 1, 1, 13])
            {
                range.Style.Font.Bold = true;
                range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                range.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                range.Style.Font.Color.SetColor(Color.Black);
                range.Style.Border.BorderAround(ExcelBorderStyle.Thin);
            }

            // Add data rows
            int row = 2;
            foreach (var employee in employees)
            {
                worksheet.Cells[row, 1].Value = employee.BusinessEntityID;
                worksheet.Cells[row, 2].Value = employee.NationalIDNumber;
                worksheet.Cells[row, 3].Value = employee.LoginID;
                worksheet.Cells[row, 4].Value = employee.JobTitle;
                worksheet.Cells[row, 5].Value = employee.BirthDate;
                worksheet.Cells[row, 6].Value = employee.MaritalStatus;
                worksheet.Cells[row, 7].Value = employee.Gender;
                worksheet.Cells[row, 8].Value = employee.HireDate;
                worksheet.Cells[row, 9].Value = employee.SalariedFlag ? "Yes" : "No";
                worksheet.Cells[row, 10].Value = employee.VacationHours;
                worksheet.Cells[row, 11].Value = employee.SickLeaveHours;
                worksheet.Cells[row, 12].Value = employee.CurrentFlag ? "Yes" : "No";
                worksheet.Cells[row, 13].Value = employee.ModifiedDate;
                
                // Format date columns
                worksheet.Cells[row, 5].Style.Numberformat.Format = "yyyy-MM-dd";
                worksheet.Cells[row, 8].Style.Numberformat.Format = "yyyy-MM-dd";
                worksheet.Cells[row, 13].Style.Numberformat.Format = "yyyy-MM-dd HH:mm:ss";
                
                row++;
            }

            // Auto-size columns
            worksheet.Cells.AutoFitColumns();

            // Convert to byte array
            return package.GetAsByteArray();
        }

        public byte[] GeneratePurchasingManagerReport(IEnumerable<PurchasingManager> purchasingManagers)
        {
            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Purchasing Managers");

            // Set up header row
            worksheet.Cells[1, 1].Value = "Business Entity ID";
            worksheet.Cells[1, 2].Value = "Last Name";
            worksheet.Cells[1, 3].Value = "First Name";

            // Style the header
            using (var range = worksheet.Cells[1, 1, 1, 3])
            {
                range.Style.Font.Bold = true;
                range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                range.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                range.Style.Font.Color.SetColor(Color.Black);
                range.Style.Border.BorderAround(ExcelBorderStyle.Thin);
            }

            // Add data rows
            int row = 2;
            foreach (var manager in purchasingManagers)
            {
                worksheet.Cells[row, 1].Value = manager.BusinessEntityID;
                worksheet.Cells[row, 2].Value = manager.LastName;
                worksheet.Cells[row, 3].Value = manager.FirstName;
                
                row++;
            }

            // Auto-size columns
            worksheet.Cells.AutoFitColumns();

            // Convert to byte array
            return package.GetAsByteArray();
        }
    }
} 