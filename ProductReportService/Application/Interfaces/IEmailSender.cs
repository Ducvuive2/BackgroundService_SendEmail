namespace Application.Interfaces
{
    public interface IEmailSender
    {
        Task SendEmailWithAttachmentAsync(
            string recipient,
            string subject,
            string body,
            byte[] attachment,
            string attachmentName);
    }
} 