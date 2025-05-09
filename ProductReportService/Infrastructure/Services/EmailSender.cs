using Application.Interfaces;
using Infrastructure.EmailConfig;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Infrastructure.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly MailSettings _mailSettings;

        public EmailSender(IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }

        public async Task SendEmailWithAttachmentAsync(
            string recipient, 
            string subject, 
            string body, 
            byte[] attachment, 
            string attachmentName)
        {
            var message = new MimeMessage();
            
            // Set the sender
            message.From.Add(new MailboxAddress(_mailSettings.SenderName, _mailSettings.SenderEmail));
            
            // Set the recipient
            message.To.Add(new MailboxAddress("", recipient));
            
            // Set subject
            message.Subject = subject;

            // Create multipart/mixed container to hold the body and attachment
            var multipart = new Multipart("mixed");
            
            // Add the body
            var textPart = new TextPart("plain")
            {
                Text = body
            };
            multipart.Add(textPart);

            // Add the attachment
            var attachmentPart = new MimePart("application", "vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            {
                Content = new MimeContent(new MemoryStream(attachment)),
                ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                ContentTransferEncoding = ContentEncoding.Base64,
                FileName = attachmentName
            };
            multipart.Add(attachmentPart);
            
            // Set the multipart as the message body
            message.Body = multipart;

            // Send the email
            using var client = new SmtpClient();
            await client.ConnectAsync(_mailSettings.Server, _mailSettings.Port, SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(_mailSettings.Username, _mailSettings.Password);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
    }
} 