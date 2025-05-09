using Application.Interfaces;
using Domain.Entities;

namespace Application.Services
{
    public class PurchasingManagerReportService
    {
        private readonly IPurchasingManagerRepository _purchasingManagerRepository;
        private readonly IExcelReportGenerator _excelReportGenerator;
        private readonly IEmailSender _emailSender;

        public PurchasingManagerReportService(
            IPurchasingManagerRepository purchasingManagerRepository,
            IExcelReportGenerator excelReportGenerator,
            IEmailSender emailSender)
        {
            _purchasingManagerRepository = purchasingManagerRepository;
            _excelReportGenerator = excelReportGenerator;
            _emailSender = emailSender;
        }

        public async Task GenerateAndSendPurchasingManagerReportAsync()
        {
            // Get all purchasing managers
            IEnumerable<PurchasingManager> purchasingManagers = await _purchasingManagerRepository.GetAllPurchasingManagersAsync();
            
            // Generate Excel report
            byte[] reportData = _excelReportGenerator.GeneratePurchasingManagerReport(purchasingManagers);
            
            // Send email with Excel attachment
            await _emailSender.SendEmailWithAttachmentAsync(
                recipient: "duc2006200101@gmail.com",
                subject: "Adventure Works - Purchasing Manager Report",
                body: "Please find attached the Purchasing Manager report.",
                attachment: reportData,
                attachmentName: $"PurchasingManagerReport_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx");
        }
    }
} 